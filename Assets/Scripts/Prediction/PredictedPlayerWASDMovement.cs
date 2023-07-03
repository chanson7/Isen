using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PredictedPlayerWASDMovement : PredictedStateProcessor, IPredictedInputProcessor
{
    public Vector3 movementInput = new Vector3();
    [SerializeField] float movementSpeed;
    [SerializeField] float walkSpeedMultiplier;
    [SerializeField] Animator animator;
    CharacterController characterController;

    static int forwardHash = Animator.StringToHash("Forward");
    static int rightHash = Animator.StringToHash("Right");

    public override void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();

        base.Start();
    }

    void Update()
    {
        if (isLocalPlayer)
            AnimateMovement(characterController.velocity);
        else if (isServer)
            RpcAnimateMovement(characterController.velocity);
    }

    void OnMove(InputValue input)
    {
        movementInput.x = input.Get<Vector2>().x;
        movementInput.z = input.Get<Vector2>().y;
    }

    public InputPayload GatherInput(InputPayload inputPayload)
    {
        inputPayload.MoveDirection = movementInput;

        return inputPayload;
    }

    public override StatePayload ProcessTick(StatePayload statePayload, InputPayload inputPayload)
    {

        Vector3 velocity = statePayload.CurrentVelocity;

        velocity = Vector3.Lerp(velocity, inputPayload.MoveDirection.normalized * movementSpeed, 1f);

        // velocity = Vector3.SmoothDamp(statePayload.CurrentVelocity,
        //                               inputPayload.MoveDirection.normalized * movementSpeed * (inputPayload.IsWalking ? walkSpeedMultiplier : 1f),
        //                               ref velocity,
        //                               smoothMovementTime);

        velocity.y = characterController.isGrounded ? 0f : (statePayload.CurrentVelocity.y + Physics.gravity.y);

        characterController.Move(velocity * (1f / MirkwoodNetworkManager.singleton.serverTickRate));

        statePayload.Position = transform.position;
        statePayload.CurrentVelocity = characterController.velocity;

        return statePayload;
    }

    void AnimateMovement(Vector3 currentVelocity)
    {
        animator.SetFloat(forwardHash, transform.InverseTransformDirection(currentVelocity).z);
        animator.SetFloat(rightHash, transform.InverseTransformDirection(currentVelocity).x);
    }

    [ClientRpcAttribute(includeOwner = false)]
    void RpcAnimateMovement(Vector3 currentVelocity)
    {
        animator.SetFloat(forwardHash, transform.InverseTransformDirection(currentVelocity).z);
        animator.SetFloat(rightHash, transform.InverseTransformDirection(currentVelocity).x);
    }

}
