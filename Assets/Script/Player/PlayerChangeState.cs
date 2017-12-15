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
                StartCoroutine(ChangeStateToWhite());
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(ChangeStateToBlack());
            }
        }
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



    private IEnumerator ChangeStateToBlack()
    {
        _playerProperty.m_playerState = JIState.Black;

        m_shockWaveMesh.gameObject.SetActive(true);
        m_shockWaveMesh.material.SetFloat("_HoleRadius", 0);

        RenderForceField();
        RenderShockWave();
        RenderPlayerSprite();

        float timer = 0;
        while (timer < m_forceFieldTime)
        {
            timer += JITimer.Instance.RealDeltTime;
            m_shockWaveMesh.material.SetFloat("_HoleRadius", timer / m_shockWaveTime); 
            yield return null;
        }

        m_forceField.SetActive(false);
        m_shockWaveMesh.gameObject.SetActive(false);
    }



    private IEnumerator ChangeStateToWhite()
    {
        _playerProperty.m_playerState = JIState.White;

        m_shockWaveMesh.gameObject.SetActive(true);
        m_shockWaveMesh.material.SetFloat("_HoleRadius", 0);

        RenderForceField();
        RenderShockWave();
        RenderPlayerSprite();

        float timer = 0;
        while (timer < m_forceFieldTime)
        {
            timer += JITimer.Instance.RealDeltTime;
            m_shockWaveMesh.material.SetFloat("_HoleRadius", timer / m_shockWaveTime);
            yield return null;
        }

        m_forceField.SetActive(false);
        m_shockWaveMesh.gameObject.SetActive(false);
    }


}
