using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperty : MonoBehaviour
{
    public int m_health;

    public bool m_elite;

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

    private void OnDestroy ()
    {
    }

}