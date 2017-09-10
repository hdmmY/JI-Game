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

        // The time before the Go release (destroy)
        public float _releaseTime = 0.1f;
        public float ReleaseTime
        {
            get
            {
                return _releaseTime;
            }
            set
            {
                _releaseTime = value <= _activeTime ? _activeTime + 0.1f : value;
            }
        }

        // Whether the game object is active.
        [HideInInspector]
        public bool _active = false;
        public bool Active
        {
            get
            {
                return Go.active;
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

    // A list of the time controled gameobject.
    public List<TimeGameobject> m_timeGos;

    // A timer for active or disactive gameobject.
    private float _timer;


    void OnEnable()
    {
        // Reset timer.
        _timer = 0f;


        if(m_useConstantInterval)
        {
            for(int i = 1; i < m_timeGos.Count; i++)
            {
                m_timeGos[i]._activeTime += m_timeGos[i - 1]._activeTime + m_ConstantInterval;
            }
        }
    }


    void Update()
    {
        _timer += UbhTimer.Instance.DeltaTime;

        for(int i = 0; i < m_timeGos.Count; i++)
        {
            // Not reach the active time.
            if(_timer < m_timeGos[i].ActiveTime)
            {
                if(m_timeGos[i].Active)
                    m_timeGos[i].Active = false;
                continue;
            }

            // Reach the active time, but not reach the release time
            if(_timer < m_timeGos[i].ReleaseTime)
            {
                if(!m_timeGos[i].Active)
                    m_timeGos[i].Active = true;
                continue;
            }

            // Release the gameobject.
            if(_timer >= m_timeGos[i].ReleaseTime)
            {
                if(m_timeGos[i].Active)
                    m_timeGos[i].Active = false;
                continue;
            }
        }

        return;
    }
}
