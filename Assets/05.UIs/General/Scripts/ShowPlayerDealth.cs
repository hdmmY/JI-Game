using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
public class ShowPlayerDealth : MonoBehaviour
{
    public AnimationCurve DisappearAlphaCurve;

    public float DisappearTime;

    private SpriteRenderer _sprite;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake ()
    {
        _sprite = GetComponent<SpriteRenderer> ();

        EventManager.Instance.AddListener<GameOverEvent> (OnGameOver);
    }

    private void OnDestroy ()
    {
        EventManager.Instance.RemoveListener<GameOverEvent> (OnGameOver);
    }

    private void OnGameOver (GameOverEvent gameOverEvent)
    {
        StartCoroutine (ShowGameOverUI ());
    }

    private IEnumerator ShowGameOverUI ()
    {
        var targetDeathColor = _sprite.color;
        targetDeathColor.a = 0;

        _sprite.color = targetDeathColor;
        _sprite.enabled = true;

        float timer = 0;

        while (timer < DisappearTime)
        {
            targetDeathColor.a = DisappearAlphaCurve.Evaluate (timer);
            _sprite.color = targetDeathColor;
            timer += JITimer.Instance.RealDeltTime;
            yield return null;
        }

        var effect = GameObject.FindGameObjectWithTag ("MainCamera")?.GetComponent<BrightnessSaturationAndContrast> ();

        if (effect)
        {
            timer = 0;
            while (timer < 1)
            {
                JITimer.Instance.TimeScale = 0;
                timer += JITimer.Instance.RealDeltTime;
                effect.m_brightness = 1 - timer;
                yield return null;
            }
            effect.m_brightness = 0;
        }

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("Menu", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}