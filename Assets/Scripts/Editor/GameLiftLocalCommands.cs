#if UNITY_EDITOR
using UnityEditor;
using System.Diagnostics;
using System;
using Newtonsoft.Json;
using System.IO;

public class GameLiftLocalCommands
{

    [MenuItem("GameLift/Create Local Game Session")]
    public static void CreateLocalGameSession()
    {
        using (Process process = new Process())
        {
            process.StartInfo.Arguments = "/C \"AWS gamelift create-game-session --endpoint-url http://localhost:8081 --maximum-player-session-count 2 --fleet-id 123 > GameSession.txt\"";
            process.StartInfo.FileName = "CMD.exe";
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            UnityEngine.Debug.Log($"..GameSession.txt File Updated");
        }
    }

    // [MenuItem("GameLift/Create Player Session")]
    // public static void CreatePlayerSession()
    // {

    //     string gameSession = JsonConvert.DeserializeObject<string>(File.ReadAllText($@"{Environment.CurrentDirectory}\GameSession.txt")).gameSession;

    //     string text = File.ReadAllText($@"{Environment.CurrentDirectory}\GameSession.txt");

    //     using (Process process = new Process())
    //     {
    //         process.StartInfo.Arguments = $"/C \"AWS gamelift create-player-session --player-id 12345 --endpoint-url http://localhost:8081 --game-session-id {gameSession.string} > PlayerSession.txt\"";
    //         process.StartInfo.FileName = "CMD.exe";
    //         process.StartInfo.CreateNoWindow = true;

    //         process.Start();

    //         UnityEngine.Debug.Log($"..PlayerSession.txt File Updated");
    //     }
    // }
}

#endif