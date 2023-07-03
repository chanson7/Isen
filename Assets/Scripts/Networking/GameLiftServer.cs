using UnityEngine;
using Aws.GameLift.Server;
using Aws.GameLift.Server.Model;
using Aws.GameLift;
using System;
using System.Collections.Generic;
using Mirror;

public class GameLiftServer : MonoBehaviour
{

    //associate Mirror server's connection to a client with a GameLift PlayerSession Id
    Dictionary<int, string> playerSessionConnection = new Dictionary<int, string>();

    public void AddPlayerSession(int connectionId, string playerSessionId)
    {
        playerSessionConnection.Add(connectionId, playerSessionId);
        Debug.Log($"..Added Player Session {playerSessionId} with connectionId {connectionId}");
    }

    //Make game server processes go active on Amazon GameLift
    public void StartGameLiftServer(ushort listeningPort, string logPath)
    {

        //InitSDK establishes a local connection with the Amazon GameLift agent to enable 
        //further communication.
        var initSDKOutcome = GameLiftServerAPI.InitSDK();
        if (initSDKOutcome.Success)
        {
            ProcessParameters processParameters = new ProcessParameters(
                (gameSession) => OnStartGameSession(gameSession),
                () =>
                {
                    //OnProcessTerminate callback. GameLift invokes this callback before shutting down 
                    //an instance hosting this game server. It gives this game server a chance to save
                    //its state, communicate with services, etc., before being shut down. 
                    //In this case, we simply tell GameLift we are indeed going to shut down.
                    GameLiftServerAPI.ProcessEnding();
                },
                () => { return OnHealthCheck(); },
                //Active game sessions that are on the same instance must have unique ports.
                listeningPort,
                new LogParameters(new List<string>()
                {
                    //Here, the game server tells GameLift what set of files to upload when the game session ends.
                    //GameLift uploads everything specified here for the developers to fetch later.
                    logPath
                }));

            //Calling ProcessReady tells GameLift this game server is ready to receive incoming game sessions!
            var processReadyOutcome = GameLiftServerAPI.ProcessReady(processParameters);
            if (processReadyOutcome.Success)
            {
                print("ProcessReady success.");
            }
            else
            {
                print("ProcessReady failure : " + processReadyOutcome.Error.ToString());
            }
        }
        else
        {
            print("InitSDK failure : " + initSDKOutcome.Error.ToString());
        }
    }

    public GenericOutcome AcceptPlayerSession(string playerSessionId)
    {
        return GameLiftServerAPI.AcceptPlayerSession(playerSessionId);
    }

    public GenericOutcome RemovePlayerSession(int connectionId)
    {
        try
        {
            string playerSessionToRemove = playerSessionConnection[connectionId];

            playerSessionConnection.Remove(connectionId);

            Debug.Log($"..Removed GameLift Player Session {connectionId}");
            return GameLiftServerAPI.RemovePlayerSession(playerSessionToRemove);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"..Could not remove GameLift player session: {e.Message}");
            return null;
        }
    }

    void OnApplicationQuit()
    {
        //Make sure to call GameLiftServerAPI.ProcessEnding() when the application quits. 
        //This resets the local connection with GameLift's agent.
        GameLiftServerAPI.ProcessEnding();
    }

    void OnStartGameSession(GameSession gameSession)
    {
        MirkwoodNetworkManager.singleton.StartServer();

        if (NetworkServer.active)
        {
            GameLiftServerAPI.ActivateGameSession();

            //names the log file the last 5 characters of the Game Session Id

            Debug.Log($"..New Game Session Activated: \n" +
                      $"..IP Address: {gameSession.IpAddress}\n" +
                      $"..Port: {gameSession.Port}\n" +
                      $"..Game Session ID: {gameSession.GameSessionId}");
        }
    }

    //This is the HealthCheck callback.
    //GameLift invokes this callback every 60 seconds or so.
    //Here, a game server might want to check the health of dependencies and such.
    //Simply return true if healthy, false otherwise.
    //The game server has 60 seconds to respond with its health status. 
    //GameLift will default to 'false' if the game server doesn't respond in time.
    //In this case, we're always healthy!
    bool OnHealthCheck()
    {
        Debug.Log($"..Health Check requested from GameLift");
        return true;
    }


}