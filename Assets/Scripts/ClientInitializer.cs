using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class ClientInitializer : MonoBehaviour
{

    [SerializeField] bool disableServerAuth = false;
    [SerializeField] bool enableFpsStats = false;

    [SerializeField] ClientLaunchConfiguration clientConfig;
    [SerializeField] GameObject inGameDebugConsole;
    [SerializeField] GameObject fpsStats;


    void Awake()
    {

#if UNITY_EDITOR
        clientConfig.disableServerAuth = disableServerAuth;
        clientConfig.enableFpsStats = enableFpsStats;
#endif
        SetConfigurationVariables();
        ApplyInitialConfigs();
    }

    void Start()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void SetConfigurationVariables()
    {
        foreach (string arg in Environment.GetCommandLineArgs())
        {
            switch (arg)
            {
                case "-disableServerAuth":
                    clientConfig.disableServerAuth = true;
                    break;

                case "-localTestClient":
                    clientConfig.disableServerAuth = true;
                    clientConfig.enableInGameDebugConsole = true;
                    clientConfig.enableFpsStats = true;

                    break;

                case "-enableInGameDebugConsole":
                    clientConfig.enableInGameDebugConsole = true;
                    break;

                default:
                    break;
            }
        }
    }

    void ApplyInitialConfigs()
    {
        if (clientConfig.enableInGameDebugConsole)
        {
            Debug.LogWarning("..In Game Debug Console enabled");
            inGameDebugConsole.SetActive(true);
        }

        if (clientConfig.enableFpsStats)
        {
            Debug.LogWarning("..FPS Statistics GUI enabled");
            fpsStats.SetActive(true);
        }
    }

}
