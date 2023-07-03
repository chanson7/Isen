using Mirror;
using UnityEngine;

[RequireComponent(typeof(PredictedPlayerTransform))]
public abstract class PredictedStateProcessor : NetworkBehaviour
{

    public PredictedPlayerTransform predictedPlayerTransform;

    public virtual void Start()
    {
        predictedPlayerTransform = gameObject.GetComponent<PredictedPlayerTransform>();

        predictedPlayerTransform.RegisterPlayerTickProcessor(this);
    }

    public abstract StatePayload ProcessTick(StatePayload statePayload, InputPayload inputPayload); //runs on the local client and the server

}

public interface IPredictedInputProcessor
{
    public InputPayload GatherInput(InputPayload inputPayload);
}

public interface IPredictedAction
{
    void OnEndOrInterrupt(); //an action ends after actionTime or upon interruption
}

public interface IPredictedInterrupt
{
    void OnEnd();
}

public struct InputPayload
{
    public InputPayload(int tick)
    {
        Tick = tick;
        MoveDirection = Vector3.zero;
        LookAtDirection = Vector3.zero;
    }

    public int Tick;
    public Vector3 MoveDirection;
    public Vector3 LookAtDirection;
}

public struct StatePayload
{
    public StatePayload(int tick)
    {
        Tick = tick;
        Position = Vector3.zero;
        Rotation = Quaternion.identity;
        CurrentVelocity = Vector3.zero;
    }

    public int Tick;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 CurrentVelocity;
}