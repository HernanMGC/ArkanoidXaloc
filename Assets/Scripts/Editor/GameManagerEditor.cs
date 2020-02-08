using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        EditorList.Show(serializedObject.FindProperty("breakableBrickPics"), EditorListOption.ListLabel | EditorListOption.Buttons);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("unbreakableBrickPic"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("capsuleDropProbability"), true);
        EditorList.Show(serializedObject.FindProperty("capsulePrizes"), EditorListOption.ListLabel | EditorListOption.Buttons);
        serializedObject.ApplyModifiedProperties();
    }
}
