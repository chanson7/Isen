using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonCharacterController : MonoBehaviour
{

    [Header("Character Controller Settings")]
    public float acceleration;
    public float braking;
    public float maxSpeed;

    Vector3 movementDirection;
    CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void OnMove(InputValue input)
    {
        movementDirection.x = input.Get<Vector2>().x;
        movementDirection.z = input.Get<Vector2>().y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movementValue = Vector3.zero;

        if (movementDirection == Vector3.zero)
        {
            movementValue = Vector3.Lerp(movementValue, Vector3.zero, braking * Time.deltaTime);
        }
        else
        {
            movementValue = Vector3.ClampMagnitude(movementValue + movementDirection * acceleration * Time.deltaTime, maxSpeed);
        }

        if (controller.isGrounded && controller.velocity.y < 0)
            movementValue.y = 0f;

        movementValue.y += Physics.gravity.y * Time.deltaTime;

        controller.Move(transform.TransformDirection(movementValue));
    }
}
