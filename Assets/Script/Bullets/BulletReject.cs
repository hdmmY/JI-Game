using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletReject : MonoBehaviour {

    private Transform _TargetTrans;
    private float _factor;

    private float _sqrDistance;
    private float _initialXVelocity;
    private float _deltXVelocity;

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
        if (!_bulletProperty.m_useBulletReject)
            return;

        Vector3 deltPosition = transform.position - _TargetTrans.position;

        _initialXVelocity = _bulletProperty.m_Velocity.x - _deltXVelocity;

        _sqrDistance = deltPosition.sqrMagnitude;

        _deltXVelocity = _factor / (_sqrDistance + 1f);
        _deltXVelocity = deltPosition.x > 0 ? _deltXVelocity : -_deltXVelocity;

        _bulletProperty.m_Velocity = new Vector2(_initialXVelocity + _deltXVelocity, _bulletProperty.m_Velocity.y);
    }


    void InitFactorAndTargettrans(Bullet_Property initProperty)
    {
        _TargetTrans = initProperty.m_targetTrans;
        _factor = initProperty.m_rejectFactor;
    }

}
