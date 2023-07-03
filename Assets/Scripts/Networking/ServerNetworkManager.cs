using System;
using UnityEngine;
using Mirror;

public class ServerNetworkManager : MirkwoodNetworkManager
{
    // Overrides the base singleton so we don't
    // have to cast to this type everywhere.
    public static new ServerNetworkManager singleton { get; private set; }

    [SerializeField] ServerLaunchConfiguration serverLaunchConfiguration;
    [SerializeField] GameLiftServer gameLiftServer;
    [SerializeField] LogHandler logHandler;

    #region Events
    [SerializeField] ScriptableEvent playerJoinedGameSessionEvent;
    [SerializeField] ScriptableEvent playerLeftGameSessionEvent;
    #endregion

    static int minPort = 5000;

    public override void Start()
    {
        singleton = this;

        if (serverLaunchConfiguration.disableServerAuth)
        {
            Debug.LogWarning($"..Authenticator has been disabled.  Make sure it is also disabled on the Client's Network Manager when testing.");
            this.authenticator = null;
        }

        if (serverLaunchConfiguration.autoStartServer)
        {
            Debug.LogWarning($"..Game Server will automatically start.");
            this.autoStartServerBuild = true;
        }

        ((kcp2k.KcpTransport)Transport.activeTransport).Port = UdpUtilities.GetFirstOpenUdpPort(minPort, 500);
        gameLiftServer.StartGameLiftServer(((kcp2k.KcpTransport)Transport.activeTransport).Port, logHandler.CreateLogFile(System.Diagnostics.Process.GetCurrentProcess().Id.ToString()));

        base.Start();
    }

    #region Server System Callbacks

    /// <summary>
    /// Called on the server when a new client connects.
    /// <para>Unity calls this on the Server when a Client connects to the Server. Use an override to tell the NetworkManager what to do when a client connects to the server.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerConnect(NetworkConnectionToClient conn) { }

    /// <summary>
    /// Called on the server when a client is ready.
    /// <para>The default implementation of this function calls NetworkServer.SetClientReady() to continue the network setup process.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);
    }

    /// <summary>
    /// Called on the server when a client adds a new player with ClientScene.AddPlayer.
    /// <para>The default implementation for this function creates a new player object from the playerPrefab.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        playerJoinedGameSessionEvent.Raise();
        base.OnServerAddPlayer(conn);
    }

    /// <summary>
    /// Called on the server when a client disconnects.
    /// <para>This is called on the Server when a Client disconnects from the Server. Use an override to decide what should happen when a disconnection is detected.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        gameLiftServer.RemovePlayerSession(conn.connectionId);
        playerLeftGameSessionEvent.Raise();

        base.OnServerDisconnect(conn);
    }

    /// <summary>
    /// Called on server when transport raises an exception.
    /// <para>NetworkConnection may be null.</para>
    /// </summary>
    /// <param name="conn">Connection of the client...may be null</param>
    /// <param name="exception">Exception thrown from the Transport.</param>
    public override void OnServerError(NetworkConnectionToClient conn, Exception exception) { }

    #endregion

}
