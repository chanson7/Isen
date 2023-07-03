#if !DISABLESTEAMWORKS && HE_SYSCORE && STEAMWORKS_NET

namespace HeathenEngineering.SteamworksIntegration
{
    public struct ConsumeOrder
    {
        public Steamworks.SteamItemDetails_t detail;
        public uint quantity;
    }
}
#endif