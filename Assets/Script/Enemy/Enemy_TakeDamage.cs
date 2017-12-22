using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TakeDamage : MonoBehaviour
{

    private Enemy_Property _property;

    private bool _isDead;


    private void OnEnable()
    {
        _isDead = false;

        _property = GetComponent<Enemy_Property>();

        if (_property == null) Debug.LogError("The Enemy Property is null!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_property.m_playerBulletTag))
        {
            var bullet = collision.transform.parent.GetComponent<JIBulletProperty>();

            UbhObjectPool.Instance.ReleaseGameObject(bullet.gameObject);

            _property.m_health -= bullet.m_damage;
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
