using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ArkanoidLevel))]
public class ArkanoidLevelDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int oldIndentLevel = EditorGUI.indentLevel;
        label = EditorGUI.BeginProperty(position, label, property);
        Rect contentPosition = EditorGUI.PrefixLabel(position, label);
        position.height = 100f;
        EditorGUI.indentLevel += 1;
        contentPosition = EditorGUI.IndentedRect(position);
        contentPosition.y += 18f;
        EditorGUI.indentLevel = 0;
        EditorGUIUtility.labelWidth = 50f;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("levelDefinition"));
        EditorGUI.EndProperty();
        EditorGUI.indentLevel = oldIndentLevel;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 120f;
    }
}