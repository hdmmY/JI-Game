using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Enemy_Property))]
public class EnemyPropertyEditor : Editor
{
    Enemy_Property tarScript;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        tarScript = (Enemy_Property)target;

        tarScript.m_enemyBulletTag = EditorGUILayout.TagField("Enemy Bullet Tag", tarScript.m_enemyBulletTag);
        tarScript.m_playerBulletTag = EditorGUILayout.TagField("Player Bullet Tag", tarScript.m_playerBulletTag);
    }

}
