using HeathenEngineering.SteamworksIntegration.UI;
using UnityEngine;
using TMPro;
public class LobbyUIController : MonoBehaviour
{

    [SerializeField] TMP_Text statusText;
    [SerializeField] QuickMatchLobbyControl lobbyControl;

    public void UpdateStatusText()
    {
        statusText.SetText($"{lobbyControl.Lobby.Members.Length}/{lobbyControl.Lobby.MaxMembers} Players");
    }

}
