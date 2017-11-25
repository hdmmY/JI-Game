using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpecialShot.GlowwarmShot))]
public class GlowwarmShotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var targetScript = (SpecialShot.GlowwarmShot)target;

        if(targetScript.m_destroyWhenVelocityZero)
        {
            targetScript.m_destroyBulletHealth = EditorGUILayout.IntField("Destroyable Health", targetScript.m_destroyBulletHealth);
        }
    }
}
