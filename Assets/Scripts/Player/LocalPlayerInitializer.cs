using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class LocalPlayerInitializer : NetworkBehaviour
{

    [SerializeField] ScriptableEvent localPlayerStartedEvent;

    public override void OnStartLocalPlayer()
    {
        this.GetComponent<PlayerInput>().enabled = true;
        this.GetComponent<PlayerInterface>().enabled = true;

        //this event lets other game objects in the scene react to the local player starting.
        //example: the cinemachine camera sets the local player as its lookAt target
        localPlayerStartedEvent.Raise();
    }
}
