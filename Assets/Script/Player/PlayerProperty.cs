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

    // shoot 
    [Range(0.05f, 2.5f)]
    public float m_shootInterval;
    public int m_bulletDamage;
    public float m_bulletSpeed;
    
    
    // state
    public enum PlayerStateType
    {
        Black,
        White
    };
    public PlayerStateType m_playerState;

    
    public enum PlayerMoveType
    {
        HighSpeed,
        SlowSpeed
    };

    public PlayerMoveType m_playerMoveState;
}
