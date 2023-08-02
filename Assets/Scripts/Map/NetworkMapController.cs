using UnityEngine;
using FIMSpace.Generating;
using Mirror;

public class NetworkMapController : NetworkBehaviour
{
    BuildPlannerExecutor buildPlannerExecutor;
    [SerializeField] ScriptableEvent randomSeedSetEvent;
    [SerializeField] MapData mapData;
    [SyncVar(hook = nameof(OnRandomSeedSet))] int randomSeed;
    const int SEED_RANGE = 9999;

    private void Awake()
    {
        buildPlannerExecutor = this.GetComponent<BuildPlannerExecutor>();
    }

    public override void OnStartServer()
    {
        randomSeed = Random.Range(-SEED_RANGE, SEED_RANGE);
    }

    void OnRandomSeedSet(int oldSeed, int newSeed)
    {
        mapData.randomSeed = newSeed;
        randomSeedSetEvent.Raise();
    }

    public void GenerateMap()
    {
        Debug.Log($"Generating map with seed: {randomSeed}");

        buildPlannerExecutor.Seed = randomSeed;
        buildPlannerExecutor.Generate();
    }
}
