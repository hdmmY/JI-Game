using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageFlicker : MonoBehaviour
{
    [Range (0, 1)]
    public float m_flickerTime;

    [Range (0, 1)]
    public float m_flickerBindFactor = 0.75f;

    public Color m_flickerColor;

    private EnemyProperty _enemy;

    private void OnEnable ()
    {
        _enemy = _enemy ?? GetComponentInParent<EnemyProperty> ();
        _enemy.OnDamage += StartFlicker;
    }

    private void OnDisable ()
    {
        _enemy.OnDamage -= StartFlicker;
    }

    private void StartFlicker (EnemyProperty enemyProperty)
    {
        SpriteFlicker flicker = enemyProperty.m_enemySprite.GetComponent<SpriteFlicker> ();
        if (flicker == null)
        {
            Debug.LogErrorFormat ("The {0} enemy doesn't have a SpriteFlicker component!", enemyProperty.gameObject.name);
        }

        StopAllCoroutines ();
        StartCoroutine (Flickering (flicker));
    }

    private IEnumerator Flickering (SpriteFlicker flicker)
    {
        flicker.m_bindFactor = m_flickerBindFactor;
        flicker.m_flickerColor = m_flickerColor;

        float flickTime = m_flickerTime;
        float timer = 0f;
        while (timer < flickTime)
        {
            timer += JITimer.Instance.DeltTime;
            yield return null;
        }

        flicker.m_bindFactor = 0;
    }
}