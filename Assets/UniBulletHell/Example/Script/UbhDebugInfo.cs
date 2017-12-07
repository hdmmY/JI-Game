using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class UbhDebugInfo : UbhMonoBehaviour
{
    const float INTERVAL_SEC = 1f;

    public Text m_fpsText;
    public Text m_bulletNumText;
    public Text m_timerText;
    public Text m_playerHealthText;

    [SerializeField]
    private TimeManager _timeManager;

    [SerializeField]
    private PlayerProperty _playerProperty;

    void Update()
    {
        // Count FPS
        if (m_fpsText != null)
        {
            m_fpsText.text = "FPS : " + (int)(1f / Time.deltaTime);
        }


        // Count Bullet Num
        if (m_bulletNumText!= null)
        {
            int bulletNum = UbhObjectPool.Instance.GetActivePooledObjectCount();
            m_bulletNumText.text = "Bullet Num : " + bulletNum.ToString();
        }

        if (_timeManager != null && m_timerText != null)
        {
            m_timerText.text = "Time : " + (int)_timeManager.m_timer;
        }

        if (_playerProperty != null && m_playerHealthText != null)
        {
            m_playerHealthText.text = "PlayerHealth : " + _playerProperty.m_playerHealth;
        }
    }
}
