using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameManager gm = (GameManager)target;

        if (GUILayout.Button("Spawn Players"))
        {
            gm.SpawnPlayers();
        }
    }
}