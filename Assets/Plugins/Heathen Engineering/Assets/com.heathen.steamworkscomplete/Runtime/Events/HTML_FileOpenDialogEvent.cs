#if !DISABLESTEAMWORKS && HE_SYSCORE && STEAMWORKS_NET
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
    [System.Serializable]
    public class HTML_FileOpenDialogEvent : UnityEvent<HTML_FileOpenDialog_t> { };
}
#endif