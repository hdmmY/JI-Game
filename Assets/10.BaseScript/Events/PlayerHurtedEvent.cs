using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Raised when player being hurted
/// </summary>
public class PlayerHurtedEvent : Event
{
    public int CurPlayerLife;

    public int CurPlayerHealth;

    public PlayerHurtedEvent (int curPlayerLife, int curPlayerHealth)
    {
        CurPlayerLife = curPlayerLife;
        CurPlayerHealth = curPlayerHealth;
    }
}