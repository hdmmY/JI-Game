using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEventMaster : BaseEventMaster
{
    /// <summary>
    /// Called when enemy is taking damage
    /// </summary>
    public System.Action<EnemyProperty> OnDamage;


    public void CallOnDamage(EnemyProperty enemyProperty)
    {
        if (enemyProperty != null)
        {
            OnDamage(enemyProperty);
        }
    }
     
}
