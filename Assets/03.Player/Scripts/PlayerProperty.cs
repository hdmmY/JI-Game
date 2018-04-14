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

    #region  Events
    /// <summary>
    /// Player being damage event, called when player being hitted
    /// </summary>
    /// <remarks>
    /// TakeDamage(int curLife, int curHealth)
    /// </remarks>
    public Action<int, int> TakeDamage;
    #endregion

    /// <summary>
    /// Add player point
    /// </summary>
    /// <remarks>
    /// If the bullet state is black/white, the player's black/white point will increment
    /// If the bullet state is none, nothing will happed
    /// If the bullet state is all, the player's black and white point will increment
    /// </remarks>
    public void AddNeutralization (int value, JIState bulletState)
    {
        if (!m_superState)
        {
            switch (bulletState)
            {
                case JIState.All:
                    m_playerBlackPoint += value;
                    m_playerWhitePoint += value;
                    break;
                case JIState.Black:
                    m_playerBlackPoint += value;
                    break;
                case JIState.White:
                    m_playerWhitePoint += value;
                    break;
                case JIState.None:
                    break;
            }

            m_playerBlackPoint = Mathf.Clamp (m_playerBlackPoint, 0, m_maxPlayerPoint);
            m_playerWhitePoint = Mathf.Clamp (m_playerWhitePoint, 0, m_maxPlayerPoint);
        }
    }

    private void OnDrawGizmosSelected ()
    {
        Gizmos.DrawWireSphere (transform.position, m_checkBound);
    }
}