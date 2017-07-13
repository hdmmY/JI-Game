using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    public InputManager m_InputManager;

    public BulletPool m_BulletPool;

    public Transform m_leftShootPos;
    public Transform m_rightShootPos;

    [Range(0.05f, 1)]
    public float m_ShootInterval;

    private float _freTimer;    // timer for count frequency


    private void OnEnable()
    {
        _freTimer = 0f;
    }

    private void Update()
    {
        if(m_InputManager.m_Shoot)
        {
            if(_freTimer >= m_ShootInterval)
            {
                Shoot();
            }
        } 

        _freTimer += Time.deltaTime;        
    }


    private void Shoot()
    {
        _freTimer = 0f;

        GameObject leftBullet = m_BulletPool.create(m_leftShootPos.position);
        GameObject rightBullet = m_BulletPool.create(m_rightShootPos.position);

        leftBullet.GetComponent<PlayerBullet>().m_BulletPool = m_BulletPool;
        rightBullet.GetComponent<PlayerBullet>().m_BulletPool = m_BulletPool;

        return;
    }

}
