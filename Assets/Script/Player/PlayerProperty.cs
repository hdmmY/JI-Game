using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerProperty : MonoBehaviour
{
    // god mode -- from Skyrim.
    public bool m_god = false;

    public bool m_superState = false;

    public JIState m_playerState;

    public PlayerMoveType m_playerMoveState;

    public int m_playerLife;   // Each life have muti health

    public int m_playerHealth;

    public float m_playerNeutralization;

    public float m_checkBound;

    [BoxGroup("Movement")]
    public float m_verticalSpeed;

    [BoxGroup("Movement")]
    public float m_horizontalSpeed;

    [BoxGroup("Movement")]
    public float m_slowHorizontalSpeed;

    [BoxGroup("Movement")]
    public float m_slowVerticalSpeed;

    [BoxGroup("Shot"), Range(0.05f, 2.5f)]
    public float m_shootInterval;

    [BoxGroup("Shot")]
    public int m_bulletDamage;

    [BoxGroup("Shot")]
    public float m_bulletSpeed;

    public enum PlayerMoveType
    {
        HighSpeed,
        SlowSpeed
    };

    [BoxGroup("Reference")]
    public SpriteRenderer m_spriteReference;

    [BoxGroup("Reference")]
    public PlayerEventMaster m_eventMaster;

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
