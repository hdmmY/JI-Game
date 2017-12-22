using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperty: MonoBehaviour
{
    public JIState m_enemyState;

    [HideInInspector]
    public string m_enemyBulletTag = "Untagged";

    [HideInInspector]
    public string m_playerBulletTag = "Untagged";

    public int m_health;

    public int m_enemyDamage;

    [HideInInspector]
    public SpriteRenderer m_enemySprite;

}
