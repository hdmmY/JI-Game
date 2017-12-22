using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyState : MonoBehaviour
{
    protected BaseAIController _AIController;

    // The state is end. UpdateState will be never called after a state is end.
    protected bool _stateEnd;

    /// <summary>
    /// Initialize a state. Call once.
    /// </summary>
    /// <param name="enemyProperty"></param>
    public virtual void Initialize(EnemyProperty enemyProperty)
    {
        if (enemyProperty == null)
        {
            Debug.LogError("Enemy Property for AI is null!\n");
        }

        _AIController = enemyProperty.GetComponent<BaseAIController>();
        if (_AIController == null)
        {
            Debug.LogError("Enemy AI Controller is null!\n");
        }

        _stateEnd = false;
    }


    /// <summary>
    /// Excute every frame to update state.
    /// </summary>
    /// <param name="enemyProperty"></param>
    public virtual void UpdateState(EnemyProperty enemyProperty)
    {
        if (enemyProperty == null)
        {
            Debug.LogError("Enemy Property for AI is null!\n");
        }                       
    }

    /// <summary>
    /// Call to force to end the state.
    /// </summary>
    public virtual void EndState(EnemyProperty enemyProperty)
    {
        _stateEnd = true;

        CallOnStateEnd();
    }




    // Method will called when state end
    public System.Action OnStateEnd;

    protected void CallOnStateEnd()
    {
        if (OnStateEnd != null)
        {
            OnStateEnd();
        }
    }

}
