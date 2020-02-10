using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CapsulePrize))]
public class CapsulePrizeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int oldIndentLevel = EditorGUI.indentLevel;

        label = EditorGUI.BeginProperty(position, label, property);
        Rect contentPosition = EditorGUI.PrefixLabel(position, label);

        if (position.height > 16f)
        {
            position.height = 16f;
            EditorGUI.indentLevel += 1;
            contentPosition = EditorGUI.IndentedRect(position);
            contentPosition.y += 18f;
        }
        contentPosition.width *= 0.7f;
        EditorGUI.indentLevel = 0;
        EditorGUIUtility.labelWidth = 50f;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("capsule"), new GUIContent("Capsule"));
        contentPosition.x += contentPosition.width;
        contentPosition.width /= 2.5f;
        EditorGUIUtility.labelWidth = 45f;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("capsuleWeight"), new GUIContent("Weight"));
        EditorGUI.EndProperty();
        EditorGUI.indentLevel = oldIndentLevel;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return label != GUIContent.none && Screen.width < 333 ? (16f + 18f) : 16f;
    }
}