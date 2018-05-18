using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlayerPoint : MonoBehaviour
{
    public Transform WhiteMask;

    public Transform BlackMask;

    private PlayerProperty _player;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable ()
    {
        _player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerProperty> ();
    }

    private void Update ()
    {
        float whitePercent = 1f * _player.m_playerWhitePoint / _player.m_maxPlayerPoint;
        float blackPercent = 1f * _player.m_playerBlackPoint / _player.m_maxPlayerPoint;

        whitePercent = Mathf.Clamp01 (whitePercent);
        blackPercent = Mathf.Clamp01 (blackPercent);

        WhiteMask.localScale = new Vector3 (whitePercent, 1, 1);
        BlackMask.localScale = new Vector3 (blackPercent, 1, 1);
    }

}