using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HDMToggle : MonoBehaviour
{
    public TimeManager m_timeManager;

    [Button("OnClick", ButtonSizes.Medium)]
    public void OnClick()
    {
        foreach(var timeGameObject in m_timeManager.m_timeGos)
        {
           
        }


    }
}
