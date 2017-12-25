using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSelfComponent : MonoBehaviour
{
    public TimeManager m_timeManager;

    public void AddEventMaster()
    {
        foreach(var go in m_timeManager.m_timeGos)
        {
            foreach (var script in go.Go.GetComponentsInChildren<ChangeTimeWhenDestroy>())
            {
                Debug.Log(script.gameObject.name);
            }  
        }                                    
    }
}
