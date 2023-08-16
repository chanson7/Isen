using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using Cinemachine;

public class HeroPlayerObject : NetworkBehaviour
{
    [SerializeField] GameObject heroModel;
    [SerializeField] GameObject localPlayerArmsModel;
    [SerializeField] CinemachineVirtualCamera firstPersonVcam;
    [SerializeField] Transform cameraPivot;

    public override void OnStartLocalPlayer()
    {
        GetComponent<PlayerInput>().enabled = true;

        heroModel.SetActive(false);
        //localPlayerArmsModel.SetActive(true);

        firstPersonVcam = Instantiate(firstPersonVcam);
        firstPersonVcam.Follow = cameraPivot;
    }

}