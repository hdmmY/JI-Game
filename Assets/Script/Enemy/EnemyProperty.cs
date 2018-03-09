using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperty : MonoBehaviour
{
    public static readonly string EnemyBulletTag = "EnemyBullet";

    public static readonly string PlayerBulletTag = "PlayerBullet";

    public JIState m_enemyState;

    public int m_health;

    public int m_enemyDamage;

    public SpriteRenderer m_enemySprite;
}