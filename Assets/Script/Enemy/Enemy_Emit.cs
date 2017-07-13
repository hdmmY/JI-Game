using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Enemy_Emit_Property))]
public class Enemy_Emit : MonoBehaviour
{
    public bool m_emit;           // if true, start emit
    public bool m_updateEmitProperty;     // if the emit_bullet_property has changed, then please set it to true

    private Enemy_Emit_Property _emitProperty;

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
        if (m_emit)
        {
            if (_timer >= _emitProperty.m_EmitInterval)
            {
                Emitting();
            }
            _timer += Time.deltaTime;
        }

        if (m_updateEmitProperty)
        {
            EmitInit(_emitProperty);
            //m_updateEmitProperty = false;
        }
    }


    void Emitting()
    {
        _timer = 0f;

        GameObject bullet;
        BulletPool bulletPool = _emitProperty.m_bulletPool;
        Vector3 bulletOrigin;
        float bulletAngleOffset;

        if (bulletPool == null)
            return;

        for (int i = 0; i < _emitPoints.Length; i++)
        {
            bulletAngleOffset = _emitDirs[i];
            bulletOrigin = _emitPoints[i];

            bullet = bulletPool.create(bulletOrigin);

            bullet.transform.rotation = Quaternion.Euler(0, 0, bulletAngleOffset);

            bullet.GetComponent<Bullet_Controller>().m_InitAngle = bulletAngleOffset;
        }    
    }




    void EmitInit(Enemy_Emit_Property emitProperty)
    {
        _emitDirs = emitProperty.GetEmitDirs();
        _emitPoints = emitProperty.GetEmitPoints();
    }



    void SetInitReference()
    {
        _emitProperty = GetComponent<Enemy_Emit_Property>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        if (_emitPoints == null) return;

        foreach(Vector3 point in _emitPoints)
        {
            Gizmos.DrawCube(point, Vector3.one * 0.15f);
        }
    }

}
