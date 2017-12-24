using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicWhenLoad : MonoBehaviour
{
    public AudioSource m_audio;

    private void Awake()
    {
        m_audio.Play();    
    }


}
