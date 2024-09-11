#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Attributes
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isArray && property.propertyType == SerializedPropertyType.Generic)
            {
                float totalHeight = EditorGUIUtility.singleLineHeight;

                if (property.isExpanded)
                {
                    totalHeight += EditorGUIUtility.singleLineHeight;
                    for (int i = 0; i < property.arraySize; i++)
                    {
                        SerializedProperty element = property.GetArrayElementAtIndex(i);
                        totalHeight += EditorGUI.GetPropertyHeight(element, GUIContent.none, true) + EditorGUIUtility.standardVerticalSpacing;
                    }
                }

                return totalHeight;
            }
            else
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool wasEnabled = GUI.enabled;
            GUI.enabled = false;

            if (property.isArray && property.propertyType == SerializedPropertyType.Generic)
            {
                Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
                property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

                if (property.isExpanded)
                {
                    EditorGUI.indentLevel++;
                
                    for (int i = 0; i < property.arraySize; i++)
                    {
                        SerializedProperty element = property.GetArrayElementAtIndex(i);
                        Rect elementRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * (i + 1), position.width, EditorGUI.GetPropertyHeight(element, GUIContent.none, true));
                        EditorGUI.PropertyField(elementRect, element, GUIContent.none, true);
                    }
                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            GUI.enabled = wasEnabled;
        }
    }
}
#endif