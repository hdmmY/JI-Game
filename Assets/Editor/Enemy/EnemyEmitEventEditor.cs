using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Enemy_Emit_Event))]
public class EnemyEmitEventEditor : Editor
{
    Enemy_Emit_Event _targetScript;

    bool isShowCondition = true;
    bool isShowResult = true;

    public override void OnInspectorGUI()
    {
        _targetScript = (Enemy_Emit_Event)target;

        isShowCondition = EditorGUILayout.Foldout(isShowCondition, "Condition Setting");
        if (isShowCondition)
        {
            EditorGUI.BeginChangeCheck();

            EditorGUI.indentLevel++;
            ShowConditionSetting();
            EditorGUI.indentLevel--;
            
            if(EditorGUI.EndChangeCheck())
            {
                _targetScript.Init();
            }
        }

        EditorGUILayout.Space();

        isShowResult = EditorGUILayout.Foldout(isShowResult, "Result Setting");
        if (isShowResult)
        {
            EditorGUI.BeginChangeCheck();

            EditorGUI.indentLevel++;
            ShowResultSetting();
            EditorGUI.indentLevel--;

            if(EditorGUI.EndChangeCheck())
            {
                _targetScript.Init();
            }
        }


        Undo.RecordObject(_targetScript, "Change Enemy Emit Event!");

    }


    private void ShowConditionSetting()
    {
        // left : condition variable
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Condition Variable Left");
        _targetScript.m_ConditionVariable_Left = (Enemy_Emit_Event.ConditionVariableType)EditorGUILayout.EnumPopup(_targetScript.m_ConditionVariable_Left);
        EditorGUILayout.EndHorizontal();


        // left : operator 
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Operator");
        _targetScript.m_Operator_Left = (Enemy_Emit_Event.OperatorType)EditorGUILayout.EnumPopup(_targetScript.m_Operator_Left);
        EditorGUILayout.EndHorizontal();

        // left : condition result;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Condition Target Value");
        switch (_targetScript._CondValueType_Left)
        {
            case Enemy_Emit_Event.ValueType.Float:
                _targetScript.m_ConditioinTargetFloat_Left = EditorGUILayout.FloatField(_targetScript.m_ConditioinTargetFloat_Left);
                break;
            case Enemy_Emit_Event.ValueType.Int:
                _targetScript.m_ConditioinTargetInt_Left = EditorGUILayout.IntField(_targetScript.m_ConditioinTargetInt_Left);
                break;
        }
        EditorGUILayout.EndHorizontal();


        // logic operator
        EditorGUI.indentLevel++;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Operator");
        _targetScript.m_LogicOperator = (Enemy_Emit_Event.LogicOperatorType)EditorGUILayout.EnumPopup(_targetScript.m_LogicOperator);
        EditorGUILayout.EndHorizontal();
        EditorGUI.indentLevel--;


        // right : condition variable
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Condition Variable Right");
        _targetScript.m_ConditionVariable_Right = (Enemy_Emit_Event.ConditionVariableType)EditorGUILayout.EnumPopup(_targetScript.m_ConditionVariable_Right);
        EditorGUILayout.EndHorizontal();


        // right : operator 
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Operator");
        _targetScript.m_Operator_Right = (Enemy_Emit_Event.OperatorType)EditorGUILayout.EnumPopup(_targetScript.m_Operator_Right);
        EditorGUILayout.EndHorizontal();

        // right : condition result;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Condition Target Value");
        switch (_targetScript._CondValueType_Right)
        {
            case Enemy_Emit_Event.ValueType.Float:
                _targetScript.m_ConditioinTargetFloat_Right = EditorGUILayout.FloatField(_targetScript.m_ConditioinTargetFloat_Right);
                break;
            case Enemy_Emit_Event.ValueType.Int:
                _targetScript.m_ConditioinTargetInt_Right = EditorGUILayout.IntField(_targetScript.m_ConditioinTargetInt_Right);
                break;
        }
        EditorGUILayout.EndHorizontal();


    }


    private void ShowResultSetting()
    {
        // result variable
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Changed Value Type");
        _targetScript.m_ResuVariableType = (Enemy_Emit_Event.ResultVariableType)EditorGUILayout.EnumPopup(_targetScript.m_ResuVariableType);
        EditorGUILayout.EndHorizontal();


        // change method
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Change Type");
        _targetScript.m_ChangeMethod = (Enemy_Emit_Event.ChangeType)EditorGUILayout.EnumPopup(_targetScript.m_ChangeMethod);
        EditorGUILayout.EndHorizontal();


        // ease type
        if (_targetScript.m_ChangeMethod == Enemy_Emit_Event.ChangeType.ChangeTo)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Ease Type");
            _targetScript.m_EaseType = (Enemy_Emit_Event.EaseMethodType)EditorGUILayout.EnumPopup(_targetScript.m_EaseType);
            EditorGUILayout.EndHorizontal();
        }


        // target value
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Target Value");
        switch (_targetScript._ResuValueType)
        {
            case Enemy_Emit_Event.ValueType.Float:
                _targetScript.m_TargetValue_Float = EditorGUILayout.FloatField(_targetScript.m_TargetValue_Float);
                break;
            case Enemy_Emit_Event.ValueType.Int:
                _targetScript.m_TargetValue_Int = EditorGUILayout.IntField(_targetScript.m_TargetValue_Int);
                break;
        }
        EditorGUILayout.EndHorizontal();        


        // change time
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Change Time");
        _targetScript.m_ChangeTime = EditorGUILayout.IntField(_targetScript.m_ChangeTime);
        EditorGUILayout.EndHorizontal();
    }
}
