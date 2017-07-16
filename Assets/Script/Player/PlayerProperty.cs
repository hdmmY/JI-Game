using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperty : MonoBehaviour
{
    // movement
    public float m_verticalSpeed;
    public float m_horizontalSpeed;


    // shoot 
    public string m_leftShootPoint;
    public string m_rightShootPoint;
    [Range(0.05f, 2.5f)]
    public float m_shootInterval;
    public int m_bulletDamage;
    
    
    // state
    public enum PlayerStateType
    {
        Black,
        White
    };
    public PlayerStateType m_playerState;
    public Sprite m_WhiteSprite;
    public Sprite m_BlackSprite;


    // layer
    public LayerMask m_PlayerBulletLayer;
    public LayerMask m_PlayerLayer;

}
