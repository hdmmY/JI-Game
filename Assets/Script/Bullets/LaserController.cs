using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LaserController : MonoBehaviour {

    public static LaserController INSTANCE
    {
        get
        {
            if (_instance == null) _instance = new LaserController();
            return _instance;                           
        }
    }

    private static LaserController _instance;

    // Time before laser collider appear
    public float m_laserDelay;

    // Time that laser collider last time
    public float m_laserLastTime;

    // Alpha before laset collider appear
    public float m_laserStartAlpha;

    [SerializeField]
    private float _timer;

    private float _alphaCounter;
    private float _alphaSpeed;

    private SpriteRenderer _sprite;
    private Color _spriteColor;

    private EdgeCollider2D _collider;

    private void OnEnable()
    {
        if(m_laserDelay < 0)
        {
            m_laserDelay = 0.1f;
        }

        if(m_laserLastTime < 0.1f)
        {
            m_laserLastTime = 0.1f;
        }

        m_laserStartAlpha = Mathf.Clamp(m_laserStartAlpha, 0.01f, 1);

        _timer = 0f;       

        _alphaCounter = 0f;
        _alphaSpeed = (1 - m_laserStartAlpha) / m_laserDelay;

        if(GetComponent<SpriteRenderer>() == null)
        {
            Debug.LogError("The sprite component is not set!");
            return;
        }
        _sprite = GetComponent<SpriteRenderer>();
        _spriteColor = _sprite.color;
        _spriteColor.a = m_laserStartAlpha;
        _sprite.color = _spriteColor;

        if(GetComponent<EdgeCollider2D>() == null)
        {
            Debug.LogError("The edgecollider is not set!");
            return;
        }
        _collider = GetComponent<EdgeCollider2D>();
        _collider.enabled = false;
    }


    private void Update()
    {
        _timer += UbhTimer.Instance.DeltaTime;

        if(_timer < m_laserDelay)
        {
            _spriteColor.a += UbhTimer.Instance.DeltaTime * _alphaSpeed;
            _sprite.color = _spriteColor;
        }
        else if(_timer < (m_laserDelay + m_laserLastTime))
        {
            if(!_collider.enabled)
                _collider.enabled = true;    
        }
        else
        {
            transform.parent.gameObject.SetActive(false);
        }                              
    }

}
