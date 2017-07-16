using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BulletEventMaster))]
public class BulletDestoryOutBound : MonoBehaviour {

    private BulletEventMaster _bulletEventMaster;
    private BulletPool _bulletPool;

        
    private void OnEnable()
    {
        _bulletPool = GetComponent<BulletReference>().m_BulletPool;
        _bulletEventMaster = GetComponent<BulletEventMaster>();

        _bulletEventMaster.TriggerEdgeEvent += DestroyBullet;
    }


    private void OnDisable()
    {
        _bulletEventMaster.TriggerEdgeEvent -= DestroyBullet;
    }


    private void DestroyBullet()
    {
        _bulletPool.delete(this.gameObject);
    }
}
