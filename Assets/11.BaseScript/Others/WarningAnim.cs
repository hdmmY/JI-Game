using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningAnim : MonoBehaviour
{
    public float m_flickerTime = 0.3f;

    public AudioSource m_backgroundAudio;
    public AudioSource m_warningAudio;

    private SpriteRenderer _sprite;
    private Color _spriteColor;

    private void Start()
    {
        _spriteColor = GetComponent<SpriteRenderer>().color;
        _sprite = GetComponent<SpriteRenderer>();

        // Reset
        _sprite.color = new Color(_spriteColor.r, _spriteColor.g, _spriteColor.b, 0);

        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        m_backgroundAudio.Pause();
        m_warningAudio.Play();



		// Flicker
		for (int i = 0; i < 3; i++)
		{
			float timer = 0;
			while(timer < m_flickerTime)
			{
				timer += JITimer.Instance.DeltTime;
				_sprite.color = new Color(_spriteColor.r, _spriteColor.g, _spriteColor.b, timer);
				yield return null;
			}
			_sprite.color = new Color(_spriteColor.r, _spriteColor.g, _spriteColor.b, 1);
			timer = 0;
			while(timer < m_flickerTime)
			{
				timer += JITimer.Instance.DeltTime;
				_sprite.color = new Color(_spriteColor.r, _spriteColor.g, _spriteColor.b, 1 - timer);
				yield return null;
			}
			_sprite.color = new Color(_spriteColor.r, _spriteColor.g, _spriteColor.b, 0);
		}

        m_backgroundAudio.Play();
        m_warningAudio.Pause();
        m_warningAudio.enabled = false;
    }


}
