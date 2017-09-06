using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Bullet_Property))]
public class Bullet_Event : MonoBehaviour
{

    #region delegate and events
    private delegate void ChangePropertyFloatDelegate(float changeToValue);
    private delegate void ChangePropertyIntDelegate(int changeToValue);
    private delegate void ChangePropertyColorDelegate(Color changeToValue);

    private delegate int GetPropertyIntDelegate();
    private delegate float GetPropertyFloatDelegate();
    private delegate Color GetPropertyColorDelegate();


    



    #endregion


    #region variable

    // condition
    public OperatorType m_Operator_Left;
    public OperatorType m_Operator_Right;

    public LogicOperatorType m_LogicOperator;

    public float m_ConditionTarget_Left;
    public float m_ConditionTarget_Right;

    public bool m_Condition_Final;


    // result
    public ResultVariableType m_ResuVariableType;

    public float m_TargetValue_Float;
    public Color m_TargetValue_Color;
    public int m_TargetValue_Int;

    public EaseMethodType m_EaseType;
    public ChangeType m_ChangeMethod;


    // value type
    public ValueType m_ResultValueType;


    // internal
    private float _initValue;
    private float _curValue;

    private float _percent;
    private float _timeSinceEventStart;
    private float _runningTime;


    public enum OperatorType
    {
        greater,
        greaterEqual,
        equal,
        less,
        lessEqual
    };

    public enum LogicOperatorType
    {
        AND,
        OR
    };

    public enum ValueType
    {
        Int,
        Float,
        Color
    };

    public enum ResultVariableType
    {
        lifetime,
        color,
        transparent,
        speed,
        acceleration,
        accelerationDirection,
        velocityHorizontalFactor,
        velocityVerticalFactor
    }

    public enum EaseMethodType
    {
        Linear,
        Sin,
    };

    public enum ChangeType
    {
        Increase,
        Decrease,
        ChangeTo
    };

    #endregion


    private void Awake()
    {
       // SetInitReference();
    }




}
