using UnityEngine;
using UnityEditor;
using HeathenEngineering.SteamworksIntegration;
using kcp2k;
using Mirror;

[CustomEditor(typeof(SteamNetworkManager))]
public class SteamNetworkManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SteamNetworkManager snm = (SteamNetworkManager)target;

        if (GUILayout.Button("Start Kcp Host"))
        {
            snm.transport = snm.gameObject.AddComponent<KcpTransport>();
            Transport.active = snm.transport;
            snm.StartHost();
        }

        if (GUILayout.Button("Start Kcp Client"))
        {
            snm.transport = snm.gameObject.AddComponent<KcpTransport>();
            Transport.active = snm.transport;
            snm.StartClient();
        }
    }
}