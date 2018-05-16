using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttackSystem : Singleton<BulletAttackSystem>
{
    private void OnEnable ()
    {
        EventManager.Instance.AddListener<BulletAttackEvent> (DealwithBulletAttack);
    }

    private void OnDisable ()
    {
        EventManager.Instance.RemoveListener<BulletAttackEvent> (DealwithBulletAttack);
    }

    private void DealwithBulletAttack (BulletAttackEvent attEvent)
    {
        bool isEnemy = attEvent.ColUnit.CompareTag ("Enemy");
        bool isPlayer = attEvent.ColUnit.CompareTag ("Player");

        if (attEvent.Bullet.IsPlayerBullet && isPlayer)
        {
            return;
        }
        else if (!attEvent.Bullet.IsPlayerBullet && isEnemy)
        {
            return;
        }

        if (attEvent.Bullet.IsLaser)
        {
            Laser laser = attEvent.Bullet.GetComponent<Laser> ();

            if (isPlayer)
            {
                DealwithEnemyLaserAttack ();
            }
            else if (isEnemy)
            {
                DealwithPlayerLaserAttack (laser,
                    attEvent.ColUnit.GetComponent<EnemyProperty> ());
            }
        }
        else
        {
            JIBulletProperty bullet = attEvent.Bullet.GetComponent<JIBulletProperty> ();

            if (isPlayer)
            {
                DealwithEnemyNormalAttack ();
            }
            else
            {
                DealwithPlayerNormalAttack (bullet,
                    attEvent.ColUnit.GetComponent<EnemyProperty> ());
            }
        }
    }

    // Player laser attack enemy
    private void DealwithPlayerLaserAttack (Laser laser, EnemyProperty enemy)
    {
        if (laser.DamageTimer < laser.Interval) return;
        if (enemy.m_isDead) return;

        laser.DamageTimer = 0f;

        enemy.m_health -= laser.Damage;
        enemy.CallOnDamage (enemy);

        if (enemy.m_health <= 0)
        {
            enemy.m_isDead = true;
            Destroy (enemy.gameObject);
        }
    }

    // Enemy laser attack player
    private void DealwithEnemyLaserAttack ()
    {

    }

    // Player bullet attack enemy
    private void DealwithPlayerNormalAttack (JIBulletProperty bullet, EnemyProperty enemy)
    {
        enemy.m_health -= bullet.Damage;
        enemy.CallOnDamage (enemy);

        if (enemy.m_health <= 0)
        {
            enemy.m_isDead = true;
            Destroy (enemy.gameObject);
        }

        BulletPool.Instance.ReleaseGameObject (bullet.gameObject);
    }

    // Enemy bullet attack player
    private void DealwithEnemyNormalAttack ()
    {

    }
}