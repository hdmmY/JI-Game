using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowPlayerStatus : MonoBehaviour
{
    public TMP_Text m_life;

    public TMP_Text m_health;

    public TMP_Text m_blackPoint;

    public TMP_Text m_whitePoint;

    private PlayerProperty _player;

    private static readonly string NuLLText = "Null"; 

    private void Start ()
    {
        if (!m_life || !m_health || !m_blackPoint || !m_whitePoint)
        {
            Debug.LogError ("Please set the correspond text!", this);
        }

        if(GameObject.FindGameObjectWithTag ("Player"))
        {
            _player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerProperty> ();
        }
    }

    private void Update ()
    {
        if (_player)
        {
            m_life.text = NumberUtil.NumberFrom0to600[_player.m_playerLife];
            m_health.text = NumberUtil.NumberFrom0to600[_player.m_playerHealth];
            m_whitePoint.text = NumberUtil.NumberFrom0to600[_player.m_playerWhitePoint];
            m_blackPoint.text = NumberUtil.NumberFrom0to600[_player.m_playerBlackPoint];
        }
        else
        {
            m_life.text = NuLLText;
            m_health.text = NuLLText;
            m_whitePoint.text = NuLLText;
            m_blackPoint.text = NuLLText;
        }
    }

}