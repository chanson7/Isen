using UnityEngine;
using Mirror;
using System.Collections;

public class GameManager : NetworkBehaviour
{

    double serverStartTime;
    int playerCount = 0;

    [SerializeField] double playerDisconnectTimeOut = 20;
    [SerializeField] double gameSessionTimeOut = 120;


    public override void OnStartServer()
    {
        serverStartTime = Time.timeAsDouble;

        if (gameSessionTimeOut > 0)
            StartCoroutine(GameSessionTimeOut(gameSessionTimeOut));
    }

    public void OnPlayerJoinedGameSession()
    {
        playerCount++;
    }

    public void OnPlayerLeftGameSession()
    {
        playerCount--;
        CheckForGameTimeOut();
    }

    //do this when a player leaves the game session to see if we should end the game
    [Server]
    void CheckForGameTimeOut()
    {
        if (playerCount < 1 && Time.time - serverStartTime > playerDisconnectTimeOut)
        {
            Debug.Log("..All players have disconnected. Ending the game.");
            Application.Quit();
        }
    }

    IEnumerator GameSessionTimeOut(double gameTimeOut)
    {
        Debug.Log($"..Game Session will time out in {gameTimeOut} seconds");

        yield return new WaitForSeconds((float)gameTimeOut);

        Application.Quit();
    }

}
