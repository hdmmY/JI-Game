using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Enemy_Property))]
public class BaseAIController : MonoBehaviour
{
    // All AI status
    public List<BaseEnemyState> m_totalEnemyAIStates;

    // All AI params. Like Unity animation params. 
    [HideInInspector] public List<AIParam> m_totalParams;

    [SerializeField] protected BaseEnemyState _currentEnemyAI;

    protected Enemy_Property _enemyProperty;

    protected virtual void Start()
    {
        _enemyProperty = GetComponent<Enemy_Property>();
    }

    protected virtual void Update()
    {
        if (_currentEnemyAI != null)
        {
            _currentEnemyAI.UpdateState(_enemyProperty);
        }
    }


    /// <summary>
    ///  Get a float param value
    /// </summary>
    /// <returns></returns>
    public float GetFloat(string name)
    {
        foreach (var param in m_totalParams)
        {
            if (param.m_name == name)
            {
                if (param.ValueType == AIParamerValueType.Float)
                {
                    return param.m_fValue;
                }
            }
        }

        return 0;
    }

    /// <summary>
    /// Get a int param value
    /// </summary>
    /// <returns></returns>
    public int GetInt(string name)
    {
        foreach (var param in m_totalParams)
        {
            if (param.m_name == name)
            {
                if (param.ValueType == AIParamerValueType.Int)
                {
                    return param.m_iValue;
                }
            }
        }

        return 0;
    }


    /// <summary>
    ///  Get a boolean param value
    /// </summary>
    /// <returns></returns>
    public bool GetBool(string name)
    {
        foreach (var param in m_totalParams)
        {
            if (param.m_name == name)
            {
                if (param.ValueType == AIParamerValueType.Boolean)
                {
                    return param.m_bValue;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Set a int param value
    /// </summary>
    /// <returns></returns>
    public void SetInt(string name, int value)
    {
        foreach (var param in m_totalParams)
        {
            if (param.m_name == name)
            {
                if (param.ValueType == AIParamerValueType.Int)
                {
                    param.m_iValue = value;
                }
            }
        }
    }



    /// <summary>
    /// Set a float param value
    /// </summary>
    /// <returns></returns>
    public void SetFloat(string name, float value)
    {
        foreach (var param in m_totalParams)
        {
            if (param.m_name == name)
            {
                if (param.ValueType == AIParamerValueType.Float)
                {
                    param.m_fValue = value;
                }
            }
        }
    }


    /// <summary>
    /// Set a bool param value
    /// </summary>
    /// <returns></returns>
    public void SetBool(string name, bool value)
    {
        foreach (var param in m_totalParams)
        {
            if (param.m_name == name)
            {
                if (param.ValueType == AIParamerValueType.Boolean)
                {
                    param.m_bValue = value;
                }
            }
        }
    }


    /// <summary>
    /// Identify the state by its gameobject's name
    /// </summary>
    /// <returns></returns>
    public BaseEnemyState GetState(string name)
    {
        foreach (var state in m_totalEnemyAIStates)
        {
            if (state.gameObject.name == name)
            {
                return state;
            }
        }

        return null;
    }

}
