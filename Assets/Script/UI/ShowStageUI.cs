using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ShowStageUI : MonoBehaviour
{                                    
    public SpriteRenderer m_stageUI;

    [CustomValueDrawer("ValidTimeValue")]
    public float m_fadeInTime = 1;
    [CustomValueDrawer("ValidTimeValue")]
    public float m_lastTime = 2;
    [CustomValueDrawer("ValidTimeValue")]
    public float m_fadeOutTime = 1;

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
        while(timer < m_fadeInTime)
        {
            m_stageUI.color = new Color(_stageUIColor.r, _stageUIColor.g, _stageUIColor.b, timer / m_fadeInTime);
            timer += JITimer.Instance.DeltTime;
            yield return null;
        }

        // Last
        timer = 0;
        while(timer < m_lastTime)
        {
            timer += JITimer.Instance.DeltTime;
            yield return null;
        }

        // Fade out
        timer = 0;
        while (timer < m_fadeOutTime)
        {
            m_stageUI.color = new Color(_stageUIColor.r, _stageUIColor.g, _stageUIColor.b, 1 - timer / m_fadeOutTime);
            timer += JITimer.Instance.DeltTime;
            yield return null;
        }

        m_stageUI.gameObject.SetActive(false);
    }


    private static float ValidTimeValue(float value, GUIContent label)
    {
        value = value < 0.01f ? 0.01f : value;
        return UnityEditor.EditorGUILayout.FloatField(label, value);
    }
}
