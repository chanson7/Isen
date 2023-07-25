using UnityEngine;
using Mirror;
using FIMSpace.Generating;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour
{

    public static GameManager singleton { get; private set; }
    [SerializeField] BuildPlannerExecutor buildPlannerExecutor;
    GameState gameState;
    List<Transform> playerSpawnPoints = new List<Transform>();
    [SerializeField] GameObject heroPlayerPrefab;
    [SerializeField] GameObject mannequinPlayerPrefab;

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

    [Server]
    public void SpawnPlayers()
    {

        if (NetworkServer.connections.Count > 0)
        {
            foreach (var connection in NetworkServer.connections)
            {
                var randomIndex = Random.Range(0, playerSpawnPoints.Count);
                Transform playerSpawnTransform = playerSpawnPoints[randomIndex];

                playerSpawnPoints.RemoveAt(randomIndex);

                GameObject player = Instantiate(mannequinPlayerPrefab, playerSpawnTransform.position, playerSpawnTransform.rotation);

                NetworkServer.ReplacePlayerForConnection(connection.Value, player);
            }
        }
    }

}
