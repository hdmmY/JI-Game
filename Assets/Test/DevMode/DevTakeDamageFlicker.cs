using System.Collections;
using UnityEngine;

public class DevTakeDamageFlicker : MonoBehaviour
{
    [Range (0, 1)]
    public float m_flickerTime = 0.03f;
    
    private EnemyEventMaster _enemyEventMaster;

    private void OnEnable ()
    {
        _enemyEventMaster = GetComponentInParent<EnemyEventMaster>();
        if(_enemyEventMaster == null)
        {
            Debug.LogError("Error!");
        }

        _enemyEventMaster.OnDamage += StartFlicker;
    }

    private void OnDisable ()
    {
        _enemyEventMaster.OnDamage -= StartFlicker;
    }

    private void StartFlicker (EnemyProperty enemyProperty)
    {
        var flicker = enemyProperty.m_enemySprite.GetComponent<DevSetEnemySprite> ();
        
        if (flicker == null)
        {
            Debug.LogErrorFormat ("The {0} enemy doesn't have a DevSetEnemySprite component!",
                enemyProperty.gameObject.name);
        }

        StopAllCoroutines ();
        StartCoroutine (Flickering (flicker));
    }

    private IEnumerator Flickering (DevSetEnemySprite flicker)
    {
        flicker.m_takingDamage = true;

        float timer = 0f;
        while (timer < m_flickerTime)
        {
            timer += JITimer.Instance.DeltTime;
            yield return null;
        }

        flicker.m_takingDamage = false;
    }
}