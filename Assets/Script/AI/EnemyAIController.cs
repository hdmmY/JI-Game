using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Enemy_Property))]
public class EnemyAIController : MonoBehaviour
{
    // All AI status
    public List<BaseEnemyState> m_totalEnemyAIStates;

    Enemy_Property _enemyProperty;
    BaseEnemyState _currentEnemyAI;


    private void Start()
    {
        _enemyProperty = GetComponent<Enemy_Property>();

        // First state is m_totalEnemyAIStates[0]
        _currentEnemyAI = m_totalEnemyAIStates[0];
        _currentEnemyAI.Initialize(_enemyProperty);
    }

    private void Update()
    {
        if (_currentEnemyAI != null)
        {
            _currentEnemyAI.UpdateState(_enemyProperty);
        }
    }
}
