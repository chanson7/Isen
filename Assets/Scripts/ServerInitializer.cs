using UnityEngine;
using System;

public class ServerInitializer : MonoBehaviour
{

    [SerializeField] bool disableServerAuth = false;
    [SerializeField] bool autoStartServer = false;



    [SerializeField] MirkwoodNetworkManager networkManager;
    [SerializeField] ServerLaunchConfiguration serverConfig;

    void Awake()
    {
#if UNITY_EDITOR
        serverConfig.disableServerAuth = disableServerAuth;
        serverConfig.autoStartServer = autoStartServer;
#endif

        EvaluateCommandLineArgs();
    }

    void EvaluateCommandLineArgs()
    {
        foreach (string arg in Environment.GetCommandLineArgs())
        {
            switch (arg)
            {
                case "-autoStartServer":
                    serverConfig.autoStartServer = true;
                    break;

                case "-disableServerAuth":
                    serverConfig.disableServerAuth = true;
                    break;

                case "-localTestServer":
                    serverConfig.disableServerAuth = true;
                    serverConfig.autoStartServer = true;
                    break;

                default:
                    break;
            }
        }
    }

}
