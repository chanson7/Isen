using WebSocketSharp;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

using SteamAuthAPI = HeathenEngineering.SteamworksIntegration.API.Authentication;

public class WebsocketMessage //todo should this just be a struct maybe
{
    public string action { get; set; }
    public Dictionary<string, object> data { get; set; }

    public WebsocketMessage(string action, Dictionary<string, object> data)
    {
        this.action = action;
        this.data = data;
    }

}

//Manages the websocket connection required for matchmaking.
//Updates the player Session instance, then disables itself.
public class MatchmakingClient : MonoBehaviour
{

    WebSocket websocketClient;
    [SerializeField] PlayerSession playerSession;
    [SerializeField] ScriptableEvent MatchRequested;
    private void OnEnable()
    {
        MatchRequested.Raise();
        StartMatchmaking();
    }

    private void StartMatchmaking()
    {

        Debug.Log("..Starting Matchmaking");

        SteamAuthAPI.GetAuthSessionTicket((ticket, IOError) =>
       {
           if (!IOError)
           {
               Debug.Log($"..Ticket data received");
               string matchmakingUrl = BuildMatchmakingUrl(ticket.Data);

               InitializeWebSocketClient(matchmakingUrl);
           }
           else
               Debug.Log($"..Couldn't get the Steam auth ticket :( {IOError.ToString()}");
       });

    }

    string BuildMatchmakingUrl(byte[] ticketData)
    {

        string ticketDataString = System.BitConverter.ToString(ticketData, 0, ticketData.Length).Replace("-", string.Empty);

        UriBuilder uriBuilder = new UriBuilder
        {
            Scheme = "wss",
            Host = $"{Utils.Constants.API_GATEWAY_WEBSOCKET_ENDPOINT}{Utils.Constants.STAGE}",
            Query = $"ticketData={ticketDataString}"
        };

        return uriBuilder.Uri.AbsoluteUri;
    }

    private void InitializeWebSocketClient(string url)
    {
        websocketClient = new WebSocket(url);
        websocketClient.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;

        websocketClient.OnOpen += OnOpen;
        websocketClient.OnClose += OnClose;
        websocketClient.OnError += OnError;
        websocketClient.OnMessage += OnMessage;

        websocketClient.ConnectAsync();
    }

    public void SendMatchmakingRequest()
    {
        if (websocketClient.IsAlive)
        {
            WebsocketMessage message = new WebsocketMessage("findManhuntMatch", null);
            Debug.Log($"..Sending matchmaking request: {JsonConvert.SerializeObject(message)}");
            websocketClient.Send(JsonConvert.SerializeObject(message));
        }
        else
        {
            Debug.Log("..Could not connect to matchmaking server");
        }
    }

    public void CancelMatchmaking()
    {
        if (websocketClient != null && websocketClient.IsAlive)
        {
            Debug.Log($"..Closing websocket connection to matchmaking server");
            websocketClient.Close();
        }
    }

    void OnOpen(object sender, EventArgs e)
    {
        Debug.Log($"..Websocket Connection opened, sending matchmaking request");
        SendMatchmakingRequest();
    }

    void OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log($"..Websocket Connection closed! {e.Reason}");

        UnityMainThreadDispatcher.Instance().Enqueue(() => this.gameObject.SetActive(false));
    }

    void OnError(object sender, ErrorEventArgs e)
    {
        Debug.Log($"..Websocket Error :( {e.Exception.Message}");
    }

    void OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log($"..Message received: \n{e.Data}");
        dynamic message = JObject.Parse(e.Data);

        switch (Convert.ToString(message.Action))
        {
            case "ConnectPlayerSession":
                playerSession.UpdatePlayerSession(message.Data);
                break;
            default:
                Debug.Log($"..Not sure how to handle message action: {message.Action}");
                break;
        }

    }

    private void CloseWebsocketConnection(WebSocket websocketClient)
    {
        if (websocketClient != null)
            websocketClient.CloseAsync();
    }

    private void OnDestroy()
    {
        CloseWebsocketConnection(websocketClient);
    }

    private void OnDisable()
    {
        CloseWebsocketConnection(websocketClient);
    }

    private void OnApplicationQuit()
    {
        CloseWebsocketConnection(websocketClient);
    }



}
