using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BulletReference))]
[RequireComponent(typeof(BulletEventMaster))]
public class Bullet_CollisionPlayer : MonoBehaviour
{
    private BulletEventMaster _bulletEventMaster;
    private BulletPool _bulletPool;

    private void OnEnable()
    {
        SetInitReference();

        // event 
        _bulletEventMaster.TriggerPlayerEvent += DisableBullet;
    }

    private void OnDisable()
    {
        // event
        _bulletEventMaster.TriggerPlayerEvent -= DisableBullet;
    }

    private void DisableBullet(Bullet_Property bulletProperty)
    {
        _bulletPool.delete(this.gameObject);
    }



    private void SetInitReference()
    {
        _bulletEventMaster = GetComponent<BulletEventMaster>();
        _bulletPool = GetComponent<BulletReference>().m_BulletPool;
    }

}
