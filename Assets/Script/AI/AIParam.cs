using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum AIParamerValueType
{
    Int,
    Float,
    Boolean
};


[System.Serializable]
public class AIParam
{
    public AIParamerValueType ValueType
    {
        get; set;
    }

    public string m_name;

    public float m_fValue;

    public int m_iValue;

    public bool m_bValue;
}

