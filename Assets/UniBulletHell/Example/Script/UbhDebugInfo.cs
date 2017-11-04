using UnityEngine;
using System.Collections;

public class UbhDebugInfo : UbhMonoBehaviour
{
    const float INTERVAL_SEC = 1f;
    [SerializeField]
    GUIText _FpsGUIText;
    [SerializeField]
    GUIText _BulletNumGUIText;
    [SerializeField]
    GUIText _timerGUIText;

    [SerializeField]
    TimeManager _timeManager;

    UbhObjectPool objectPool;
    
    
    void Update ()
    {
        // Count FPS
        if(_FpsGUIText != null)
        {
            _FpsGUIText.text = "FPS : " + (int)(1f / Time.deltaTime);
        }


        // Count Bullet Num
        if (objectPool == null)
        {
            objectPool = FindObjectOfType<UbhObjectPool>();
        }
        if (objectPool != null && _BulletNumGUIText != null)
        {
            int bulletNum = objectPool.GetActivePooledObjectCount();
            _BulletNumGUIText.text = "Bullet Num : " + bulletNum.ToString();
        }
        
        if(_timeManager != null && _timerGUIText)
        {
            _timerGUIText.text = "Time : " + (int)_timeManager.m_timer;
        }

    }
}
