using UnityEngine;
using Mirror;

public class TickManager : NetworkBehaviour
{

    //The amount of time between ticks on the server in milliseconds
    [SyncVar] float serverTickMs;
    float timer;
    [SerializeField] ScriptableEvent serverTickEvent;

    // Start is called before the first frame update
    public override void OnStartServer()
    {
        serverTickMs = 1f / MirkwoodNetworkManager.singleton.serverTickRate;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        while (timer >= serverTickMs)
        {
            timer -= serverTickMs;
            serverTickEvent.Raise();
        }
    }

}
