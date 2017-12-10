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
    public ShockWaveEffect m_shockWaveEffectCamera;
    public float m_shockWaveRadius;



    //public GameObject m_ChangeStateExplosion;
    //public float m_Velocity;
    //public float m_Time;



    private PlayerProperty _playerProperty;
    private SpriteRenderer _playerSpriteRender;
    private MeshRenderer _forceFieldRender;

    private void Start()
    {
        _playerProperty = GetComponent<PlayerProperty>();
        _playerSpriteRender = _playerProperty.m_spriteReference;
        _forceFieldRender = m_forceField.GetComponent<MeshRenderer>();

        RenderForceField();
    }

    private void Update()
    {
        if(Input.GetButton("Change State"))
        {
            _playerProperty.m_playerMoveState = PlayerProperty.PlayerMoveType.SlowSpeed;
        }
        else
        {
            _playerProperty.m_playerMoveState = PlayerProperty.PlayerMoveType.HighSpeed;
        }


        if (Input.GetButtonDown("Change State"))
        {

            if (_playerProperty.m_playerState == PlayerProperty.PlayerStateType.Black)
            {
                StartCoroutine(ChangeStateToWhite());
            }
            else
            {
                StartCoroutine(ChangeStateToBlack());
            }
        }
    }



    private void RenderForceField()
    {
        m_forceField.SetActive(true);

        if(_playerProperty.m_playerState == PlayerProperty.PlayerStateType.Black)
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
        if (_playerProperty.m_playerState == PlayerProperty.PlayerStateType.Black)
        {
            _playerSpriteRender.sprite = m_blackSprite;
        }
        else
        {
            _playerSpriteRender.sprite = m_whiteSprite;
        }
    }




    private IEnumerator ChangeStateToBlack()
    {
        _playerProperty.m_playerState = PlayerProperty.PlayerStateType.Black;

        m_shockWaveEffectCamera.StartShockWave(_playerProperty.transform.position, m_shockWaveRadius);   

        RenderForceField();
        RenderPlayerSprite();

        float timer = 0;
        while(timer < m_forceFieldTime)
        {
            timer += JITimer.Instance.RealDeltTime;
            yield return null;
        }

        m_forceField.SetActive(false);
    }



    private IEnumerator ChangeStateToWhite()
    {
        _playerProperty.m_playerState = PlayerProperty.PlayerStateType.White;

        m_shockWaveEffectCamera.StartShockWave(_playerProperty.transform.position, m_shockWaveRadius);

        RenderForceField();
        RenderPlayerSprite();

        float timer = 0;
        while (timer < m_forceFieldTime)
        {
            timer += JITimer.Instance.RealDeltTime;
            yield return null;
        }

        m_forceField.SetActive(false);
    }











    //IEnumerator TriggerEnter()
    //{
    //    JITimer.Instance.TimeScale = 0f;
    //    _property.m_tgm = true;

    //    yield return new WaitForSeconds(0.5f);

    //    JITimer.Instance.TimeScale = 1f;
    //    _property.m_tgm = false;

    //    Transform ChangeStateTras = UbhObjectPool.Instance.GetGameObject
    //        (m_ChangeStateExplosion, transform.position, Quaternion.identity).transform;

    //    SpriteRenderer spriteRender = ChangeStateTras.GetComponent<SpriteRenderer>();
    //    spriteRender.enabled = true;

    //    CircleCollider2D collider = ChangeStateTras.GetComponent<CircleCollider2D>();
    //    collider.enabled = true;

    //    float timer = 0f;

    //    while (true)
    //    {
    //        timer += Time.deltaTime;

    //        ChangeStateTras.localScale += Vector3.one * Time.deltaTime * m_Velocity;

    //        if (timer < m_Time)
    //        {
    //            yield return null;
    //        }
    //        else
    //        {
    //            JITimer.Instance.TimeScale = 1f;
    //            ChangeStateTras.localScale = Vector3.one;
    //            spriteRender.enabled = false;
    //            collider.enabled = false;
    //            UbhObjectPool.Instance.ReleaseGameObject(ChangeStateTras.gameObject);
    //            yield break;
    //        }
    //    }
    //}


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_shockWaveRadius);
    }

}
