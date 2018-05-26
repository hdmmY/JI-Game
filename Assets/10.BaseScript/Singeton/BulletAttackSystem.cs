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

    #region Main Method

    private void DealwithBulletAttack (BulletAttackEvent attEvent)
    {
        if (attEvent.Bullet.IsPlayerBullet)
        {
            switch (attEvent.ColliderUnit.UnitType)
            {
                case JIColliderUnitType.Player:
                    PlayerBullet_Player (attEvent.Bullet, attEvent.ColliderUnit.Player);
                    break;
                case JIColliderUnitType.PlayerBullet:
                    PlayerBullet_PlayerBullet (attEvent.Bullet, attEvent.ColliderUnit.PlayerBullet);
                    break;
                case JIColliderUnitType.Enemy:
                    PlayerBullet_Enemy (attEvent.Bullet, attEvent.ColliderUnit.Enemy);
                    break;
                case JIColliderUnitType.EnemyBullet:
                    PlayerBullet_EnemyBullet (attEvent.Bullet, attEvent.ColliderUnit.EnemyBullet);
                    break;
            }
        }
        else
        {
            switch (attEvent.ColliderUnit.UnitType)
            {
                case JIColliderUnitType.Player:
                    EnemyBullet_Player (attEvent.Bullet, attEvent.ColliderUnit.Player);
                    break;
                case JIColliderUnitType.PlayerBullet:
                    EnemyBullet_PlayerBullet (attEvent.Bullet, attEvent.ColliderUnit.PlayerBullet);
                    break;
                case JIColliderUnitType.Enemy:
                    EnemyBullet_Enemy (attEvent.Bullet, attEvent.ColliderUnit.Enemy);
                    break;
                case JIColliderUnitType.EnemyBullet:
                    EnemyBullet_EnemyBullet (attEvent.Bullet, attEvent.ColliderUnit.EnemyBullet);
                    break;
            }
        }
    }

    private void PlayerBullet_PlayerBullet (JIBulletProperty playerBulletA, JIBulletProperty playerBulletB)
    {

    }

    private void PlayerBullet_EnemyBullet (JIBulletProperty playerBullet, JIBulletProperty enemyBullet)
    {

    }

    private void PlayerBullet_Player (JIBulletProperty playerBullet, PlayerProperty player)
    {

    }

    private void PlayerBullet_Enemy (JIBulletProperty playerBullet, EnemyProperty enemy)
    {
        if (playerBullet.Type == JIBulletType.Laser)
        {
            var laser = playerBullet.GetComponent<Laser> ();

            if (laser.DamageTimer < laser.Interval) return;
            laser.DamageTimer = 0f;

            AttackEnemy (laser.Damage, enemy);
        }
        else if (playerBullet.Type == JIBulletType.Normal)
        {
            AttackEnemy (playerBullet.Damage, enemy);
            BulletPool.Instance.ReleaseGameObject (playerBullet.gameObject);
        }
        else if (playerBullet.Type == JIBulletType.Bomb)
        {
            if (!enemy.m_elite)
            {
                AttackEnemy (enemy.m_health, enemy);
            }
        }
    }

    private void EnemyBullet_Player (JIBulletProperty enemyBullet, PlayerProperty player)
    {
        if (!player.m_god)
        {
            player.GetComponentInChildren<PlayerTakeDamage> ().PlayerDeath ();
        }

        if (enemyBullet.Type == JIBulletType.Normal)
        {
            BulletPool.Instance.ReleaseGameObject (enemyBullet.gameObject);
        }
        else
        {
            Destroy (enemyBullet.gameObject);
        }
    }

    private void EnemyBullet_EnemyBullet (JIBulletProperty enemyBulletA, JIBulletProperty enmeyBulletB)
    {

    }

    private void EnemyBullet_PlayerBullet (JIBulletProperty enemyBullet, JIBulletProperty playerBullet)
    {

    }

    private void EnemyBullet_Enemy (JIBulletProperty enemyBullet, EnemyProperty enemy)
    {

    }

    #endregion

    #region Helper Methods

    private void AttackEnemy (int damage, EnemyProperty enemy)
    {
        enemy.m_health -= damage;
        enemy.CallOnDamage (enemy);

        if (enemy.m_health <= 0 && !enemy.m_isDead)
        {
            enemy.m_isDead = true;
            Destroy (enemy.gameObject);
        }
    }

    #endregion
}