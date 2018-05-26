using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

/// <summary>
/// Raised when bullet attack a unit
/// </summary>
public class BulletAttackEvent : Event
{
    public JIBulletProperty Bullet;
    public JIColliderUnit ColliderUnit;

    // public JIBulletProperty Bullet;

    public Transform ColUnit;

    public BulletAttackEvent (JIBulletProperty bullet, JIColliderUnit colUnit)
    {
        Bullet = bullet;
        ColliderUnit = colUnit;
    }

    public BulletAttackEvent (JIBulletProperty bullet, Transform colUnit)
    {
        Bullet = bullet;
        ColUnit = colUnit;
    }
}

public enum JIColliderUnitType : byte
{
    Player,
    Enemy,
    PlayerBullet,
    EnemyBullet
}

public struct JIColliderUnit
{
    public JIColliderUnitType UnitType;

    public JIBulletProperty EnemyBullet;

    public JIBulletProperty PlayerBullet;

    public EnemyProperty Enemy;

    public PlayerProperty Player;

    public JIColliderUnit (EnemyProperty enemy,
        JIColliderUnitType unitType = JIColliderUnitType.Enemy)
    {
        UnitType = unitType;
        EnemyBullet = null;
        PlayerBullet = null;
        Player = null;
        Enemy = enemy;
    }

    public JIColliderUnit (PlayerProperty player,
        JIColliderUnitType unitType = JIColliderUnitType.Player)
    {
        UnitType = unitType;
        EnemyBullet = null;
        PlayerBullet = null;
        Enemy = null;
        Player = player;
    }

    public JIColliderUnit (JIBulletProperty bullet, JIColliderUnitType unitType)
    {
        UnitType = unitType;
        Player = null;
        Enemy = null;
        if (unitType == JIColliderUnitType.EnemyBullet)
        {
            PlayerBullet = null;
            EnemyBullet = bullet;
        }
        else
        {
            EnemyBullet = null;
            PlayerBullet = bullet;
        }
    }
}