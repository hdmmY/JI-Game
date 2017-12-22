using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(EnemyProperty))]
public class EnemyPropertyEditor : Editor
{
    EnemyProperty tarScript;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        tarScript = (EnemyProperty)target;

        tarScript.m_enemyBulletTag = EditorGUILayout.TagField("Enemy Bullet Tag", tarScript.m_enemyBulletTag);
        tarScript.m_playerBulletTag = EditorGUILayout.TagField("Player Bullet Tag", tarScript.m_playerBulletTag);

        EditorGUILayout.Space();

        EditorGUILayout.PrefixLabel("Reference");
        tarScript.m_enemySprite = EditorGUILayout.ObjectField("Enemy Sprite", tarScript.m_enemySprite, typeof(SpriteRenderer), true) as SpriteRenderer;
    }

}
