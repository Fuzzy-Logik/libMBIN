﻿using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(Size = 0x20, GUID = 0x94D7D10B4E01B45E, SubGUID = 0x74723646531009E2)]
    public class GcAISpaceshipPreloadCacheData : NMSTemplate
    {
        /* 0x00 */ public GcRealityCommonFactions Faction;
        /* 0x04 */ public GcAISpaceshipRoles ShipRole;
        /* 0x08 */ public GcSpaceshipClasses ShipClass;
        /* 0x0C */ public GcFrigateClass FrigateClass;
        /* 0x10 */ public GcSeed Seed;
    }
}
