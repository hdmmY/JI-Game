using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TakeDamage : MonoBehaviour {

    private Enemy_Property _property;

    private void OnEnable()
    {
        _property = GetComponent<Enemy_Property>();

        if (_property == null) Debug.LogError("The Enemy Property is null!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(_property.m_playerBulletTag))
        {
            Debug.Log(collision.name);
            var bullet = collision.transform.parent.GetComponent<UbhBullet>();

            UbhObjectPool.Instance.ReleaseGameObject(bullet.gameObject);

            _property.m_health -= bullet.m_damage;
            if(_property.m_health <= 0)
            {
                EnemyDeath();
            }
        }


    }

    private void EnemyDeath()
    {
        Destroy(_property.gameObject);
    }
}
