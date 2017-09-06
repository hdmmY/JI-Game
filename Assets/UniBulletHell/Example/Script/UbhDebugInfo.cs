using UnityEngine;
using System.Collections;

public class UbhDebugInfo : UbhMonoBehaviour
{
    const float INTERVAL_SEC = 1f;
    [SerializeField]
    GUIText _FpsGUIText;
    [SerializeField]
    GUIText _BulletNumGUIText;
    UbhObjectPool objectPool;
    float _LastUpdateTime;
    int _Frame = 0;

    void Start ()
    {
        if (Debug.isDebugBuild == false) {
            gameObject.SetActive(false);
            return;
        }
        _LastUpdateTime = Time.realtimeSinceStartup;
    }

    void Update ()
    {
        if (_FpsGUIText == null || _BulletNumGUIText == null) {
            return;
        }

        _Frame++;
        float time = Time.realtimeSinceStartup - _LastUpdateTime;

        if (INTERVAL_SEC <= time) {
            // Count FPS
            float frameRate = _Frame / time;
            _FpsGUIText.text = "FPS : " + ((int) frameRate).ToString();
            _LastUpdateTime = Time.realtimeSinceStartup;
            _Frame = 0;

            // Count Bullet Num
            if (objectPool == null) {
                objectPool = FindObjectOfType<UbhObjectPool>();
            }
            if (objectPool != null) {
                int bulletNum = objectPool.GetActivePooledObjectCount();
                _BulletNumGUIText.text = "Bullet Num : " + bulletNum.ToString();
            }
        }
    }
}
