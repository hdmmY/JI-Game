using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(PlayerProperty))]
public class PlayerTakeDamage: MonoBehaviour
{
    private PlayerProperty _playerProperty;
    private EnemyEventMaster _enemyEventMaster;

    private void OnEnable()
    {
        SetInitReference();

        PlayerProperty.PlayerShootEvent += AddHurtFunction;
    }

    private void OnDisable()
    {
        
    }


    void SetInitReference()
    {
        _playerProperty = GetComponent<PlayerProperty>();
    }


    void AddHurtFunction(GameObject bulletGo)
    {
        bulletGo.GetComponent<BulletEventMaster>().TriggerEnemyEvent += HurtEnemy;
    }

    void HurtEnemy(Bullet_Property bulletProperty, Enemy_Property enemyProperty)
    {


        enemyProperty.m_enmeyHealth -= bulletProperty.m_BulletDamage;

        if(enemyProperty.m_enmeyHealth <= 0)
        {
            enemyProperty.GetComponent<EnemyEventMaster>().CallEnemyDeathEvent();
        }   
    }


    void RemoveEvent()
    {
        
    }

}
