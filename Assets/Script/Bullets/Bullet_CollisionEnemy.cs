using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BulletReference))]
[RequireComponent(typeof(BulletEventMaster))]
[RequireComponent(typeof(Bullet_Property))]
public class Bullet_CollisionEnemy: MonoBehaviour
{
    private BulletEventMaster _bulletEventMaster;
    private BulletPool _bulletPool;


    private void OnEnable()
    {
    	SetInitReference();

        // event
        _bulletEventMaster.TriggerEnemyEvent += TakeDamageAndDisable;
    }

    private void OnDisable()
    {
        _bulletEventMaster.TriggerEnemyEvent -= TakeDamageAndDisable;
    }

    private void SetInitReference()
    {
        _bulletEventMaster = GetComponent<BulletEventMaster>();
        _bulletPool = GetComponent<BulletReference>().m_BulletPool;

    }


    private void TakeDamageAndDisable(Bullet_Property bulletProperty, Enemy_Property enemyProperty)
    {
        enemyProperty.m_enmeyHealth -= bulletProperty.m_BulletDamage;

        if(enemyProperty.m_enmeyHealth <= 0)
        {
            enemyProperty.GetComponent<EnemyEventMaster>().CallEnemyDeathEvent();
        }

        _bulletPool.delete(bulletProperty.gameObject);
    }

    
}
