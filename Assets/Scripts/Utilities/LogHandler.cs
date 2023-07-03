using UnityEngine;
using System;
using System.IO;

//this class allows for some customization of the logs that get saved.
public class LogHandler : MonoBehaviour
{
    private StreamWriter writer;

    public string CreateLogFile(string logPrefix)
    {
        string logName = $"{logPrefix}Log.txt";
        string path = $"{Directory.GetCurrentDirectory()}\\{logName}";

        Debug.Log($"..Creating Log File at {path}");

        writer = File.CreateText(path);

        Application.logMessageReceived += HandleLog;

        return logName;
    }

    private void HandleLog(string condition, string stackTrace, LogType type)
    {
        var logEntry = string.Format("\n{0} {1} {2} {3}"
            , DateTime.Now, type, condition, stackTrace);
        writer.Write(logEntry);
    }

    void OnDestroy()
    {
        writer.Close();
    }
}