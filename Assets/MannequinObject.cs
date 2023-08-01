using UnityEngine;

public class MannequinObject : MonoBehaviour
{

    [SerializeField] MapData mapData;
    [SerializeField] Animator animator;
    int poseCount = 4;

    void Start()
    {
        int poseIndex = Mathf.Abs(Random.Range(0, mapData.randomSeed % poseCount));
        // animator.SetInteger("poseIndex", poseIndex);
    }

}
