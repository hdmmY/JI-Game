using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperty : MonoBehaviour
{
    // god mode -- from Skyrim.
    public bool m_tgm = false;

    public bool m_superState = false;

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

    public int m_playerLife;   // Each life have muti health

    public int m_playerHealth;

    public float m_checkBound;

    public float m_playerNeutralization;

    public SpriteRenderer m_spriteReference;

    public PlayerEventMaster m_eventMaster;

    public AudioSource m_playerAudio;

    public void AddNeutralization(float value)
    {
        if (!m_superState)
        {
            m_playerNeutralization += value;
            m_playerNeutralization = Mathf.Clamp(m_playerNeutralization, 0, 100);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, m_checkBound);
    }

}
