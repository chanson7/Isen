using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using Cinemachine;

public class MannequinPlayerObject : NetworkBehaviour
{
    [SerializeField] CinemachineVirtualCamera thirdPersonVcam;
    [SerializeField] Transform cameraPivot;

    public override void OnStartLocalPlayer()
    {
        this.GetComponent<PlayerInput>().enabled = true;

        thirdPersonVcam = Instantiate(thirdPersonVcam);
        thirdPersonVcam.Follow = cameraPivot;
        thirdPersonVcam.LookAt = cameraPivot;
    }

}