using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSelfComponent : MonoBehaviour
{
    public GameObject m_parent;

    public Material m_material;

    public AnimationToggle.TakeDamageFlicker m_prefab;

    public void AddEventMaster()
    {
        foreach(var enemyProperty in m_parent.GetComponentsInChildren<EnemyProperty>(true))
        {
            // Add Sprite Reference
            enemyProperty.m_enemySprite = enemyProperty.GetComponent<SpriteRenderer>();

            // Set Sprite Material
            enemyProperty.m_enemySprite.material = m_material;


            // Add Event Master
            var eventMaster = enemyProperty.GetComponent<EnemyEventMaster>();
            if(eventMaster == null)
            {
                eventMaster = enemyProperty.gameObject.AddComponent<EnemyEventMaster>();
            }

            // Add Sprite Flicker
            var flicker = enemyProperty.m_enemySprite.GetComponent<SpriteFlicker>();
            if(flicker == null)
            {
                flicker = enemyProperty.m_enemySprite.gameObject.AddComponent<SpriteFlicker>();
            }

            // Set Sprite Flicker
            flicker.m_bindFactor = 0;
            flicker.m_flickerColor = Color.white;


            // Add Flicker Controller
            var flickerController = Instantiate(m_prefab);

            // Set Flicker Controller
            flickerController.transform.SetParent(enemyProperty.transform);
            flickerController.gameObject.name = "Event Collection";
            flickerController.transform.localPosition = Vector3.zero;
            flickerController.m_enemyEventMaster = eventMaster;
            flickerController.m_flickerBindFactor = 0.75f;
            flickerController.m_flickerColor = Color.white;
            flickerController.m_flickerTime = 0.03f;


            Debug.Log("Finish");
        }
    }

}
