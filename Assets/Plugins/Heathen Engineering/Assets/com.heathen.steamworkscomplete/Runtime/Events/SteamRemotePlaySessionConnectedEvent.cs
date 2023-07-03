#if !DISABLESTEAMWORKS && HE_SYSCORE && STEAMWORKS_NET
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
    [System.Serializable]
    public class SteamRemotePlaySessionConnectedEvent : UnityEvent<SteamRemotePlaySessionConnected_t> { }
}
#endif