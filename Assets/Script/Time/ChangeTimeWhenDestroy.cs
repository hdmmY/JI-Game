using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTimeWhenDestroy : MonoBehaviour {

    public TimeManager m_timeManager;

    public float m_destTime;

    private void OnDestroy()
    {
        m_timeManager.m_timer = m_destTime;
    }

}
