using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Property: MonoBehaviour
{
    public enum EnemyState
    {
        Black,
        White
    };

    public EnemyState m_enemyState;

    [HideInInspector]
    public string m_enemyBulletTag = "Untagged";

    [HideInInspector]
    public string m_playerBulletTag = "Untagged";

    public int m_health;

    public int m_enemyDamage;

}
