using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
public class TakeDamagerFlickerForBoss : MonoBehaviour
{
    [Range (0, 1)]
    public float m_flickerTime;

    [Range (0, 1)]
    public float m_flickerBindFactor = 0.75f;
    public Color m_flickerColor;

    private SpriteFlicker _flicker;

    private EnemyProperty _enemy;

    private void OnEnable ()
    {
        _flicker = GetComponent<SpriteFlicker> ();

        _enemy = GetComponentInParent<EnemyProperty> ();
        _enemy.OnDamage += StartFlicker;
    }

    private void OnDisable ()
    {
        _enemy.OnDamage -= StartFlicker;
    }

    private void StartFlicker (EnemyProperty enemyProperty)
    {
        if (_flicker == null)
        {
            Debug.LogErrorFormat ("The {0} enemy doesn't have a SpriteFlicker component!", enemyProperty.gameObject.name);
        }

        StopAllCoroutines ();
        StartCoroutine (Flickering (_flicker));
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