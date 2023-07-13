using Mirror;
using UnityEngine;

public class GameState : NetworkBehaviour
{

    public enum EGameState { Off, Loading, Ready, Pregame, Play, End, Postgame };

    [SyncVar(hook = nameof(GameStateChanged))] public EGameState currentState;

    [SerializeField] ScriptableEvent BeginPregameEvent;
    [SerializeField] ScriptableEvent BeginPlayEvent;
    [SerializeField] ScriptableEvent BeginPostgameEvent;
    [SerializeField] ScriptableEvent BeginEndEvent;

    [Server]
    public void SetState(EGameState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
    }

    void GameStateChanged(EGameState oldState, EGameState newState)
    {
        Debug.Log($"Game State: {oldState} -> {newState}");
        switch (newState)
        {
            case EGameState.Pregame:
                BeginPregameEvent.Raise();
                break;
            case EGameState.Play:
                BeginPlayEvent.Raise();
                break;
            case EGameState.End:
                BeginPostgameEvent.Raise();
                break;
            case EGameState.Postgame:
                BeginEndEvent.Raise();
                break;
            default:
                break;
        }
    }

}
