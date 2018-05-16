using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerProperty : MonoBehaviour
{
    // god mode -- from Skyrim.
    public bool m_god = false;

    public bool m_superState = false;

    public JIState m_playerState;

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

    [BoxGroup ("Movement")]
    public float m_blackVSpeed;

    [BoxGroup ("Movement")]
    public float m_blackHSpeed;

    [BoxGroup ("Movement")]
    public float m_whiteVSpeed;

    [BoxGroup ("Movement")]
    public float m_whiteHSpeed;

    [BoxGroup ("Reference")]
    public SpriteRenderer m_spriteReference;

    #region Public method

    public float GetHMoveSpeed ()
    {
        if (m_playerState == JIState.Black)
        {
            return m_blackHSpeed;
        }
        else if (m_playerState == JIState.White)
        {
            return m_whiteHSpeed;
        }
        return 0;
    }

    public float GetVMoveSpeed ()
    {
        if (m_playerState == JIState.Black)
        {
            return m_blackVSpeed;
        }
        else if (m_playerState == JIState.White)
        {
            return m_whiteVSpeed;
        }
        return 0;
    }

    #endregion
}