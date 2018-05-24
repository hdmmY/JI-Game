using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Raised when bullet attack a unit
/// </summary>
public class BulletAttackEvent : Event
{
    public JIBulletProperty Bullet;

    public Transform ColUnit;

    public BulletAttackEvent (JIBulletProperty bullet, Transform colUnit)
    {
        Bullet = bullet;
        ColUnit = colUnit;
    }
}