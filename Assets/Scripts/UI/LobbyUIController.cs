using HeathenEngineering.SteamworksIntegration.UI;
using UnityEngine;
using TMPro;
public class LobbyUIController : MonoBehaviour
{

    [SerializeField] TMP_Text statusText;
    [SerializeField] QuickMatchLobbyControl lobbyControl;

    public void UpdateStatusText()
    {
        switch (lobbyControl.WorkingStatus)
        {
            case QuickMatchLobbyControl.Status.Searching:
                statusText.SetText($"Searching...");
                break;
            case QuickMatchLobbyControl.Status.WaitingForStart:
                statusText.SetText($"{lobbyControl.Lobby.Members.Length}/{lobbyControl.Lobby.MaxMembers} Players Found");
                break;
            case QuickMatchLobbyControl.Status.Starting:
                statusText.SetText($"Match Starting!");
                break;
            default:
                break;
        }
    }

}
