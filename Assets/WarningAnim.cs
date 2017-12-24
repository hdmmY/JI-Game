using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningAnim : MonoBehaviour
{
    public float m_totalTime = 8f;

    public Vector3 m_startPos;
    public Vector3 m_endPos;

    private SpriteRenderer _sprite;
    private Color _spriteColor;

    private void Start()
    {
        _spriteColor = GetComponent<SpriteRenderer>().color;
        _sprite = GetComponent<SpriteRenderer>();

        // Reset
        _sprite.color = new Color(_spriteColor.r, _spriteColor.g, _spriteColor.b, 1);

        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        float timer = 0;

        transform.position = m_startPos;

        Vector3 speed = (m_endPos - m_startPos) / (m_totalTime - 4);

        // Move
        while (timer < m_totalTime - 4)
        {
            transform.position += speed * JITimer.Instance.DeltTime;
            timer += JITimer.Instance.DeltTime;
            yield return null;
        }

        // Last
        timer = 0;
        while(timer < 2)
        {
            timer += JITimer.Instance.DeltTime;
            yield return null;
        }


        // Fade
        timer = 0;
        while (timer < 2)
        {
            _sprite.color = new Color(_spriteColor.r, _spriteColor.g, _spriteColor.b, 1 - timer / 2f);
            timer += JITimer.Instance.DeltTime;
            yield return null;
        }


    }


}
