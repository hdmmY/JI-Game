using System;
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

    private delegate bool GetConditionResultDelegate(bool isLeft);
    private delegate bool GetConditionFinalResultDelegate();

    private delegate float EaseMethodDelegate(float start, float end, float value);

    private delegate void UpdatePropDelegate();

    // It is used to get the arbitrary emit property
    private GetEmitPropertyFloatDelegate GetPropFunc_Cond_Float_Left;
    private GetEmitPropertyIntDelegate GetPropFunc_Cond_Int_Left;
    private GetEmitPropertyFloatDelegate GetPropFunc_Cond_Float_Right;
    private GetEmitPropertyIntDelegate GetPropFunc_Cond_Int_Right;
    private GetEmitPropertyFloatDelegate GetPropFunc_Resu_Float;
    private GetEmitPropertyIntDelegate GetPropFunc_Resu_Int;

    // It is used to change arbitrary emit property
    private ChangePropertyFloatDelegate ChangePropFunc_Float;
    private ChangePropertyIntDelegate ChangePropFunc_Int;

    // It is used to determine whether the condition is true
    private GetConditionResultDelegate GetConditionResultFunc_Left;
    private GetConditionResultDelegate GetConditionResultFunc_Right;
    private GetConditionFinalResultDelegate GetConditionResultFunc_Final;

    // It is used to represent the ease method
    private EaseMethodDelegate EaseFunc;

    private UpdatePropDelegate UpdatePropertyMethod;

    #endregion

    #region variables
    private Enemy_Emit_Property _property;

    // condition
    public ConditionVariableType m_ConditionVariable_Left;
    public OperatorType m_Operator_Left;
    public int m_ConditioinTargetInt_Left;
    public float m_ConditioinTargetFloat_Left;
    public bool m_ConditionResult_Left;

    public ConditionVariableType m_ConditionVariable_Right;
    public OperatorType m_Operator_Right;
    public int m_ConditioinTargetInt_Right;
    public float m_ConditioinTargetFloat_Right;
    public bool m_ConditionResult_Right;

    public LogicOperatorType m_LogicOperator;
    public bool m_ConditionResult_Final;


    // result
    public ResultVariableType m_ResuVariableType;
    public float m_TargetValue_Float;
    public int m_TargetValue_Int;
    public int m_ChangeTime;
    public EaseMethodType m_EaseType;
    public ChangeType m_ChangeMethod;


    // value type
    public ValueType _CondValueType_Left;
    public ValueType _CondValueType_Right;
    public ValueType _ResuValueType;


    // internal 
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

    public enum LogicOperatorType
    {
        AND,
        OR
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
        m_ConditionResult_Final = GetConditionResultFunc_Final();

        if (m_ConditionResult_Final)
        {
            UpdatePercentage(Time.deltaTime);
            UpdatePropertyMethod();
        }

    }

    public void Init()
    {
        // set  left GetPropFunc_Cond_Int || GetPropFunc_Cond_Float variable
        switch (m_ConditionVariable_Left)
        {
            case ConditionVariableType.startTime:
                _CondValueType_Left = ValueType.Float;
                GetPropFunc_Cond_Float_Left = GetRunningTime;
                break;
            case ConditionVariableType.angleRange:
                _CondValueType_Left = ValueType.Int;
                GetPropFunc_Cond_Int_Left = _property.GetEmitAngleRange;
                break;
            case ConditionVariableType.interval:
                _CondValueType_Left = ValueType.Float;
                GetPropFunc_Cond_Float_Left = _property.GetEmitInterval;
                break;
            case ConditionVariableType.lineAngleOffset:
                _CondValueType_Left = ValueType.Int;
                GetPropFunc_Cond_Int_Left = _property.GetEmitLineAngleOffset;
                break;
            case ConditionVariableType.lineNumber:
                _CondValueType_Left = ValueType.Int;
                GetPropFunc_Cond_Int_Left = _property.GetEmitLineNumber;
                break;
            case ConditionVariableType.pointAngleOffset:
                _CondValueType_Left = ValueType.Int;
                GetPropFunc_Cond_Int_Left = _property.GetEmitPointAngleOffset;
                break;
            case ConditionVariableType.radius:
                _CondValueType_Left = ValueType.Float;
                GetPropFunc_Cond_Float_Left = _property.GetEmitRadius;
                break;
        }

        // set  right GetPropFunc_Cond_Int || GetPropFunc_Cond_Float variable
        switch (m_ConditionVariable_Right)
        {
            case ConditionVariableType.startTime:
                _CondValueType_Right = ValueType.Float;
                GetPropFunc_Cond_Float_Right = GetRunningTime;
                break;
            case ConditionVariableType.angleRange:
                _CondValueType_Right = ValueType.Int;
                GetPropFunc_Cond_Int_Right = _property.GetEmitAngleRange;
                break;
            case ConditionVariableType.interval:
                _CondValueType_Right = ValueType.Float;
                GetPropFunc_Cond_Float_Right = _property.GetEmitInterval;
                break;
            case ConditionVariableType.lineAngleOffset:
                _CondValueType_Right = ValueType.Int;
                GetPropFunc_Cond_Int_Right = _property.GetEmitLineAngleOffset;
                break;
            case ConditionVariableType.lineNumber:
                _CondValueType_Right = ValueType.Int;
                GetPropFunc_Cond_Int_Right = _property.GetEmitLineNumber;
                break;
            case ConditionVariableType.pointAngleOffset:
                _CondValueType_Right = ValueType.Int;
                GetPropFunc_Cond_Int_Right = _property.GetEmitPointAngleOffset;
                break;
            case ConditionVariableType.radius:
                _CondValueType_Right = ValueType.Float;
                GetPropFunc_Cond_Float_Right = _property.GetEmitRadius;
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


        // set the left GetConditionResultFunc variable
        switch (m_Operator_Left)
        {
            case OperatorType.equal:
                GetConditionResultFunc_Left = IsEqual;
                break;
            case OperatorType.greater:
                GetConditionResultFunc_Left = IsGreater;
                break;
            case OperatorType.greaterEqual:
                GetConditionResultFunc_Left = IsGreaterEqual;
                break;
            case OperatorType.less:
                GetConditionResultFunc_Left = IsLess;
                break;
            case OperatorType.lessEqual:
                GetConditionResultFunc_Left = IsLessEqual;
                break;
        }

        // set the right GetConditionResultFunc variable
        switch (m_Operator_Right)
        {
            case OperatorType.equal:
                GetConditionResultFunc_Right = IsEqual;
                break;
            case OperatorType.greater:
                GetConditionResultFunc_Right = IsGreater;
                break;
            case OperatorType.greaterEqual:
                GetConditionResultFunc_Right = IsGreaterEqual;
                break;
            case OperatorType.less:
                GetConditionResultFunc_Right = IsLess;
                break;
            case OperatorType.lessEqual:
                GetConditionResultFunc_Right = IsLessEqual;
                break;
        }

        // set the final GetConditionResultFunc variable
        switch (m_LogicOperator)
        {
            case LogicOperatorType.AND:
                GetConditionResultFunc_Final = IsAndTrue;
                break;
            case LogicOperatorType.OR:
                GetConditionResultFunc_Final = IsOrTrue;
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
        GetPropFunc_Cond_Float_Left = null;
        GetPropFunc_Cond_Int_Left = null;
        GetPropFunc_Cond_Float_Right = null;
        GetPropFunc_Cond_Int_Right = null;

        GetPropFunc_Resu_Float = null;
        GetPropFunc_Resu_Int = null;

        ChangePropFunc_Float = null;
        ChangePropFunc_Int = null;

        GetConditionResultFunc_Left = null;
        GetConditionResultFunc_Right = null;
        GetConditionResultFunc_Final = null;

    }


    private void UpdatePercentage(float deltTime)
    {
        _timeSinceEvetStart += deltTime;

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
        if (start > end)
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
    private bool IsEqual(bool isLeft)
    {
        if(isLeft)
        {
            switch (_CondValueType_Left)
            {
                case ValueType.Float:
                    return Mathf.Approximately(GetPropFunc_Cond_Float_Left(), m_ConditioinTargetFloat_Left);
                case ValueType.Int:
                    return GetPropFunc_Cond_Int_Left() == m_ConditioinTargetInt_Left;
            }
        }

        switch (_CondValueType_Right)
        {
            case ValueType.Float:
                return Mathf.Approximately(GetPropFunc_Cond_Float_Right(), m_ConditioinTargetFloat_Right);
            case ValueType.Int:
                return GetPropFunc_Cond_Int_Right() == m_ConditioinTargetInt_Right;
        }

        return false;
    }

    private bool IsGreater(bool isLeft)
    {
        if(isLeft)
        {
            switch (_CondValueType_Left)
            {
                case ValueType.Float:
                    return GetPropFunc_Cond_Float_Left() > m_ConditioinTargetFloat_Left;
                case ValueType.Int:
                    return GetPropFunc_Cond_Int_Left() > m_ConditioinTargetInt_Left;
            }
        }

        switch (_CondValueType_Right)
        {
            case ValueType.Float:
                return GetPropFunc_Cond_Float_Right() > m_ConditioinTargetFloat_Right;
            case ValueType.Int:
                return GetPropFunc_Cond_Int_Right() > m_ConditioinTargetInt_Right;
        }

        return false;
    }

    private bool IsLess(bool isLeft)
    {
        if (isLeft)
        {
            switch (_CondValueType_Left)
            {
                case ValueType.Float:
                    return GetPropFunc_Cond_Float_Left() < m_ConditioinTargetFloat_Left;
                case ValueType.Int:
                    return GetPropFunc_Cond_Int_Left() < m_ConditioinTargetInt_Left;
            }
        }

        switch (_CondValueType_Right)
        {
            case ValueType.Float:
                return GetPropFunc_Cond_Float_Right() < m_ConditioinTargetFloat_Right;
            case ValueType.Int:
                return GetPropFunc_Cond_Int_Right() < m_ConditioinTargetInt_Right;
        }

        return false;
    }

    private bool IsLessEqual(bool isLeft)
    {
        if(isLeft)
        {
            return IsLess(true) || IsEqual(true);
        }

        return IsLess(false) || IsEqual(false);
    }

    private bool IsGreaterEqual(bool isLeft)
    {
        if (isLeft)
        {
            return IsGreater(true) || IsEqual(true);
        }

        return IsGreater(false) || IsEqual(false);
    }


    private bool IsAndTrue()
    {
        return GetConditionResultFunc_Left(true) && GetConditionResultFunc_Right(false);
    }

    public bool IsOrTrue()
    {
        return GetConditionResultFunc_Left(true) || GetConditionResultFunc_Right(false);
    }

    


    #endregion
}