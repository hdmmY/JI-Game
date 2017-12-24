using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningAnim : MonoBehaviour
{
    public float m_totalTime = 8f;         

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
        // Fade in
        float timer = 0;
        while (timer < 1)
        {
            _sprite.color = new Color(_spriteColor.r, _spriteColor.g, _spriteColor.b, timer);
            timer += JITimer.Instance.DeltTime;
            yield return null;
        }

        // Last
        timer = 0;
        while(timer < m_totalTime - 2)
        {
            timer += JITimer.Instance.DeltTime;
            yield return null;
        }


        // Fade
        timer = 0;
        while (timer < 1)
        {
            _sprite.color = new Color(_spriteColor.r, _spriteColor.g, _spriteColor.b, 1 - timer);
            timer += JITimer.Instance.DeltTime;
            yield return null;
        }            
    }


}
