using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // GameObject that controlleed by TimeManager.
    [System.Serializable]
    public class TimeGameobject
    {
        public GameObject Go;

        // The time before the Go active.
        public float _activeTime = 0f;
        public float ActiveTime
        {
            get
            {
                return _activeTime;
            }
            set
            {
                _activeTime = value <= 0 ? 0 : value;
            }
        }

        // Whether the game object is active.
        [HideInInspector]
        public bool _active = false;
        public bool Active
        {
            get
            {
                return Go.activeInHierarchy;
            }
            set
            {
                Go.SetActive(value);
            }
        }
    }

    // Use constant interval to active gameobject.
    public bool m_useConstantInterval;
    public float m_ConstantInterval;

    public bool m_destroySelfReachEndTime = true;

    // A list of the time controled gameobject.
    public List<TimeGameobject> m_timeGos;

    private List<bool> _activedTimeGos;

    // A timer for active or disactive gameobject.
    [SerializeField ]
    private float _timer;
                                        
    void OnEnable()
    {
        // Reset timer. The init _timer is a little below zero to aviod inaccuracy.
        _timer = -0.5f;

        _activedTimeGos = new List<bool>(m_timeGos.Count);
        for (int i = 0; i < m_timeGos.Count; i++) _activedTimeGos.Add(false);

        if (m_useConstantInterval)
        {
            for (int i = 1; i < m_timeGos.Count; i++)
            {
                m_timeGos[i]._activeTime += m_timeGos[i - 1]._activeTime + m_ConstantInterval;
            }
        }
    }


    void Update()
    {
        _timer += UbhTimer.Instance.DeltaTime;

        for (int i = 0; i < m_timeGos.Count; i++)
        {
            if (_activedTimeGos[i]) continue;

            if (m_timeGos[i].Go == null) continue;

            // Not reach the active time.
            if (_timer < m_timeGos[i].ActiveTime)
            {
                if (m_timeGos[i].Active)
                    m_timeGos[i].Active = false;
                continue;
            }

            // Reach the active time.
            if(_timer >= m_timeGos[i].ActiveTime)
            {
                if (!m_timeGos[i].Active)
                {
                    m_timeGos[i].Active = true;
                    _activedTimeGos[i] = true;
                }                                                 
            }
        }    
        

        // Destroy this component when all object has been actived.
        if(m_destroySelfReachEndTime)
        {
            if (!_activedTimeGos.Contains(false))
                Destroy(this);
        }
    }
}
