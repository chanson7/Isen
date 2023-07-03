using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PlayerSession", menuName = "ScriptableObjects/PlayerSession", order = 1)]
public class PlayerSession : ScriptableObject
{
    public string playerSessionId;
    public string ipAddress;
    public int port;
    public string dnsName;

    [SerializeField] ScriptableEvent onPlayerSessionUpdated;

    public void UpdatePlayerSession(dynamic data)
    {
        try
        {
            this.playerSessionId = data.PlayerSessionId;
            this.ipAddress = data.IpAddress;
            this.port = data.Port;
            this.dnsName = data.DnsName;

            UnityMainThreadDispatcher.Instance().Enqueue(() => onPlayerSessionUpdated.Raise());
        }
        catch (Exception e)
        {
            Debug.Log($"..Could not update current player session: \n{e.Data}");
        }
    }

    public void ClearPlayerSession()
    {
        this.playerSessionId = "";
        this.ipAddress = "";
        this.port = 0;
        this.dnsName = "";
    }

}