using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerProperty : MonoBehaviour
{
    public enum PlayerMoveType
    {
        HighSpeed,
        SlowSpeed
    }

    // god mode -- from Skyrim.
    public bool m_god = false;

    public bool m_superState = false;

    public JIState m_playerState;

    public PlayerMoveType m_playerMoveState;

    public int m_playerLife; // Each life have muti health

    public int m_playerHealth;

    [Space]

    public int m_playerBlackPoint;

    public int m_playerWhitePoint;

    public int m_maxPlayerPoint;

    /// <summary>
    /// When the bullet attack the enemy, this value will be add to the player's point
    /// </summary>
    public int m_addValue = 1;

    [Space]

    public float m_checkBound;

    [BoxGroup ("Movement")]
    public float m_verticalSpeed;

    [BoxGroup ("Movement")]
    public float m_horizontalSpeed;

    [BoxGroup ("Movement")]
    public float m_slowHorizontalSpeed;

    [BoxGroup ("Movement")]
    public float m_slowVerticalSpeed;

    [BoxGroup ("Shot"), Range (0.05f, 2.5f)]
    public float m_shootInterval;

    [BoxGroup ("Shot")]
    public int m_bulletDamage;

    [BoxGroup ("Shot")]
    public float m_bulletSpeed;

    [BoxGroup ("Reference")]
    public SpriteRenderer m_spriteReference;

    private void OnDrawGizmosSelected ()
    {
        Gizmos.DrawWireSphere (transform.position, m_checkBound);
    }
}