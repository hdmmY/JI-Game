using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTimeWhenBothDestroy : MonoBehaviour
{
    public GameObject m_go1;
    public GameObject m_go2;

    public TimeManager m_timeManager;

    public float m_targetTime;

    private void Update()
    {
        if(m_go1 == null && m_go2 == null)
        {
            m_timeManager.m_timer = m_targetTime;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

}
