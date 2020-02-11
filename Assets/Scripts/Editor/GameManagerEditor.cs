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


        EditorGUILayout.PropertyField(serializedObject.FindProperty("maximumLifes"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("brickPrefab"), true);
        string arkanoidLevelsHelpBox = "" +
            "This is a text-based level definition. Each line represents an Arkanoid line. For each line only 10 first chars will be taken into account." +
            "Type \"U\" for unbreakable bricks, \"1\" for one-hit bricks, \"2\" for two-hit bricks and \"3\" for three-hit bricks. Any other char will be interpreted as no brick.";
        EditorList.Show(serializedObject.FindProperty("arkanoidLevels"), EditorListOption.ListLabel | EditorListOption.Buttons, arkanoidLevelsHelpBox);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("vBrickOffset"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("hBrickOffset"), true);

        serializedObject.ApplyModifiedProperties();
    }
}
