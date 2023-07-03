#if !DISABLESTEAMWORKS && HE_SYSCORE && STEAMWORKS_NET

namespace HeathenEngineering.SteamworksIntegration
{
    [System.Serializable]
    public struct UserLobbyLeaveData
    {
        public UserData user;
        public Steamworks.EChatMemberStateChange state;
    }
}
#endif