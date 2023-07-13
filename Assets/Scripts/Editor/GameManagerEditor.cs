using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameManager myScript = (GameManager)target;

        if (GUILayout.Button("Spawn Players"))
        {
            myScript.SpawnPlayers();
        }
    }
}