using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(BaseAIController), true)]
public class BaseAIControllerEditor : Editor
{
    BaseAIController _target;

    private static bool _showAIParams = false;

    public override void OnInspectorGUI()
    {                                   
        _target = target as BaseAIController;

        EditorGUILayout.Space();   
        ShowAIParams();

        base.OnInspectorGUI();

    }


    private void ShowAIParams()
    {
        if(_target.m_totalParams == null)
        {
            _target.m_totalParams = new List<AIParam>();
        }


        if(_showAIParams = EditorGUILayout.Foldout(_showAIParams, "AI Params"))
        {
            EditorGUI.indentLevel++;

            int paramsCount = EditorGUILayout.IntField("Size", _target.m_totalParams.Count);

            // Add 
            if(paramsCount > _target.m_totalParams.Count)
            {
                for(int i = _target.m_totalParams.Count; i < paramsCount; i++)
                {
                    _target.m_totalParams.Add(new AIParam());
                }
            }

            // Delete
            if(paramsCount < _target.m_totalParams.Count)
            {
                _target.m_totalParams.RemoveRange(paramsCount, _target.m_totalParams.Count - paramsCount);
            }

            // Show 
            for(int i = 0; i < paramsCount; i++)
            {
                EditorGUILayout.PrefixLabel("Element" + i);

                EditorGUI.indentLevel++;
                _target.m_totalParams[i].m_name = EditorGUILayout.TextField("Name", _target.m_totalParams[i].m_name);

                EditorGUILayout.BeginHorizontal();
                _target.m_totalParams[i].ValueType = (AIParamerValueType)EditorGUILayout.EnumPopup(_target.m_totalParams[i].ValueType);
                switch(_target.m_totalParams[i].ValueType)
                {
                    case AIParamerValueType.Boolean:
                        _target.m_totalParams[i].m_bValue = EditorGUILayout.Toggle(_target.m_totalParams[i].m_bValue);
                        break;
                    case AIParamerValueType.Int:
                        _target.m_totalParams[i].m_iValue = EditorGUILayout.IntField(_target.m_totalParams[i].m_iValue);
                        break;
                    case AIParamerValueType.Float:
                        _target.m_totalParams[i].m_fValue = EditorGUILayout.FloatField(_target.m_totalParams[i].m_fValue);
                        break;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel--;
            }                         


            EditorGUI.indentLevel--;
        }

        
    }


}
