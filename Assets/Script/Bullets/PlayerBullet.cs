using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

    public BulletPool m_BulletPool;

    public float m_speed;     

    private Rigidbody2D _rb2D;

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _rb2D.velocity = new Vector2(0, m_speed);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if(collision.CompareTag("Edge"))
        {
            m_BulletPool.delete(this.gameObject);
        }
    }

}
