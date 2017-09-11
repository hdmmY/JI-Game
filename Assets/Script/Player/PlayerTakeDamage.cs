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

    // Player Death.
    void OnTriggerEnter2D(Collider2D other)
    {
        string otherTag = other.tag;

        // Enemy bullet or Enemy
        if(otherTag.Contains("Enemy"))
        {
            if(! m_playerProperty.m_tgm)
            {
                Instantiate(m_Explosion, transform.position, Quaternion.identity);
                StartCoroutine(TurnOnGodMode());
            }  
        }   
    }


    IEnumerator TurnOnGodMode()
    {
        m_playerProperty.m_tgm = true;

        yield return UbhUtil.WaitForSeconds(m_godModeTime);

        m_playerProperty.m_tgm = false;
    }

}