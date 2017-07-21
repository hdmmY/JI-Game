using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineManager : MonoBehaviour
{

    public List<TimeLineGameObject> m_timeLineGameObject;


    [System.Serializable]
    public class TimeLineGameObject
    {
        public GameObject m_gameObject;
        public float m_activeTime;
        public float m_disactiveTime;
    }







}
