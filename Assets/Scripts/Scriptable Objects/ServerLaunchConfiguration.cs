using UnityEngine;

[CreateAssetMenu(fileName = "ServerLaunchConfiguration", menuName = "ScriptableObjects/ServerLaunchConfiguration", order = 1)]
public class ServerLaunchConfiguration : ScriptableObject
{

    public bool disableServerAuth = false;
    public bool autoStartServer = false;

}
