using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using Cinemachine;

public class HeroPlayerObject : NetworkBehaviour
{
    [SerializeField] GameObject heroModel;
    [SerializeField] CinemachineVirtualCamera firstPersonVcam;
    [SerializeField] Transform cameraFollow;

    public override void OnStartLocalPlayer()
    {
        this.GetComponent<PlayerInput>().enabled = true;

        heroModel.gameObject.SetActive(false);

        firstPersonVcam = Instantiate(firstPersonVcam);
        firstPersonVcam.Follow = cameraFollow;
    }

}