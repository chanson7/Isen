using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using Cinemachine;

public class MannequinPlayerObject : NetworkBehaviour
{
    [SerializeField] CinemachineVirtualCamera thirdPersonVcam;
    [SerializeField] Transform cameraFollow;

    public override void OnStartLocalPlayer()
    {
        this.GetComponent<PlayerInput>().enabled = true;

        thirdPersonVcam = Instantiate(thirdPersonVcam);
        thirdPersonVcam.Follow = cameraFollow;
    }

}