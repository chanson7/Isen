#if !DISABLESTEAMWORKS && HE_SYSCORE && STEAMWORKS_NET
using System;

namespace HeathenEngineering.SteamworksIntegration
{
    [Serializable]
    public struct ItemTag
    {
        public string category;
        public string tag;

        public override string ToString()
        {
            return category + ":" + tag;
        }
    }
}
#endif