using UnityEngine;
using UnityEngine.InputSystem;

public class MannequinMovementAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    void OnMove(InputValue input)
    {
        animator.SetFloat("Right", input.Get<Vector2>().x);
        animator.SetFloat("Forward", input.Get<Vector2>().y);
    }

}
