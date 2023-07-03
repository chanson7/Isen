#if !DISABLESTEAMWORKS && HE_SYSCORE && STEAMWORKS_NET
using System;

namespace HeathenEngineering.SteamworksIntegration
{
    [Serializable]
    public struct ItemProperty
    {
        public string key;
        public string value;
    }
}
#endif