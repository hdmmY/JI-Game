using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperty : MonoBehaviour
{
    public static readonly string EnemyBulletTag = "EnemyBullet";

    public static readonly string PlayerBulletTag = "PlayerBullet";

    public int m_health;

    public SpriteRenderer m_enemySprite;

    // Whether the enemy is dead
    [HideInInspector] public bool m_isDead;

    #region Events
    /// <summary>
    /// Called when enemy is taking damage
    /// </summary>
    public System.Action<EnemyProperty> OnDamage;

    /// <summary>
    /// Called when enemy is taking damage
    /// </summary>
    /// <param name="enemyProperty"></param>
    public void CallOnDamage (EnemyProperty enemyProperty)
    {
        if (enemyProperty != null && OnDamage != null)
        {
            OnDamage (enemyProperty);
        }
    }
    #endregion
}