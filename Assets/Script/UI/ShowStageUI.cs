using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStageUI : MonoBehaviour
{                                    
    public SpriteRenderer m_stageUI;

    private Color _stageUIColor;

    private void Awake()
    {
        _stageUIColor = m_stageUI.color;

        StartCoroutine(Show());
    }


    private IEnumerator Show()
    {
        m_stageUI.gameObject.SetActive(true);
        m_stageUI.color = new Color(_stageUIColor.r, _stageUIColor.g, _stageUIColor.b, 0);

        float timer = 0;

        // Fade in 
        while(timer < 1)
        {
            m_stageUI.color = new Color(_stageUIColor.r, _stageUIColor.g, _stageUIColor.b, timer);
            timer += JITimer.Instance.DeltTime;
            yield return null;
        }

        // Last
        timer = 0;
        while(timer < 2f)
        {
            timer += JITimer.Instance.DeltTime;
            yield return null;
        }

        // Fade out
        timer = 0;
        while (timer < 1)
        {
            m_stageUI.color = new Color(_stageUIColor.r, _stageUIColor.g, _stageUIColor.b, 1 - timer);
            timer += JITimer.Instance.DeltTime;
            yield return null;
        }

        m_stageUI.gameObject.SetActive(false);
    }

}
