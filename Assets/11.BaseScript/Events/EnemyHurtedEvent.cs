using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Raised when enemy is hurted by player bullet
/// </summary>
public class EnemyHurtedEvent : Event
{
    public JIState PlayerBulletState;

    public EnemyHurtedEvent (JIState playerBulletState)
    {
        PlayerBulletState = playerBulletState;
    }
}