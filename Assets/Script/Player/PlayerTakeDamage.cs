using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamage: MonoBehaviour
{
    // The animation Object that used to show player death animation.
    public GameObject m_Explosion;

    // The player's god mode time after it destroied.
    public float m_godModeTime;

    // The player property. 
    public PlayerProperty m_playerProperty; 

    public SpriteRenderer m_playerSprite;

    // Player Death.
    void OnTriggerEnter2D(Collider2D other)
    {
        string otherTag = other.tag;

        // Enemy bullet or Enemy
        if(otherTag.Contains("Enemy"))
        {
            if(! m_playerProperty.m_tgm)
            {
                Instantiate(m_Explosion, transform.position, Quaternion.identity, transform.parent);
                StartCoroutine(TurnOnGodMode());
            }  
        }   
    }


    IEnumerator TurnOnGodMode()
    {
        float prevHorizontalSpeed = m_playerProperty.m_horizontalSpeed;
        float prevVerticalSpeed = m_playerProperty.m_verticalSpeed;
        Color prevColor = m_playerSprite.color;

        //m_playerProperty.m_horizontalSpeed = 0f;
        //m_playerProperty.m_verticalSpeed = 0f;
        m_playerSprite.color = new Color(prevColor.r, 
                                         prevColor.g, 
                                         prevColor.b,
                                         0.3f);
        m_playerProperty.m_tgm = true;

        yield return new WaitForSeconds(m_godModeTime);

        m_playerProperty.m_tgm = false;
        //m_playerProperty.m_horizontalSpeed = prevHorizontalSpeed;
        //m_playerProperty.m_verticalSpeed = prevVerticalSpeed;
        m_playerSprite.color = prevColor;

    }

}