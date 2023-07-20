using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FIMSpace.Generating;
using Mirror;

public class NetworkMapGenerator : NetworkBehaviour
{
    BuildPlannerExecutor buildPlannerExecutor;
    [SyncVar(hook = nameof(GenerateMap))] int randomSeed;
    const int SEED_RANGE = 9999;

    private void Awake()
    {
        buildPlannerExecutor = this.GetComponent<BuildPlannerExecutor>();
    }

    public override void OnStartServer()
    {
        randomSeed = Random.Range(-SEED_RANGE, SEED_RANGE);
    }

    void GenerateMap(int oldSeed, int newSeed)
    {
        Debug.Log($"Generating map with seed: {newSeed}");

        buildPlannerExecutor.Seed = newSeed;
        buildPlannerExecutor.Generate();
    }
}
