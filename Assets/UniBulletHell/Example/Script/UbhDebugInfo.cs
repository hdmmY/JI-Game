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
    float _timer;

    UbhObjectPool objectPool;
    float _LastUpdateTime;
    int _Frame = 0;

    void Start()
    {
        if (Debug.isDebugBuild == false)
        {
            gameObject.SetActive(false);
            return;
        }
        _LastUpdateTime = Time.realtimeSinceStartup;

        // Reset timer. The init _timer is a little below zero to aviod inaccuracy.
        _timer = -0.5f;
    }

    void Update()
    {
        if (_FpsGUIText == null || _BulletNumGUIText == null)
        {
            return;
        }

        _Frame++;
        float time = Time.realtimeSinceStartup - _LastUpdateTime;

        if (INTERVAL_SEC <= time)
        {
            // Count FPS
            float frameRate = _Frame / time;
            _FpsGUIText.text = "FPS : " + ((int)frameRate).ToString();
            _LastUpdateTime = Time.realtimeSinceStartup;
            _Frame = 0;

            // Count Bullet Num
            if (objectPool == null)
            {
                objectPool = FindObjectOfType<UbhObjectPool>();
            }
            if (objectPool != null)
            {
                int bulletNum = objectPool.GetActivePooledObjectCount();
                _BulletNumGUIText.text = "Bullet Num : " + bulletNum.ToString();
            }
        }

        // Time
        _timer += UbhTimer.Instance.DeltaTime;
        _timerGUIText.text = "Time : " + ((int)_timer).ToString();
    }
}
