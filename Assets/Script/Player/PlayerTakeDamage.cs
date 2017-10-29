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

    private void Update()
    {
        if(m_playerProperty.m_playerMoveState == PlayerProperty.PlayerMoveType.HighSpeed)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }


    IEnumerator TurnOnGodMode()
    {
        Color prevColor = m_playerSprite.color;
  
        m_playerSprite.color = new Color(prevColor.r, 
                                         prevColor.g, 
                                         prevColor.b,
                                         0.3f);
        m_playerProperty.m_tgm = true;

        yield return new WaitForSeconds(m_godModeTime);

        m_playerProperty.m_tgm = false;                         
        m_playerSprite.color = prevColor;             
    }

}