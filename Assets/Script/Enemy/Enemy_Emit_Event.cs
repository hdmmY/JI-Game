using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Enemy_Emit_Property))]
public class Enemy_Emit_Event : MonoBehaviour
{

    #region delegates and events

    private delegate void ChangePropertyFloatDelegate(float changeToValue);
    private delegate void ChangePropertyIntDelegate(int changeToValue);

    private delegate int GetEmitPropertyIntDelegate();
    private delegate float GetEmitPropertyFloatDelegate();

    private delegate bool GetConditionResultDelegate();

    private delegate float EaseMethodDelegate(float start, float end, float value);

    private delegate void UpdatePropDelegate();

    // It is used to get the arbitrary emit property
    private GetEmitPropertyFloatDelegate GetPropFunc_Cond_Float;
    private GetEmitPropertyFloatDelegate GetPropFunc_Resu_Float;
    private GetEmitPropertyIntDelegate GetPropFunc_Cond_Int;
    private GetEmitPropertyIntDelegate GetPropFunc_Resu_Int;

    // It is used to change arbitrary emit property
    private ChangePropertyFloatDelegate ChangePropFunc_Float;
    private ChangePropertyIntDelegate ChangePropFunc_Int;

    // It is used to determine whether the condition is true
    private GetConditionResultDelegate GetConditionResultFunc;

    // It is used to represent the ease method
    private EaseMethodDelegate EaseFunc;

    private UpdatePropDelegate UpdatePropertyMethod;

    #endregion

    #region variables
    private Enemy_Emit_Property _property;

    public ConditionVariableType m_ConditionVariable;
    public OperatorType m_Operator;
    public int m_ConditioinTargetInt;
    public float m_ConditioinTargetFloat;
    public bool m_ConditionResult;


    public ResultVariableType m_ResuVariableType;
    public float m_TargetValue_Float;
    public int m_TargetValue_Int;
    public int m_ChangeTime;
    public EaseMethodType m_EaseType;
    public ChangeType m_ChangeMethod;

    public ValueType _CondValueType;
    public ValueType _ResuValueType;

    private float _initValue_Float;
    private int _initValue_Int;
    private float _curValue_Float;
    private int _curValue_Int;

    private float _percent;
    private float _timeSinceEvetStart;
    private float _runingTime;



    public enum ConditionVariableType
    {
        startTime,
        radius,
        lineNumber,
        pointAngleOffset,
        interval,
        lineAngleOffset,
        angleRange
    };

    public enum OperatorType
    {
        greater,
        greaterEqual,
        equal,
        less,
        lessEqual
    };

    public enum ValueType
    {
        Int,
        Float
    };

    public enum ResultVariableType
    {
        radius,
        lineNumber,
        pointAngleOffset,
        interval,
        lineAngleOffset,
        angleRange
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
        SetInitReference();
    }

    private void OnEnable()
    {
        Init();
    }


    private void Update()
    {
        _runingTime += Time.deltaTime;
        m_ConditionResult = GetConditionResultFunc();

        if (m_ConditionResult)
        {
            UpdatePercentage();
            UpdatePropertyMethod();
        }

    }


    public void Init()
    {
        // set  GetPropFunc_Cond_Int || GetPropFunc_Cond_Float variable
        switch (m_ConditionVariable)
        {
            case ConditionVariableType.startTime:
                _CondValueType = ValueType.Float;
                GetPropFunc_Cond_Float = GetRunningTime;
                break;
            case ConditionVariableType.angleRange:
                _CondValueType = ValueType.Int;
                GetPropFunc_Cond_Int = _property.GetEmitAngleRange;
                break;
            case ConditionVariableType.interval:
                _CondValueType = ValueType.Float;
                GetPropFunc_Cond_Float = _property.GetEmitInterval;
                break;
            case ConditionVariableType.lineAngleOffset:
                _CondValueType = ValueType.Int;
                GetPropFunc_Cond_Int = _property.GetEmitLineAngleOffset;
                break;
            case ConditionVariableType.lineNumber:
                _CondValueType = ValueType.Int;
                GetPropFunc_Cond_Int = _property.GetEmitLineNumber;
                break;
            case ConditionVariableType.pointAngleOffset:
                _CondValueType = ValueType.Int;
                GetPropFunc_Cond_Int = _property.GetEmitPointAngleOffset;
                break;
            case ConditionVariableType.radius:
                _CondValueType = ValueType.Float;
                GetPropFunc_Cond_Float = _property.GetEmitRadius;
                break;
        }


        // set GetPropFunc_Resu_Int || GetPropFunc_Resu_Float variable
        switch (m_ResuVariableType)
        {
            case ResultVariableType.angleRange:
                _ResuValueType = ValueType.Int;
                GetPropFunc_Resu_Int = _property.GetEmitAngleRange;
                ChangePropFunc_Int = _property.ChangeEmitAngleRange;
                break;
            case ResultVariableType.interval:
                _ResuValueType = ValueType.Float;
                GetPropFunc_Resu_Float = _property.GetEmitInterval;
                ChangePropFunc_Float = _property.ChangeEmitInterval;
                break;
            case ResultVariableType.lineAngleOffset:
                _ResuValueType = ValueType.Int;
                GetPropFunc_Resu_Int = _property.GetEmitLineAngleOffset;
                ChangePropFunc_Int = _property.ChangeEmitDirAngleOffset;
                break;
            case ResultVariableType.lineNumber:
                _ResuValueType = ValueType.Int;
                GetPropFunc_Resu_Int = _property.GetEmitLineNumber;
                ChangePropFunc_Int = _property.ChangeEmitLineNumber;
                break;
            case ResultVariableType.pointAngleOffset:
                _ResuValueType = ValueType.Int;
                GetPropFunc_Resu_Int = _property.GetEmitPointAngleOffset;
                ChangePropFunc_Int = _property.ChangePointAngleOffset;
                break;
            case ResultVariableType.radius:
                _ResuValueType = ValueType.Float;
                GetPropFunc_Resu_Float = _property.GetEmitRadius;
                ChangePropFunc_Float = _property.ChangeEmitRadius;
                break;
        }


        // set the GetConditionResultFunc variable
        switch (m_Operator)
        {
            case OperatorType.equal:
                GetConditionResultFunc = IsEqual;
                break;
            case OperatorType.greater:
                GetConditionResultFunc = IsGreater;
                break;
            case OperatorType.greaterEqual:
                GetConditionResultFunc = IsGreaterEqual;
                break;
            case OperatorType.less:
                GetConditionResultFunc = IsLess;
                break;
            case OperatorType.lessEqual:
                GetConditionResultFunc = IsLessEqual;
                break;
        }


        // set _intiValue && _curValue
        switch (_ResuValueType)
        {
            case ValueType.Float:
                _initValue_Float = GetPropFunc_Resu_Float();
                _curValue_Float = _initValue_Float;
                break;
            case ValueType.Int:
                _initValue_Int = GetPropFunc_Resu_Int();
                _curValue_Int = _initValue_Int;
                _initValue_Float = (float)_curValue_Int;
                _curValue_Float = _initValue_Float;
                break;
        }


        // set  ease function
        switch (m_EaseType)
        {
            case EaseMethodType.Linear:
                EaseFunc = LinearEase;
                break;
            case EaseMethodType.Sin:
                EaseFunc = SineEase;
                break;
        }


        // set change method
        switch (m_ChangeMethod)
        {
            case ChangeType.ChangeTo:
                UpdatePropertyMethod = ChangeTo;
                break;
            case ChangeType.Decrease:
                UpdatePropertyMethod = Decrease;
                break;
            case ChangeType.Increase:
                UpdatePropertyMethod = Increase;
                break;
        }
                              
        _percent = 0f;
        _timeSinceEvetStart = 0f;
        _runingTime = 0f;
    }


    private void SetInitReference()
    {
        _property = GetComponent<Enemy_Emit_Property>();


        // reset the event
        GetPropFunc_Cond_Float = null;
        GetPropFunc_Cond_Int = null;
        GetPropFunc_Resu_Float = null;
        GetPropFunc_Resu_Int = null;

        ChangePropFunc_Float = null;
        ChangePropFunc_Int = null;

        GetConditionResultFunc = null;

    }


    private void UpdatePercentage()
    {
        _timeSinceEvetStart += Time.deltaTime;

        _percent = _timeSinceEvetStart / m_ChangeTime;
    }

    private float GetRunningTime()
    {
        return _runingTime;
    }


    #region Change method
    private void Increase()
    {
        if (_percent > 1)
            return;

        switch (_ResuValueType)
        {
            case ValueType.Float:
                _curValue_Float += m_TargetValue_Float * Time.deltaTime;
                ChangePropFunc_Float(_curValue_Float);
                break;
            case ValueType.Int:
                _curValue_Float += m_TargetValue_Int * Time.deltaTime;
                _curValue_Int = (int)_curValue_Float;
                ChangePropFunc_Int(_curValue_Int);
                break;
        }
    }

    private void Decrease()
    {
        if (_percent > 1)
            return;

        switch (_ResuValueType)
        {
            case ValueType.Float:
                _curValue_Float -= m_TargetValue_Float * Time.deltaTime;
                ChangePropFunc_Float(_curValue_Float);
                break;
            case ValueType.Int:
                _curValue_Float -= m_TargetValue_Int * Time.deltaTime;
                _curValue_Int = (int)_curValue_Float;
                ChangePropFunc_Int(_curValue_Int);
                break;
        }
    }

    private void ChangeTo()
    {
        if (_percent > 1)
            return;

        switch (_ResuValueType)
        {
            case ValueType.Float:
                _curValue_Float = EaseFunc(_initValue_Float, m_TargetValue_Float, _percent);
                ChangePropFunc_Float(_curValue_Float);
                break;
            case ValueType.Int:
                _curValue_Int = (int)EaseFunc(_initValue_Float, (float)m_TargetValue_Int, _percent);
                ChangePropFunc_Int(_curValue_Int);
                break;
        }
    }

    #endregion


    #region Ease method
    private float SineEase(float start, float end, float value)
    {
        if(start > end)
        {
            start = start + end;
            end = start - end;
            start = start - end;
            value = 1 - value;
        }   

        end -= start;
        end = end - start;
        return -end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start + start;
    }

    private float LinearEase(float start, float end, float value)
    {
        if (start > end)
        {
            start = start + end;
            end = start - end;
            start = start - end;
            value = 1 - value;
        }

        return Mathf.Lerp(0, end - start, value) + start;
    }
    #endregion


    #region Get condition result method
    private bool IsEqual()
    {
        switch (_CondValueType)
        {
            case ValueType.Float:
                return Mathf.Approximately(GetPropFunc_Cond_Float(), m_ConditioinTargetFloat);
            case ValueType.Int:
                return GetPropFunc_Cond_Int() == m_ConditioinTargetInt;
        }

        return false;
    }

    private bool IsGreater()
    {
        switch (_CondValueType)
        {
            case ValueType.Float:
                return GetPropFunc_Cond_Float() > m_ConditioinTargetFloat;
            case ValueType.Int:
                return GetPropFunc_Cond_Int() > m_ConditioinTargetInt;
        }

        return false;
    }

    private bool IsLess()
    {
        switch (_CondValueType)
        {
            case ValueType.Float:
                return GetPropFunc_Cond_Float() < m_ConditioinTargetFloat;
            case ValueType.Int:
                return GetPropFunc_Cond_Int() < m_ConditioinTargetInt;
        }

        return false;
    }

    private bool IsLessEqual()
    {
        return IsLess() || IsEqual();
    }

    private bool IsGreaterEqual()
    {
        return IsGreater() || IsEqual();
    }
    #endregion
}