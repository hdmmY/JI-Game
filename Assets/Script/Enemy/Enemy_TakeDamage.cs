using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EnemyProperty))]
public class Enemy_TakeDamage : MonoBehaviour
{                                 
    private EnemyProperty _property;
    private EnemyEventMaster _eventMaster;

    private bool _isDead;

    private int _enemyHealth;

    private void OnEnable()
    {
        _isDead = false;

        _eventMaster = GetComponent<EnemyEventMaster>();
        _property = GetComponent<EnemyProperty>();   
        if (_property == null) Debug.LogError("The Enemy Property is null!");

        _enemyHealth = _property.m_health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_property.m_playerBulletTag))
        {
            var bullet = collision.transform.parent.GetComponent<JIBulletProperty>();

            UbhObjectPool.Instance.ReleaseGameObject(bullet.gameObject);

            _property.m_health -= bullet.m_damage;
            if (_eventMaster != null)
            {
                _eventMaster.CallOnDamage(_property);
            }

            if (_property.m_health <= 0)
            {
                EnemyDeath(bullet.State);
            }
        }
    }

    private void EnemyDeath(JIState bulletState)
    {
        if (bulletState != _property.m_enemyState && !_isDead)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerProperty>().AddNeutralization(GlobalStaticVariable.AddedNeutralization);
        }
        _isDead = true;
        Destroy(_property.gameObject);
    }
}
