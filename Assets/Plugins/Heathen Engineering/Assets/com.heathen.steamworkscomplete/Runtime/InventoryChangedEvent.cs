#if !DISABLESTEAMWORKS && HE_SYSCORE && STEAMWORKS_NET
using System;
using UnityEngine.Events;

namespace HeathenEngineering.SteamworksIntegration
{
    [Serializable]
    public class InventoryChangedEvent : UnityEvent<InventoryChangeRecord>
    { }
}
#endif