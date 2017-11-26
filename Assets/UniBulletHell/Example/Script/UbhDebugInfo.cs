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
    GUIText _playerHealthGUIText;

    [SerializeField]
    TimeManager _timeManager;

    [SerializeField]
    PlayerProperty _playerProperty;

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
        
        if(_timeManager != null && _timerGUIText != null)
        {
            _timerGUIText.text = "Time : " + (int)_timeManager.m_timer;
        }

        if(_playerProperty != null && _playerHealthGUIText != null)
        {
            _playerHealthGUIText.text = "PlayerHealth : " + _playerProperty.m_playerHealth;
        }
    }
}
