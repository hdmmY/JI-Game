using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeState : MonoBehaviour
{

    public KeyCode m_ChangeStateKey;

    public Sprite m_whiteSprite;
    public Sprite m_blackSprite;

    public float m_Radius;

    public GameObject m_ChangeStateExplosion;
    public float m_Velocity;
    public float m_Time;

    private PlayerProperty _property;
    private SpriteRenderer _spriteRender;

    private void Start()
    {
        _property = GetComponent<PlayerProperty>();
        _spriteRender = GetComponent<SpriteRenderer>();


        switch (_property.m_playerState)
        {
            case PlayerProperty.PlayerStateType.White:
                _spriteRender.sprite = m_whiteSprite;
                break;

            case PlayerProperty.PlayerStateType.Black:
                _spriteRender.sprite = m_blackSprite;
                break;
        }
    }

    private void Update()
    {
        if(Input.GetKey(m_ChangeStateKey))
        {
            _property.m_playerMoveState = PlayerProperty.PlayerMoveType.SlowSpeed;
        }
        else
        {
            _property.m_playerMoveState = PlayerProperty.PlayerMoveType.HighSpeed;
        }

        if (Input.GetKeyDown(m_ChangeStateKey))
        {
            switch (_property.m_playerState)
            {
                case PlayerProperty.PlayerStateType.White:
                    _property.m_playerState = PlayerProperty.PlayerStateType.Black;
                    _spriteRender.sprite = m_blackSprite;
                    break;

                case PlayerProperty.PlayerStateType.Black:
                    _property.m_playerState = PlayerProperty.PlayerStateType.White;
                    _spriteRender.sprite = m_whiteSprite;
                    break;
            }

            RaycastHit2D[] rayhits = Physics2D.CircleCastAll(transform.position, m_Radius, Vector2.zero);

            foreach (RaycastHit2D rayhit in rayhits)
            {
                string name = (rayhit.collider.gameObject.name).ToLower();

                switch (_property.m_playerState)
                {
                    case PlayerProperty.PlayerStateType.Black:
                        if (name.Contains("black"))
                        {
                            StartCoroutine(TriggerEnter());
                        }
                        break;


                    case PlayerProperty.PlayerStateType.White:
                        if (name.Contains("red"))
                        {
                            StartCoroutine(TriggerEnter());
                        }
                        break;
                }
            }

        }


    }


    IEnumerator TriggerEnter()
    {
        UbhTimer.Instance.TimeScale = 0f;
        _property.m_tgm = true;

        yield return new WaitForSeconds(0.5f);

        UbhTimer.Instance.TimeScale = 1f;
        _property.m_tgm = false;

        Transform ChangeStateTras = UbhObjectPool.Instance.GetGameObject
            (m_ChangeStateExplosion, transform.position, Quaternion.identity).transform;

        SpriteRenderer spriteRender = ChangeStateTras.GetComponent<SpriteRenderer>();
        spriteRender.enabled = true;

        CircleCollider2D collider = ChangeStateTras.GetComponent<CircleCollider2D>();
        collider.enabled = true;

        float timer = 0f;

        while (true)
        {
            timer += Time.deltaTime;

            ChangeStateTras.localScale += Vector3.one * Time.deltaTime * m_Velocity;

            if (timer < m_Time)
            {
                yield return null;
            }
            else
            {
                UbhTimer.Instance.TimeScale = 1f;
                ChangeStateTras.localScale = Vector3.one;
                spriteRender.enabled = false;
                collider.enabled = false;
                UbhObjectPool.Instance.ReleaseGameObject(ChangeStateTras.gameObject);
                yield break;
            }
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, m_Radius);
    }


}
