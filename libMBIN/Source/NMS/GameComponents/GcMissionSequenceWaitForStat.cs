﻿using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x45EBC02E8908E2BA)]
    public class GcMissionSequenceWaitForStat : GameComponent {

        [NMS(Size = 0x80)]
        /* 0x000 */ public string Message;
        [NMS(Size = 0x10)]
        /* 0x080 */ public string Stat;
        [NMS(Size = 0x10)]
        /* 0x090 */ public string StatGroup;
        /* 0x0A0 */ public int Amount;
        [NMS(Size = 0x80)]
        /* 0x0A4 */ public string DebugText;
        [NMS(Size = 0x4, Ignore = true)]
        public byte[] EndPadding;
    }
}
