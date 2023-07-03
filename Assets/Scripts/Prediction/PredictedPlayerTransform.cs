using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PredictedPlayerTransform : NetworkBehaviour
{

    #region private 
    List<PredictedStateProcessor> playerStateProcessors = new List<PredictedStateProcessor>();
    int currentTick;

    #endregion

    #region constants
    const int BUFFER_SIZE = 1024;

    #endregion

    //Processing Components

    #region client only
    private StatePayload[] clientStateBuffer;
    private InputPayload[] clientInputBuffer;
    float acceptablePositionError = 0.001f;
    public StatePayload latestServerState;
    StatePayload lastProcessedState;

    #endregion

    #region server only
    StatePayload[] serverStateBuffer;
    Queue<InputPayload> inputQueue;

    #endregion

    public override void OnStartLocalPlayer()
    {

        clientStateBuffer = new StatePayload[BUFFER_SIZE];
        clientInputBuffer = new InputPayload[BUFFER_SIZE];

        base.OnStartLocalPlayer();
    }

    public override void OnStartServer()
    {
        serverStateBuffer = new StatePayload[BUFFER_SIZE];
        inputQueue = new Queue<InputPayload>();

        base.OnStartServer();
    }

    public void RegisterPlayerTickProcessor(PredictedStateProcessor playerTickProcessor)
    {
        playerStateProcessors.Add(playerTickProcessor);
    }

    public void ServerUpdate()
    {
        if (isLocalPlayer)
            HandleTickOnLocalClient();
        else if (isServer)
            HandleTickOnServer();
        else if (isClient && !isLocalPlayer)
            HandleTickOnOtherClient();

        currentTick++;
    }

    [Client]
    void HandleTickOnLocalClient()
    {
        if (!latestServerState.Equals(default(StatePayload)) && (lastProcessedState.Equals(default(StatePayload)) || !latestServerState.Equals(lastProcessedState)))
            HandleServerReconciliation();

        int bufferIndex = currentTick % BUFFER_SIZE;

        //Add the input payload to the input buffer
        InputPayload inputPayload = new InputPayload(currentTick);

        foreach (IPredictedInputProcessor inputProcessor in playerStateProcessors)
            inputPayload = inputProcessor.GatherInput(inputPayload);

        clientInputBuffer[bufferIndex] = inputPayload;
        clientStateBuffer[bufferIndex] = ProcessInput(inputPayload);

        CmdOnClientInput(inputPayload);
    }

    [Command]
    void CmdOnClientInput(InputPayload inputPayload)
    {
        //TODO the player can just send any frequency of inputs to speed hack. this is bad and should be fixed by somebody
        inputQueue.Enqueue(inputPayload);
    }

    [Server]
    void HandleTickOnServer()
    {
        int bufferIndex = -1;

        //server has some movements to process
        while (inputQueue.Count > 0)
        {
            InputPayload inputPayload = inputQueue.Dequeue();

            bufferIndex = inputPayload.Tick % BUFFER_SIZE;

            StatePayload statePayload = ProcessInput(inputPayload);
            serverStateBuffer[bufferIndex] = statePayload;
        }

        //we processed all of the input!!
        if (bufferIndex != -1)
            RpcOnServerMovementState(serverStateBuffer[bufferIndex]);

    }

    [ClientRpcAttribute]
    void RpcOnServerMovementState(StatePayload statePayload)
    {
        latestServerState = statePayload;
    }

    void HandleTickOnOtherClient()
    {
        //TODO interpolation should probably be done here at some point
        transform.position = latestServerState.Position;
        transform.rotation = latestServerState.Rotation;
    }

    StatePayload ProcessInput(InputPayload input)
    {
        StatePayload processedState = new StatePayload(input.Tick);

        foreach (PredictedStateProcessor stateProcessor in playerStateProcessors)
            processedState = stateProcessor.ProcessTick(processedState, input);

        return processedState;
    }

    [Client]
    void HandleServerReconciliation()
    {
        lastProcessedState = latestServerState;

        int serverStateBufferIndex = latestServerState.Tick % BUFFER_SIZE;

        float positionError = Vector3.Distance(latestServerState.Position, clientStateBuffer[serverStateBufferIndex].Position);

        // this is how to find the difference between the rotations i guess
        Quaternion serverRotation = Quaternion.identity * Quaternion.Inverse(latestServerState.Rotation);
        Quaternion clientRotation = Quaternion.identity * Quaternion.Inverse(clientStateBuffer[serverStateBufferIndex].Rotation);

        Quaternion rotationError = clientRotation * Quaternion.Inverse(serverRotation);
        // Debug.Log($"euler angles magnitude{rotationError.eulerAngles}\nrotation error {rotationError}");

        if (positionError > acceptablePositionError)
        {
            Debug.Log($"..Reconciling for {positionError} position error");

            // Rewind & Replay
            transform.position = latestServerState.Position;

            // Update buffer at index of latest server state
            clientStateBuffer[serverStateBufferIndex] = latestServerState;

            // Now re-simulate the rest of the ticks up to the current tick on the client
            int tickToProcess = latestServerState.Tick + 1;

            while (tickToProcess < currentTick)
            {
                int bufferIndex = tickToProcess % BUFFER_SIZE;

                // Process new movement with reconciled state
                StatePayload statePayload = ProcessInput(clientInputBuffer[bufferIndex]);

                // Update buffer with recalculated state
                clientStateBuffer[bufferIndex] = statePayload;

                tickToProcess++;
            }
        }
    }

}
