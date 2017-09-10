using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperty : MonoBehaviour
{
    // movement
    public float m_verticalSpeed;
    public float m_horizontalSpeed;


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
    public Sprite m_WhiteSprite;
    public Sprite m_BlackSprite;


    // layer
    public string m_PlayerBulletLayer;
    public string m_PlayerLayer;

}
