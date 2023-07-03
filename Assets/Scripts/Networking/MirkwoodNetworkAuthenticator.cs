using Mirror;
using UnityEngine;

/*
    Documentation: https://mirror-networking.gitbook.io/docs/components/network-authenticators
    API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkAuthenticator.html
*/

public class MirkwoodNetworkAuthenticator : NetworkAuthenticator
{

    [Tooltip("Server Only")]
    [SerializeField] GameLiftServer gameLiftServer;
    [SerializeField] PlayerSession playerSession;

    #region Messages

    public struct AuthRequestMessage : NetworkMessage
    {
        public string playerSessionId;
    }

    public struct AuthResponseMessage : NetworkMessage
    {
        public bool authSuccess;
    }

    #endregion

    #region Server

    /// <summary>
    /// Called on server from StartServer to initialize the Authenticator
    /// <para>Server message handlers should be registered in this method.</para>
    /// </summary>
    public override void OnStartServer()
    {
        Debug.Log($"..Server started. Registering auth message handlers");
        // register a handler for the authentication request we expect from client
        NetworkServer.RegisterHandler<AuthRequestMessage>(OnAuthRequestMessage, false);
    }

    /// <summary>
    /// Called on server from OnServerAuthenticateInternal when a client needs to authenticate
    /// </summary>
    /// <param name="conn">Connection to client.</param>
    public override void OnServerAuthenticate(NetworkConnectionToClient conn) { }

    /// <summary>
    /// Called on server when the client's AuthRequestMessage arrives
    /// </summary>
    /// <param name="conn">Connection to client.</param>
    /// <param name="msg">The message payload</param>
    public void OnAuthRequestMessage(NetworkConnectionToClient conn, AuthRequestMessage msg)
    {
        Debug.Log($"..Authentication message received from client at {conn.address}");
        AuthResponseMessage authResponseMessage = new AuthResponseMessage();

        if (gameLiftServer.AcceptPlayerSession(msg.playerSessionId).Success)
        {
            authResponseMessage.authSuccess = true;
            gameLiftServer.AddPlayerSession(conn.connectionId, msg.playerSessionId);

            ServerAccept(conn);
            Debug.Log($"..Authenticator ACCEPTS {msg.playerSessionId} connection at {NetworkTime.time}");
        }
        else
        {
            // reject the connection
            authResponseMessage.authSuccess = false;
            ServerReject(conn);
            Debug.Log($"..Authenticator REJECTS {msg.playerSessionId} connection at {NetworkTime.time}");
        }

        conn.Send(authResponseMessage);
    }

    #endregion

    #region Client

    /// <summary>
    /// Called on client from StartClient to initialize the Authenticator
    /// <para>Client message handlers should be registered in this method.</para>
    /// </summary>
    public override void OnStartClient()
    {
        Debug.Log($"..Client started. Registering auth message handlers");
        // register a handler for the authentication response we expect from server
        NetworkClient.RegisterHandler<AuthResponseMessage>(OnAuthResponseMessage, false);
    }

    /// <summary>
    /// Called on client from OnClientAuthenticateInternal when a client needs to authenticate
    /// </summary>
    public override void OnClientAuthenticate()
    {
        Debug.Log($"..Client sending Auth Request Message at {NetworkTime.time}");
        AuthRequestMessage authRequestMessage = new AuthRequestMessage
        {
            playerSessionId = playerSession.playerSessionId
        };

        NetworkClient.Send(authRequestMessage);
    }

    /// <summary>
    /// Called on client when the server's AuthResponseMessage arrives
    /// </summary>
    /// <param name="msg">The message payload</param>
    public void OnAuthResponseMessage(AuthResponseMessage msg)
    {
        Debug.Log($"..Server auth response received at {NetworkTime.time}");

        if (msg.authSuccess)
        {
            Debug.Log($"..Accepting server authentication");
            ClientAccept();
        }
    }

    #endregion
}
