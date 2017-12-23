using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioWhenShot : MonoBehaviour
{
    public PlayerProperty _playerProperty;

    public AudioClip m_shotAudio;

    private void OnEnable()
    {
        _playerProperty.m_eventMaster.OnShot += PlayShotAudio;    
    }

    private void OnDisable()
    {
        _playerProperty.m_eventMaster.OnShot -= PlayShotAudio;
    }

    private void PlayShotAudio(JIBulletController bullet)
    {
        _playerProperty.m_playerAudio.clip = m_shotAudio;
        _playerProperty.m_playerAudio.Play();
    }

}
