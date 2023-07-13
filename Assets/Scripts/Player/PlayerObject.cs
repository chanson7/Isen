using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerObject : NetworkBehaviour
{
    public Role role;

    NetworkAnimator netAnimator;
    [SerializeField] Animator heroModel;
    [SerializeField] Animator mannequinModel;
    [SerializeField] CinemachineVirtualCamera firstPersonVcam;
    [SerializeField] CinemachineVirtualCamera thirdPersonVcam;
    [SerializeField] Transform cameraFollow;

    void Awake()
    {

        netAnimator = gameObject.GetComponent<NetworkAnimator>();

        switch (role)
        {
            case Role.Hero:
                heroModel = Instantiate(heroModel, this.transform);
                netAnimator.animator = heroModel;
                break;
            case Role.Mannequin:
                Instantiate(mannequinModel, this.transform);
                netAnimator.animator = mannequinModel;
                break;
        }
    }

    public override void OnStartLocalPlayer()
    {
        this.GetComponent<PlayerInput>().enabled = true;

        switch (role)
        {
            case Role.Hero:
                LocalInitHero();
                break;
            case Role.Mannequin:
                LocalInitMannequin();
                break;
        }
    }

    void LocalInitHero()
    {
        this.gameObject.AddComponent(typeof(FirstPersonCharacterController));

        heroModel.gameObject.SetActive(false);

        firstPersonVcam = Instantiate(firstPersonVcam);
        firstPersonVcam.Follow = cameraFollow;
    }

    void LocalInitMannequin()
    {
        this.gameObject.AddComponent(typeof(ThirdPersonCharacterController));

        thirdPersonVcam = Instantiate(thirdPersonVcam);
        thirdPersonVcam.Follow = cameraFollow;
    }

}

public enum Role
{
    Hero,
    Mannequin
}
