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
    public Transform m_lightTransform;

    [Space]

    [Header("Shock Wave Distortion")]
    public MeshRenderer m_shockWaveMesh;
    public Material m_whiteShockWaveMaterial;
    public Material m_blackShockWaveMaterial;
    [Range(0, 1)]
    public float m_shockWaveRadius;
    public float m_shockWaveTime;


    private PlayerProperty _playerProperty;
    private SpriteRenderer _playerSpriteRender;
    private MeshRenderer _forceFieldRender;

    private CircleCollider2D _waveCricleCollider;
    private JIDestroyArea _waevDestroyAreaScript;

    private void Start()
    {
        _playerProperty = GetComponent<PlayerProperty>();
        _playerSpriteRender = _playerProperty.m_spriteReference;
        _forceFieldRender = m_forceField.GetComponent<MeshRenderer>();

        _waveCricleCollider = m_shockWaveMesh.GetComponent<CircleCollider2D>();
        _waevDestroyAreaScript = m_shockWaveMesh.GetComponent<JIDestroyArea>();
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
                StopAllCoroutines();

                _playerProperty.m_playerState = JIState.White;
                StartCoroutine(ChangeState());
            }
            else
            {
                StopAllCoroutines();

                _playerProperty.m_playerState = JIState.Black;
                StartCoroutine(ChangeState());
            }
        }
    }

    private IEnumerator ChangeState()
    {
        InitWaveDestroy();

        _waveCricleCollider.radius = 0.0001f;

        m_shockWaveMesh.gameObject.SetActive(true);
        m_shockWaveMesh.material.SetFloat("_HoleRadius", 0);

        RenderForceField();
        RenderShockWave();
        RenderPlayerSprite();

        float timer = 0;
        while (timer < m_shockWaveTime)
        {
            timer += JITimer.Instance.RealDeltTime;

            m_shockWaveMesh.material.SetFloat("_HoleRadius", timer / m_shockWaveTime * 0.7f);
            _waveCricleCollider.radius = timer / m_shockWaveTime * 0.5f;

            yield return null;
        }

        m_forceField.SetActive(false);
        m_shockWaveMesh.gameObject.SetActive(false);
    }


    private void RenderForceField()
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
    }

    private void RenderPlayerSprite()
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

    private void RenderShockWave()
    {
        if (_playerProperty.m_playerState == JIState.Black)
        {
            m_shockWaveMesh.material = m_blackShockWaveMaterial;
        }
        else
        {
            m_shockWaveMesh.material = m_whiteShockWaveMaterial;
        }
    }       

    /// <summary>
    /// Init wave destroy area
    /// </summary>
    private void InitWaveDestroy()
    {
        _waevDestroyAreaScript.m_destroyBulletType = JIState.None;

        var cols = Physics2D.CircleCastAll(transform.position, _playerProperty.m_checkBound, Vector2.zero);
        foreach (var col in cols)
        {
            if (CheckBulletType(col.transform.parent.name))
            {
                _waevDestroyAreaScript.m_destroyBulletType = _playerProperty.m_playerState;
                break;
            }
        }

        if (_waevDestroyAreaScript.m_destroyBulletType != JIState.None)
        {
            for (int i = 0; i < cols.Length; i++)             // Destroy all bullets that detected
            {
                if (cols[i].transform.tag.ToLower().Contains("bullet"))
                {
                    UbhObjectPool.Instance.ReleaseGameObject(cols[i].transform.parent.gameObject);
                }
            }
        }
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
