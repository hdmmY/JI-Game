using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Enemy_Property))]
public class StageManager : MonoBehaviour
{

    [System.Serializable]
    public class StageGameObject
    {
        // Target stage gameobject
        public GameObject m_stageDataGO;

        private JiPathMoveCtrl _pathMoveComponent;
        public JiPathMoveCtrl PathMoveComponent
        {
            get
            {
                return m_stageDataGO.GetComponent<JiPathMoveCtrl>();
            }
        }

        private UbhShotCtrl _shotCtrlComponent;
        public UbhShotCtrl ShotCtrlComponent
        {
            get
            {
                return m_stageDataGO.GetComponent<UbhShotCtrl>();
            }
        }

        [Space]

        public float m_thresoldTime;
        public float m_thresoldHealth;
        public List<GameObject> m_destroiedGos;
    }


    public List<StageGameObject> m_stages;

    private float _timer;

    private Enemy_Property _property;


    private void Start()
    {
        // Reset timer. The init _timer is a little below zero to aviod inaccuracy.
        _timer = -0.5f;

        _property = GetComponent<Enemy_Property>();

        for(int i = 0; i < m_stages.Count; i++)
        {
            if (m_stages[i].m_stageDataGO == null) continue;
            m_stages[i].m_stageDataGO.SetActive(false);
        }

        InvokeStage(0);
    }


    private void Update()
    {
        _timer += UbhTimer.Instance.DeltaTime;


        for (int i = 0; i < m_stages.Count - 1; i++)
        {
            // Use for 'm_thresoldTime' condition
            if (m_stages[i].m_thresoldTime != 0)
            {
                if (m_stages[i].m_thresoldTime <= _timer)
                {
                    InvokeStage(i + 1);
                    continue;
                }
            }

            // Use for 'm_thresoldHealth' condition
            if (m_stages[i].m_thresoldHealth != 0)
            {
                if (_property.m_health < m_stages[i].m_thresoldHealth)
                {
                    InvokeStage(i + 1);
                    continue;
                }
            }

            // Use for 'm_destroiedGos' condition
            if (m_stages[i].m_destroiedGos.Count != 0)
            {
                bool allDestroied = true;
                foreach (var go in m_stages[i].m_destroiedGos)
                {
                    if (go != null)
                    {
                        allDestroied = false;
                        break;
                    }
                }

                if (allDestroied)
                {
                    InvokeStage(i + 1);
                    continue;
                }
            }
        }   

    }


    private void InvokeStage(int stageIndex)
    {
        if(stageIndex != 0)
        {
            Destroy(m_stages[stageIndex - 1].m_stageDataGO);
            m_stages[stageIndex - 1].m_stageDataGO = null;
        }
                     

        GameObject stageData = m_stages[stageIndex].m_stageDataGO;

        if (stageData == null)
        {
            Destroy(this.gameObject);
        }

        if(stageData.GetComponent<JiPathMoveCtrl>() != null)
        {
            stageData.GetComponent<JiPathMoveCtrl>().m_targetGameObject = this.gameObject;
        }
        
        stageData.SetActive(true);                                                          
    }


}
