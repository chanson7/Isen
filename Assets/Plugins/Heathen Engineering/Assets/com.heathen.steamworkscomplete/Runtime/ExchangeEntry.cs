#if !DISABLESTEAMWORKS && HE_SYSCORE && STEAMWORKS_NET
using Steamworks;

namespace HeathenEngineering.SteamworksIntegration
{
    public struct ExchangeEntry
    {
        public SteamItemInstanceID_t instance;
        public uint quantity;
    }

}
#endif