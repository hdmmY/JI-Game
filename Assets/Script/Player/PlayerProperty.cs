using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperty : MonoBehaviour
{
    // god mode -- from Skyrim.
    public bool m_tgm = false;

    // movement
    public float m_verticalSpeed;
    public float m_horizontalSpeed;
    public float m_slowHorizontalSpeed;
    public float m_slowVerticalSpeed;

    // shoot 
    [Range(0.05f, 2.5f)]
    public float m_shootInterval;
    public int m_bulletDamage;
    public float m_bulletSpeed;
    
    
    // state
    public JIState m_playerState;

    
    public enum PlayerMoveType
    {
        HighSpeed,
        SlowSpeed
    };

    public PlayerMoveType m_playerMoveState;

    public int m_playerHealth;

    public int m_playerNeutralization;

    public SpriteRenderer m_spriteReference;


    public void AddNeutralization(int value)
    {
        m_playerNeutralization = Mathf.Clamp(m_playerNeutralization + value, 0, 100);
    }
}
