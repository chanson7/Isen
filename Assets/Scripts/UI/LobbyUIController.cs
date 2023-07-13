using HeathenEngineering.SteamworksIntegration;
using UnityEngine;
using Steamworks;
using TMPro;
public class LobbyUIController : MonoBehaviour
{

    LobbyManager lobbyManager;
    [SerializeField] GameObject privateLobbyUI;
    [SerializeField] GameObject quickMatchLobbyUI;
    [SerializeField] TMP_Text statusText;

    private void Awake()
    {
        lobbyManager = this.GetComponent<LobbyManager>();

        lobbyManager.evtEnterSuccess.AddListener(ActivateLobbyUI);
    }

    void ActivateLobbyUI(Lobby lobby)
    {
        switch (lobby.Type)
        {
            case ELobbyType.k_ELobbyTypePrivate:
                privateLobbyUI.SetActive(true);
                break;
            default:
                quickMatchLobbyUI.SetActive(true);
                break;
        }
    }

    public void UpdateStatusText()
    {
        statusText.SetText($"{lobbyManager.Lobby.Members.Length}/{lobbyManager.Lobby.MaxMembers} Players");
    }

    public void TryStartGame()
    {
        switch (lobbyManager.Lobby.Type)
        {
            case ELobbyType.k_ELobbyTypePrivate:
                break;
            case ELobbyType.k_ELobbyTypePublic:
                if (lobbyManager.Lobby.IsOwner && lobbyManager.Lobby.Members.Length == lobbyManager.Lobby.MaxMembers && !SteamNetworkManager.singleton.isNetworkActive)
                    SteamNetworkManager.singleton.StartHost();
                break;
            default:
                break;
        }
    }

}
