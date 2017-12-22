using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSelfComponent : MonoBehaviour
{
    public GameObject m_parent;

    public Sprite m_targetSprite;

    public void AddEventMaster()
    {
        foreach(var enemyProperty in m_parent.GetComponentsInChildren<EnemyProperty>(true))
        {
            enemyProperty.m_enemySprite.sprite = m_targetSprite;

            enemyProperty.GetComponent<CircleCollider2D>().radius = 0.13f;
        }
    }

}
