#if !DISABLESTEAMWORKS && HE_SYSCORE && STEAMWORKS_NET
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
    [System.Serializable]
    public class HTML_JSAlertEvent : UnityEvent<HTML_JSAlert_t> { };
}
#endif