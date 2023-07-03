#if !DISABLESTEAMWORKS && HE_SYSCORE && STEAMWORKS_NET
#if MIRROR
#endif


namespace HeathenEngineering.SteamworksIntegration
{
    /// <summary>
    /// Enumberator used in Game Server Browser searches
    /// </summary>
    public enum GameServerSearchType
    {
        Internet,
        Friends,
        Favorites,
        LAN,
        Spectator,
        History
    }
}
#endif