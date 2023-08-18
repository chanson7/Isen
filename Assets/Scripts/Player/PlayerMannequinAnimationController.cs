using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMannequinAnimationController : MannequinAnimationController
{
    void OnMove(InputValue input)
    {
        animator.SetBool("isMoving", input.Get<Vector2>().magnitude > 0 ? true : false);
        animator.SetFloat("right", input.Get<Vector2>().x);
        animator.SetFloat("forward", input.Get<Vector2>().y);
    }
}
