using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ChangeTimeWhenDestroy : MonoBehaviour
{
    [SceneObjectsOnly]
    public List<GameObject> m_monitedGameobject;

    public TimeManager m_timeManager;

    public float m_destTime;

    private void Update()
    {
        bool allDead = true;

        for (int i = 0; i < m_monitedGameobject.Count; i++)
        {
            if (m_monitedGameobject[i] != null)
            {
                allDead = false;
                break;
            }
        }

        if (allDead)
        {
            m_timeManager.m_timer = m_destTime;

            this.enabled = false;
            Destroy(this);
        }
    }
}
