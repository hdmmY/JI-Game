using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DanmakU
{

    [CanEditMultipleObjects]
    [CustomPropertyDrawer (typeof (Range))]
    internal class RangeDrawer : PropertyDrawer
    {

        const float buttonSize = 30f;

        Dictionary<string, bool> _propertyType;

        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
        {
            var min = property.FindPropertyRelative ("_min");
            var max = property.FindPropertyRelative ("_max");

            _propertyType = _propertyType ?? (_propertyType = new Dictionary<string, bool> ());

            bool isRange;
            if (!_propertyType.TryGetValue (property.propertyPath, out isRange))
            {
                isRange = !Mathf.Approximately (min.floatValue, max.floatValue);
                _propertyType.Add (property.propertyPath, isRange);
            }

            _propertyType[property.propertyPath] = DrawRangeEditor (position, property, label, isRange);
        }

        static bool DrawRangeEditor (Rect position, SerializedProperty property, GUIContent label, bool isRange)
        {
            var fieldPosition = position;
            var buttonPosition = position;
            fieldPosition.width -= buttonSize;
            buttonPosition.x += fieldPosition.width;
            buttonPosition.width = buttonSize;

            var min = property.FindPropertyRelative ("_min");
            var max = property.FindPropertyRelative ("_max");

            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            EditorGUI.BeginProperty (position, label, property);
            if (isRange)
            {
                MultiFloatField (fieldPosition, label,
                    new [] { new GUIContent ("-"), new GUIContent ("+") },
                    new [] { min, max });
            }
            else
            {
                var multiEditing = min.hasMultipleDifferentValues || max.hasMultipleDifferentValues;
                EditorGUI.showMixedValue = multiEditing;
                var value = EditorGUI.FloatField (fieldPosition, label, min.floatValue);
                if (!min.hasMultipleDifferentValues)
                {
                    min.floatValue = value;
                    max.floatValue = value;
                }
            }
            EditorGUI.showMixedValue = false;
            if (GUI.Button (buttonPosition, isRange ? "\u2194" : "\u2022"))
            {
                isRange = !isRange;
                if (!isRange)
                {
                    var average = (min.floatValue + max.floatValue) / 2;
                    min.floatValue = average;
                    max.floatValue = average;
                }
            }
            EditorGUI.EndProperty ();
            return isRange;
        }

        static void MultiFloatField (Rect position, GUIContent label, GUIContent[] subLabels, SerializedProperty[] properties)
        {
            int controlId = GUIUtility.GetControlID ("foldout".GetHashCode (), FocusType.Passive, position);
            position = MultiFieldPrefixLabel (position, controlId, label, subLabels.Length);
            position.height = 16f;
            MultiFloatField (position, subLabels, properties);
        }

        static void MultiFloatField (Rect position, GUIContent[] subLabels, SerializedProperty[] properties)
        {
            int length = properties.Length;
            float num = (position.width - (float) (length - 1) * 2f) / (float) length;
            Rect position1 = new Rect (position);
            position1.width = num;
            float labelWidth1 = EditorGUIUtility.labelWidth;
            int indentLevel = EditorGUI.indentLevel;
            EditorGUIUtility.labelWidth = 13f;
            EditorGUI.indentLevel = 0;
            for (int index = 0; index < properties.Length; ++index)
            {
                EditorGUI.PropertyField (position1, properties[index], subLabels[index], false);
                position1.x += num + 2f;
            }
            EditorGUIUtility.labelWidth = labelWidth1;
            EditorGUI.indentLevel = indentLevel;
        }

        static bool LabelHasContent (GUIContent label)
        {
            if (label == null || label.text != string.Empty)
                return true;
            return label.image != null;
        }

        static Rect MultiFieldPrefixLabel (Rect totalPosition, int id, GUIContent label, int columns)
        {
            if (!LabelHasContent (label))
                return EditorGUI.IndentedRect (totalPosition);
            var indent = EditorGUI.indentLevel * 15f;
            if (EditorGUIUtility.wideMode)
            {
                Rect labelPosition = new Rect (totalPosition.x + indent, totalPosition.y, EditorGUIUtility.labelWidth - indent, 16f);
                Rect rect = totalPosition;
                rect.xMin += EditorGUIUtility.labelWidth;
                if (columns > 1)
                {
                    --labelPosition.width;
                    --rect.xMin;
                }
                if (columns == 2)
                {
                    float num = (float) (((double) rect.width - 4.0) / 3.0);
                    rect.xMax -= num + 2f;
                }
                EditorGUI.HandlePrefixLabel (totalPosition, labelPosition, label, id);
                return rect;
            }
            Rect labelPosition1 = new Rect (totalPosition.x + indent, totalPosition.y, totalPosition.width - indent, 16f);
            Rect rect1 = totalPosition;
            rect1.xMin += indent + 15f;
            rect1.yMin += 16f;
            EditorGUI.HandlePrefixLabel (totalPosition, labelPosition1, label, id);
            return rect1;
        }

    }

}