using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttract : MonoBehaviour
{                          
    private Transform _targetTrans;
    private float _factor;

    private bool _canAttract;

    private Bullet_Property _bulletProperty;
    private BulletEventMaster _bulletEventMaster;

    private void OnEnable()
    {
        _bulletProperty = GetComponent<Bullet_Property>();
        _bulletEventMaster = GetComponent<BulletEventMaster>();

        _bulletEventMaster.BulletPropertyInitEvent += InitFactorAndTargettrans;
    }

    private void OnDisable()
    {
        _bulletEventMaster.BulletPropertyInitEvent -= InitFactorAndTargettrans;
    }


    private void Update()
    {
        if (!_bulletProperty.m_useBulletAttrack)
            return;

        float distance = (_targetTrans.position - transform.position).sqrMagnitude;
        if (distance <= 0.1f)
        {
            _canAttract = false;
        }
        else
        {
            _canAttract = true;
        }


        if (_canAttract)
        {
            Attract();
        }
    }


    private void Attract()
    {
        Vector3 dir = _targetTrans.position - transform.position;
        float time = dir.y / _bulletProperty.m_Velocity.y;
        //_rb2D.velocity = new Vector2(dir.x / time, _rb2D.velocity.y);

        _bulletProperty.m_Velocity = new Vector2(dir.x / time, _bulletProperty.m_Velocity.y);
    }


    void InitFactorAndTargettrans(Bullet_Property initProperty)
    {
        _targetTrans = initProperty.m_targetTrans;
        _factor = initProperty.m_rejectFactor;
    }
}
