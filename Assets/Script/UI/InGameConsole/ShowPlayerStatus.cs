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

	private void Start ()
	{
		if (!m_life || !m_health || !m_blackPoint || !m_whitePoint)
		{
			Debug.LogError ("Please set the correspond text!", this);
		}

		_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerProperty> ();
	}

	private void Update ()
	{
		m_life.text = NumberUtil.NumberFrom1to300[_player.m_playerLife];
		m_health.text = NumberUtil.NumberFrom1to300[_player.m_playerHealth];
		m_whitePoint.text = NumberUtil.NumberFrom1to300[_player.m_playerWhitePoint];
		m_blackPoint.text = NumberUtil.NumberFrom1to300[_player.m_playerBlackPoint];
	}

}