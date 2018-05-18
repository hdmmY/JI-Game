using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TimeManager : MonoBehaviour
{
    // GameObject that controlleed by TimeManager.
    [System.Serializable]
    public class TimeGameobject
    {
        [ValidateInput("NotNull", "The gameobject is not set!")]
        public GameObject Go;

        // The time before the Go active.
        [SerializeField, HideInInspector]
        private float _activeTime = 0f;
        [ShowInInspector]
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

        [ReadOnly, ShowInInspector]
        public bool Active
        {
            get
            {
                if (Go != null)
                {
                    return Go.activeInHierarchy;
                }
                return false;
            }
            set
            {
                if (Go != null)
                {
                    Go.SetActive(value);
                }
            }
        }

        private bool NotNull(GameObject go)
        {
            return go != null;
        }
    }

    /// <summary>
    /// Use constant interval to active gameobject.
    /// </summary>
    public bool m_useConstantInterval;

    [ShowIf("m_useConstantInterval")]
    [Range(0.01f, 5)]
    public float m_ConstantInterval;

    public bool m_destroySelfReachEndTime = true;


    /// <summary>
    /// A list of the time controled gameobject.
    /// </summary>
    [ListDrawerSettings(DraggableItems = true, Expanded = true, ShowIndexLabels = true, NumberOfItemsPerPage = 20)]
    public List<TimeGameobject> m_timeGos;

    private List<bool> _activedTimeGos;

    /// <summary>
    /// A timer for active or disactive gameobject.
    /// </summary>
    [ShowInInspector]
    [ReadOnly]
    public float m_timer;

    void Awake()
    {
        // Reset timer. The init _timer is a little below zero to aviod inaccuracy.
        m_timer = -0.5f;

        _activedTimeGos = new List<bool>(m_timeGos.Count);
        for (int i = 0; i < m_timeGos.Count; i++) _activedTimeGos.Add(false);

        if (m_useConstantInterval)
        {
            for (int i = 1; i < m_timeGos.Count; i++)
            {
                m_timeGos[i].ActiveTime += m_timeGos[i - 1].ActiveTime + m_ConstantInterval;
            }
        }
    }

    void Update()
    {
        m_timer += JITimer.Instance.DeltTime;

        for (int i = 0; i < m_timeGos.Count; i++)
        {
            if (_activedTimeGos[i]) continue;

            if (m_timeGos[i].Go == null) continue;

            // Not reach the active time.
            if (m_timer < m_timeGos[i].ActiveTime)
            {
                if (m_timeGos[i].Active)
                    m_timeGos[i].Active = false;
                continue;
            }

            // Reach the active time.
            if (m_timer >= m_timeGos[i].ActiveTime)
            {
                if (!m_timeGos[i].Active)
                {
                    m_timeGos[i].Active = true;
                    _activedTimeGos[i] = true;
                }
            }
        }


        // Destroy this component when all object has been actived.
        if (m_destroySelfReachEndTime)
        {
            if (!_activedTimeGos.Contains(false))
                Destroy(this);
        }
    }

    [ButtonGroup("ActiveGroup")]
    [Button("SetActive", ButtonSizes.Small)]
    public void SetAllTimeGameobjectActive()
    {
        if(m_timeGos == null || m_timeGos.Count == 0)
        {
            return;
        }

        foreach(var timeGameobject in m_timeGos)
        {
            timeGameobject.Active = true;
        }
    }


    [ButtonGroup("ActiveGroup")]
    [Button("SetDeactive", ButtonSizes.Small)]
    public void SetAllTimeGameobjectDeactive()
    {
        if(m_timeGos == null || m_timeGos.Count == 0)
        {
            return;
        }

        foreach(var timeGameobject in m_timeGos)
        {
            timeGameobject.Active = false;
        }
    }
}
