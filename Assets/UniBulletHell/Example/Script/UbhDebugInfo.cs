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

    void Start ()
    {
        if (Debug.isDebugBuild == false) {
            gameObject.SetActive(false);
            return;
        }

        // Reset timer. The init _timer is a little below zero to aviod inaccuracy.
        _timer = -0.5f;
    }

    void Update ()
    {
        if (_FpsGUIText == null || _BulletNumGUIText == null) {
            return;
        }

        // Count FPS
        _FpsGUIText.text = "FPS : " + 1f / Time.deltaTime;

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

        // Time
        _timer += UbhTimer.Instance.DeltaTime;
        _timerGUIText.text = "Time : " + ((int)_timer).ToString();
    }
}
