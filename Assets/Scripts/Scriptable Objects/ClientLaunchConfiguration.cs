using UnityEngine;

[CreateAssetMenu(fileName = "ClientLaunchConfiguration", menuName = "ScriptableObjects/ClientLaunchConfiguration", order = 1)]
public class ClientLaunchConfiguration : ScriptableObject
{

    public bool disableServerAuth = false;
    public bool enableInGameDebugConsole = false;
    public bool enableFpsStats = false;


}
