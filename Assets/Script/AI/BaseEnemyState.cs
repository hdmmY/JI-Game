using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyState : MonoBehaviour
{
    protected EnemyAIController _AIController;
             
    public virtual void Initialize(Enemy_Property enemyProperty)
    {
        if (enemyProperty == null)
        {
            Debug.LogError("Enemy Property for AI is null!\n");
        }

        _AIController = enemyProperty.GetComponent<EnemyAIController>();
        if(_AIController == null)
        {
            Debug.LogError("Enemy AI Controller is null!\n");
        }
    }             

    public virtual void UpdateState(Enemy_Property enemyProperty)
    {
        if(enemyProperty == null)
        {
            Debug.LogError("Enemy Property for AI is null!\n");
        }
    }     

}
