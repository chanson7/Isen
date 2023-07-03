#if !DISABLESTEAMWORKS && HE_SYSCORE && STEAMWORKS_NET
using Steamworks;
using System;
using System.Collections.Generic;

namespace HeathenEngineering.SteamworksIntegration
{
    [Serializable]
    public struct LeaderboardScores
    {
        public bool bIOFailure;
        public bool playerIncluded;
        public List<LeaderboardEntry> scoreData;
    }
}
#endif
