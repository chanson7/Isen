﻿#if !DISABLESTEAMWORKS && HE_SYSCORE && STEAMWORKS_NET
using System;
using System.Linq;

namespace HeathenEngineering.SteamworksIntegration
{
    [Serializable]
    public struct ItemChangeRecord
    {
        public ItemDefinition item;
        public ItemInstanceChangeRecord[] changes;

        public bool HasChanges => changes != null && changes.Length > 0;
        public long TotalQuantityBefore => changes.Sum(x => x.quantityBefore);
        public long TotalQuantityAfter => changes.Sum(x => x.quantityAfter);
        public long TotalQuantityChange => changes.Sum(x => x.QuantityChange);
    }
}
#endif