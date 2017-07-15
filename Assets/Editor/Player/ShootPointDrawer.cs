using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(ShootPoint))]
public class ShootPointDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //string index = label.text.Remove(0, 7);

        // draw the label
        Rect contentRect = EditorGUI.PrefixLabel(position, new GUIContent("Shoot Point: "));


        // draw the name
        if (position.height > 16f)
        {
            position.height = 16f;
            EditorGUI.indentLevel += 1;
            contentRect = EditorGUI.IndentedRect(position);
            contentRect.y += 18f;
        }
        EditorGUI.indentLevel = 0;
        EditorGUIUtility.labelWidth = 15;
        contentRect.width *= 0.4f;
        EditorGUI.PropertyField(contentRect, property.FindPropertyRelative("name"), new GUIContent("N", "name"));


        // draw transform 
        EditorGUIUtility.labelWidth = 45;
        contentRect.x += contentRect.width;
        contentRect.width *= 1.5f;
        EditorGUI.PropertyField(contentRect, property.FindPropertyRelative("transform"), new GUIContent(" Trans", "Shoot Point Transform"));
                 
    }



    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return Screen.width < 333 ? (16f + 18f) : 16f;
    }



}
