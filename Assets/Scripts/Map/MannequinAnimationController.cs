using UnityEngine;
using Mirror;

public class MannequinAnimationController : NetworkBehaviour
{

    [SerializeField] protected Animator animator;
    [SyncVar(hook = nameof(SetPoseIndex))] int poseIndex;

    public override void OnStartServer()
    {
        poseIndex = Random.Range(0, 21); //there are 21 poses right now
    }

    private void SetPoseIndex(int oldPoseIndex, int newPoseIndex)
    {
        animator.SetInteger("poseIndex", newPoseIndex);
    }

}
