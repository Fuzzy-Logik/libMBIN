﻿using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x4A9DE083678EC5C2)]
    public class GcPlayerMissionUpgradeMapEntry : GameComponent {

        [NMS(Size = 0x10)]
        /* 0x00 */ public string Mission;
        /* 0x10 */ public int MinProgress;
        /* 0x14 */ public int CompletePoint;
        [NMS(Size = 0x10)]
        /* 0x18 */ public string NewMission;

        /* 0x28 */ public List<NMSString0x10> CompleteMissions;
    }
}
