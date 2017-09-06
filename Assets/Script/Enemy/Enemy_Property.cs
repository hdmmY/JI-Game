using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Property: MonoBehaviour
{
    public enum EnemyState
    {
        Black,
        White
    };

    public EnemyState m_enemyState;

    public int m_enmeyHealth;

    public int m_EnemyBulletLayer;

    public int m_EnemyLayer;

    [System.Serializable]
    public class TimeLineEmitPoint
    {
        public GameObject m_emitPoint;
        public float m_activeTime;
        public float m_disactiveTime;
    }

    public List<TimeLineEmitPoint> m_TimeLineEmitPoints;

}
