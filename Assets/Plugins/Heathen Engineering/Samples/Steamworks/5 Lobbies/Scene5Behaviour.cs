#if HE_SYSCORE && STEAMWORKS_NET && !DISABLESTEAMWORKS 
using HeathenEngineering.SteamworksIntegration;
using UnityEngine;

namespace HeathenEngineering.DEMO
{
    [System.Obsolete("This script is for demonstration purposes ONLY")]
    public class Scene5Behaviour : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.InputField lobbyIdField;
        [SerializeField]
        private UnityEngine.UI.Text chatText;
        [SerializeField]
        private LobbyManager lobbyManager;

        private void Start()
        {
            lobbyManager.evtCreated.AddListener(HandleLobbyCreated);
        }

        private void HandleLobbyCreated(Lobby arg0)
        {
            var id = arg0.id;
            Debug.Log(id.GetEAccountType() + " " + id.GetEUniverse() + " " + id.GetAccountID());
        }

        public void OpenKnowledgeBaseLobbies()
        {
            Application.OpenURL("https://kb.heathenengineering.com/assets/steamworks/features/multiplayer");
        }

        public void OpenKnowledgeBaseLobbyManager()
        {
            Application.OpenURL("https://kb.heathenengineering.com/assets/steamworks/components/lobby-manager");
        }
        
        public void OpenKnowledgeInspector()
        {
            Application.OpenURL("https://kb.heathenengineering.com/assets/steamworks/quick-start-guide#debugging");
        }

        public void ReportSearchResults(Lobby[] results)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("Found: " + results.Length);
            for (int i = 0; i < results.Length; i++)
            {
                var name = results[i].Name;
                if (string.IsNullOrEmpty(name))
                    name = "UNKNOWN";
                sb.Append("\n " + results[i].id + ", name = " + name);
            }

            Debug.Log(sb.ToString());
        }

        public void JoinLobby()
        {
            lobbyManager.Join(lobbyIdField.text);
        }

        public void HandleChatMessages(LobbyChatMsg message)
        {
            chatText.text += "\n" + message.sender.Name + " said: " + message.Message;
        }

        [ContextMenu("Set Ready")]
        public void SetReady()
        {
            var lobby = lobbyManager.Lobby;
            lobby.IsReady = !lobbyManager.Lobby.IsReady;
        }
    }
}
#endif
