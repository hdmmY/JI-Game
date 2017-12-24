using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeState : MonoBehaviour
{
    [Header("Reference")]
    public Sprite m_whiteSprite;
    public Sprite m_blackSprite;

    [Space]

    [Header("Shield Force Field")]
    public GameObject m_forceField;
    public Material m_whiteForceFieldMaterial;
    public Material m_blackForceFieldMaterial;

    public float m_forceFieldTime;

    [Space]

    [Header("Shock Wave Distortion")]
    public GameObject m_shockWavePrefab;
    public Material m_whiteShockWaveMaterial;
    public Material m_blackShockWaveMaterial;

    [Space]
    [Header("Shock Wave Destroy")]
    [Range(0, 1)]
    public float m_shockWaveRadius;
    public float m_shockWaveTime;
    public AnimationCurve m_shockWaveSpeedCurve;
    [Range(0, 1)]
    public float m_timeSlowTime;
    [Range(0, 1)]
    public float m_timeScaleWhenSlow;


    private PlayerProperty _playerProperty;
    private SpriteRenderer _playerSpriteRender;
    private MeshRenderer _forceFieldRender;

    private void Start()
    {
        _playerProperty = GetComponent<PlayerProperty>();
        _playerSpriteRender = _playerProperty.m_spriteReference;
        _forceFieldRender = m_forceField.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (Input.GetButton("Change State"))
        {
            _playerProperty.m_playerMoveState = PlayerProperty.PlayerMoveType.SlowSpeed;
        }
        else
        {
            _playerProperty.m_playerMoveState = PlayerProperty.PlayerMoveType.HighSpeed;
        }


        if (Input.GetButtonDown("Change State"))
        {

            if (_playerProperty.m_playerState == JIState.Black)
            {
                ChangeState(JIState.White);
            }
            else
            {
                ChangeState(JIState.Black);
            }
        }
    }


    // Use this variable so that prev unfinished UpdateForceField coroutin will be shut down when next UpdateForceField coroutin will be apply.
    private Coroutine _prevUpdateForceFieldCoroutin;
    public void ChangeState(JIState state, bool forceWaveEffect = false)
    {
        _playerProperty.m_playerState = state;

        UpdatePlayerSprite();

        if (_prevUpdateForceFieldCoroutin != null)
        {
            StopCoroutine(_prevUpdateForceFieldCoroutin);
        }
        _prevUpdateForceFieldCoroutin = StartCoroutine(UpdateForceField());

        StartCoroutine(UpdateShockWave(forceWaveEffect));
    }

    private IEnumerator UpdateForceField()
    {
        m_forceField.SetActive(true);

        if (_playerProperty.m_playerState == JIState.Black)
        {
            _forceFieldRender.material = m_blackForceFieldMaterial;
        }
        else
        {
            _forceFieldRender.material = m_whiteForceFieldMaterial;
        }

        float timer = 0f;
        float targetValue = 0f;
        while (timer < m_forceFieldTime)
        {
            timer += JITimer.Instance.DeltTime;

            // Value is between [0, 1f]
            targetValue = Mathf.Clamp01(timer / m_forceFieldTime) * 0.5f;
            _forceFieldRender.material.SetFloat("_LightDirFactor", targetValue);

            yield return null;
        }

        m_forceField.SetActive(false);
    }

    private IEnumerator UpdateShockWave(bool forceWaveEffect)
    {
        // Create a shockwave collider
        GameObject shockWave = Instantiate(m_shockWavePrefab, transform);
        var shockWaveMesh = shockWave.GetComponent<MeshRenderer>();
        var waveCollider = shockWave.GetComponent<CircleCollider2D>();


        // Break if detect none bullet
        if (!forceWaveEffect)
        {
            bool canDestory = InitWaveDestroy(shockWave.GetComponent<JIDestroyArea>());
            if (!canDestory)
            {
                shockWave.SetActive(false);
                Destroy(shockWave);
                yield break;
            }
        }
        else
        {
            shockWave.GetComponent<JIDestroyArea>().m_destroyBulletType = JIState.All;
        }

        // Update shock wave material
        if (_playerProperty.m_playerState == JIState.Black)
        {
            shockWaveMesh.material = m_blackShockWaveMaterial;
        }
        else
        {
            shockWaveMesh.material = m_whiteShockWaveMaterial;
        }

        // Initialize collider and material
        waveCollider.radius = 0.0001f;
        shockWaveMesh.material.SetFloat("_HoleRadius", 0);

        // Update collider and material
        float timer = 0;
        JITimer.Instance.TimeScale = m_timeScaleWhenSlow;
        while (timer < m_shockWaveTime)
        {
            timer += JITimer.Instance.RealDeltTime;

            float percent = Mathf.Clamp01(timer / m_shockWaveTime);
            shockWaveMesh.material.SetFloat("_HoleRadius", m_shockWaveSpeedCurve.Evaluate(percent));
            waveCollider.radius = Mathf.Clamp((percent + 0.2f) * 0.5f, 0, 0.5f);

            if (timer > m_timeSlowTime)
            {
                JITimer.Instance.TimeScale = 1;
            }

            yield return null;
        }

        shockWave.SetActive(false);
        Destroy(shockWave);
    }

    private void UpdatePlayerSprite()
    {
        if (_playerProperty.m_playerState == JIState.Black)
        {
            _playerSpriteRender.sprite = m_blackSprite;
        }
        else
        {
            _playerSpriteRender.sprite = m_whiteSprite;
        }
    }




    /// <summary>
    ///  Init wave destroy area
    /// </summary>
    /// <returns> Return true means can trigger destroy. Return false means cann't </returns>
    private bool InitWaveDestroy(JIDestroyArea destroyArea)
    {
        destroyArea.m_destroyBulletType = JIState.None;

        var cols = Physics2D.CircleCastAll(transform.position, _playerProperty.m_checkBound, Vector2.zero);
        foreach (var col in cols)
        {
            if (CheckBulletType(col.transform.parent.name))
            {
                destroyArea.m_destroyBulletType = _playerProperty.m_playerState;
                break;
            }
        }

        if (destroyArea.m_destroyBulletType != JIState.None)
        {
            for (int i = 0; i < cols.Length; i++)             // Destroy all bullets that detected
            {
                if (cols[i].transform.tag.ToLower().Contains("bullet"))
                {
                    UbhObjectPool.Instance.ReleaseGameObject(cols[i].transform.parent.gameObject);
                }
            }

            return true;
        }

        return false;
    }

    // Check whether a bullet's state is equals to player state
    private bool CheckBulletType(string bulletName)
    {
        bulletName = bulletName.ToLower();

        if ((_playerProperty.m_playerState & JIState.Black) == JIState.Black)
        {
            if (bulletName.Contains("black")) return true;
        }

        if ((_playerProperty.m_playerState & JIState.White) == JIState.White)
        {
            if (bulletName.Contains("white")) return true;
        }

        return false;
    }
}
