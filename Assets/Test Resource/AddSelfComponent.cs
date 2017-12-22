using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSelfComponent : MonoBehaviour
{
    public GameObject m_parent;

    public Material m_material;

    public void AddEventMaster()
    {
        foreach (var spriteRender in m_parent.GetComponentsInChildren<AnimationToggle.TakeDamageFlicker>(true))
        {
            spriteRender.m_flickerBindFactor = 0.75f;
        }
    }

}
