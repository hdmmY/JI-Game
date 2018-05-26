using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (EnemyProperty))]
public class ShowEnemyHealthUI : MonoBehaviour
{
    public Slider HealthUI;

    private EnemyProperty _enemy;

    private int _initHealth;

    private void OnEnable ()
    {
        _enemy = GetComponent<EnemyProperty> ();
        _initHealth = _enemy.m_health;
    }

    private void Update ()
    {
        HealthUI.value = 1.0f * Mathf.Abs (_enemy.m_health) / _initHealth;
    }

}