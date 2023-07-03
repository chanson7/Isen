using UnityEngine;
using Cinemachine;
using Mirror;

public class FollowTargetController : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera vcam;

    public void SetLocalPlayerFollowTarget()
    {
        vcam.Follow = NetworkClient.localPlayer.transform;
    }
}
