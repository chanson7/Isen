using UnityEngine;
using Mirror;
using FIMSpace.Generating;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour
{

    public static GameManager singleton { get; private set; }
    const int SEED_RANGE = 9999;
    [SerializeField] BuildPlannerExecutor buildPlannerExecutor;
    GameState gameState;
    List<Transform> playerSpawnPoints = new List<Transform>();
    [SyncVar(hook = nameof(GenerateMap))] int randomSeed;

    private void Awake()
    {
        if (singleton is not null)
            Destroy(singleton.gameObject);

        singleton = this;
        gameState = this.GetComponent<GameState>();
    }

    public override void OnStartServer()
    {
        gameState.SetState(GameState.EGameState.Loading);

        randomSeed = Random.Range(-SEED_RANGE, SEED_RANGE);
    }

    void GenerateMap(int oldSeed, int newSeed)
    {
        Debug.Log($"Generating Seed: {newSeed}");

        buildPlannerExecutor.Seed = newSeed;
        buildPlannerExecutor.Generate();
    }

    public void OnMapGenerated()
    {
        if (isServer)
            gameState.SetState(GameState.EGameState.Pregame);
    }

    public void RegisterPlayerSpawnPoint(Transform playerSpawnPointTransform)
    {
        playerSpawnPoints.Add(playerSpawnPointTransform);
    }

    [Server] //TODO this should probably be in the network manager
    public void SpawnPlayers()
    {
        foreach (var connection in NetworkServer.connections)
        {
            var randomIndex = Random.Range(0, playerSpawnPoints.Count);
            Transform playerSpawnTransform = playerSpawnPoints[randomIndex];

            playerSpawnPoints.RemoveAt(randomIndex);

            GameObject player = Instantiate(SteamNetworkManager.singleton.playerPrefab, playerSpawnTransform.position, playerSpawnTransform.rotation);
            player.name = $"{SteamNetworkManager.singleton.playerPrefab.name} [connId={connection.Key}]";
            NetworkServer.AddPlayerForConnection(connection.Value, player);
        }
    }

}
