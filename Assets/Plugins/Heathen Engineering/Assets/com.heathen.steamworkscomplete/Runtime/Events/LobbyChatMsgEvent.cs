#if !DISABLESTEAMWORKS && HE_SYSCORE && STEAMWORKS_NET
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
    [System.Serializable]
    public class LobbyChatMsgEvent : UnityEvent<LobbyChatMsg> { }
}
#endif