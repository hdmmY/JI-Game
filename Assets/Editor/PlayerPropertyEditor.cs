using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(PlayerProperty))]
public class PlayerPropertyEditor : Editor
{
    PlayerProperty _targetScript;


    public override void OnInspectorGUI()
    {
        _targetScript = (PlayerProperty)target;

        #region movement
        EditorGUILayout.PrefixLabel("Movement");

        EditorGUI.indentLevel++;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Vertical Speed");
        _targetScript.m_verticalSpeed = EditorGUILayout.FloatField(_targetScript.m_verticalSpeed);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Horizontal Speed");
        _targetScript.m_horizontalSpeed = EditorGUILayout.FloatField(_targetScript.m_horizontalSpeed);
        EditorGUILayout.EndHorizontal();

        EditorGUI.indentLevel--;
        #endregion

        #region shoot

        EditorGUILayout.PrefixLabel("Shoot");

        EditorGUI.indentLevel++;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Shoot Interval");
        _targetScript.m_shootInterval = EditorGUILayout.FloatField(_targetScript.m_shootInterval);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Bullet Speed");
        _targetScript.m_bulletSpeed = EditorGUILayout.FloatField(_targetScript.m_bulletSpeed);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Bullet Damage");
        _targetScript.m_bulletDamage = EditorGUILayout.IntField(_targetScript.m_bulletDamage);
        EditorGUILayout.EndHorizontal();

        EditorGUI.indentLevel--;
        #endregion

        #region state
        EditorGUILayout.PrefixLabel("State");

        EditorGUI.indentLevel++;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Player State");
        _targetScript.m_playerState = (PlayerProperty.PlayerStateType)EditorGUILayout.EnumPopup(_targetScript.m_playerState);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("White Sprite");
        _targetScript.m_WhiteSprite = EditorGUILayout.ObjectField(_targetScript.m_WhiteSprite, typeof(Sprite)) as Sprite;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Black Sprite");
        _targetScript.m_BlackSprite = EditorGUILayout.ObjectField(_targetScript.m_BlackSprite, typeof(Sprite)) as Sprite;
        EditorGUILayout.EndHorizontal();

        EditorGUI.indentLevel--;
        #endregion

        #region layer

        EditorGUILayout.PrefixLabel("Layer");

        EditorGUI.indentLevel++;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Player Bullet Layer");
        _targetScript.m_PlayerBulletLayer = EditorGUILayout.TextField(_targetScript.m_PlayerBulletLayer);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Player Layer");
        _targetScript.m_PlayerLayer = EditorGUILayout.TextField(_targetScript.m_PlayerLayer);
        EditorGUILayout.EndHorizontal();

        EditorGUI.indentLevel--;

        #endregion
    }
}
