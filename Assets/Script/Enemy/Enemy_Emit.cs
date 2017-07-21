using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Enemy_Emit_Property))]
public class Enemy_Emit : MonoBehaviour
{
    public bool m_emit;           // if true, start emit
    public bool m_updateEmitProperty;     // if the emit_bullet_property has changed, then please set it to true

    private Enemy_Emit_Property _emitProperty;
    private Enemy_Emit_Reference _enemyEmitReference;
    private Enemy_Property _enemyProperty;

    private BulletPool _bulletPool;
    private Bullet_Property _bulletTempleProperty;

    private float[] _emitDirs;
    private Vector3[] _emitPoints;

    private float _timer;

    private void OnEnable()
    {
        SetInitReference();

        EmitInit(_emitProperty);
    }

    private void Update()
    {
        if (m_updateEmitProperty)
        {
            EmitInit(_emitProperty);
            //m_updateEmitProperty = false;
        }


        if (m_emit)
        {
            if (_timer >= _emitProperty.m_EmitInterval)
            {
                Emitting();
            }
            _timer += Time.deltaTime;
        }
    }


    void Emitting()
    {
        _timer = 0f;

        GameObject bullet;

        Vector3 bulletOrigin;
        float bulletAngleOffset;

        if (_bulletPool == null)
            return;

        for (int i = 0; i < _emitPoints.Length; i++)
        {
            bulletAngleOffset = _emitDirs[i];
            bulletOrigin = _emitPoints[i];

            bullet = _bulletPool.create(bulletOrigin);                                    // create bullet
            bullet.transform.rotation = Quaternion.Euler(0, 0, bulletAngleOffset);        // set bullet rotation

            bullet.GetComponent<Bullet_Property>().CopyProperty(_bulletTempleProperty);   // set bullet property

            bullet.GetComponent<Bullet_Controller>().m_InitAngle = bulletAngleOffset;     // init bullet controller

            bullet.layer = LayerMask.NameToLayer(_enemyProperty.m_EnemyBulletLayer);      // set bullet layer

            bullet.GetComponent<BulletEventMaster>().CallBulletPropertyInitEvent(_bulletTempleProperty);

            bullet.SetActive(true);
        }
    }



    /// <summary>
    /// init the _emitDirs and _emitPoints
    /// </summary>
    /// <param name="emitProperty"></param>
    void EmitInit(Enemy_Emit_Property emitProperty)
    {
        _emitDirs = emitProperty.GetEmitDirs();
        _emitPoints = emitProperty.GetEmitPoints();
    }



    void SetInitReference()
    {
        _emitProperty = GetComponent<Enemy_Emit_Property>();
        _enemyEmitReference = GetComponent<Enemy_Emit_Reference>();
        _enemyProperty = _enemyEmitReference.m_enemyProperty;

        _bulletPool = _enemyEmitReference.m_BulletPool;
        _bulletTempleProperty = _enemyEmitReference.m_TempleBulletProperty;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        if (_emitPoints == null) return;

        foreach (Vector3 point in _emitPoints)
        {
            Gizmos.DrawCube(point, Vector3.one * 0.15f);
        }
    }

}
