using System;
using UnityEngine;

public class ClientNetworkManager : MirkwoodNetworkManager
{
    // Overrides the base singleton so we don't
    // have to cast to this type everywhere.
    public static new ClientNetworkManager singleton { get; private set; }

    [SerializeField] ClientLaunchConfiguration clientLaunchConfiguration;

    public override void Start()
    {
        singleton = this;

        if (clientLaunchConfiguration.disableServerAuth)
        {
            Debug.LogWarning($"..Authenticator has been disabled.  Make sure it is also disabled on the Server's Network Manager when testing.");
            this.authenticator = null;
        }

        base.Start();
    }

    /// <summary>
    /// Called on the client when a new GameLift Player Session has been received from AWS.
    /// </summary>
    /// <param name="playerSession">The updated Player Session scriptable object</param>

    public void ConnectToPlayerSession(PlayerSession playerSession)
    {
        Debug.Log($"..New Player Session Received, starting Mirror Client");

        singleton.StartClient(new UriBuilder
        {
            Scheme = "kcp",
            Host = playerSession.dnsName,
            Port = playerSession.port
        }.Uri);
    }

    #region Client System Callbacks

    /// <summary>
    /// Called on the client when connected to a server.
    /// <para>The default implementation of this function sets the client as ready and adds a player. Override the function to dictate what happens when the client connects.</para>
    /// </summary>
    public override void OnClientConnect()
    {
        base.OnClientConnect();
    }

    /// <summary>
    /// Called on clients when disconnected from a server.
    /// <para>This is called on the client when it disconnects from the server. Override this function to decide what happens when the client disconnects.</para>
    /// </summary>
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
    }

    /// <summary>
    /// Called on clients when a servers tells the client it is no longer ready.
    /// <para>This is commonly used when switching scenes.</para>
    /// </summary>
    public override void OnClientNotReady() { }

    /// <summary>
    /// Called on client when transport raises an exception.</summary>
    /// </summary>
    /// <param name="exception">Exception thrown from the Transport.</param>
    public override void OnClientError(Exception exception) { }

    #endregion

}
