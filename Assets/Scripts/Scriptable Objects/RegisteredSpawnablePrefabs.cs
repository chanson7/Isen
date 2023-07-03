using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RegisteredSpawnablePrefabs", menuName = "ScriptableObjects/RegisteredSpawnablePrefabs", order = 1)]
public class RegisteredSpawnablePrefabs : ScriptableObject
{
    public List<GameObject> spawnablePrefabs;
}
