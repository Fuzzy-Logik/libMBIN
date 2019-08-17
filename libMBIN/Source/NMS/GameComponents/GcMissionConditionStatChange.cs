﻿using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(Size = 0x20, GUID = 0x8BFCD316778640C0, SubGUID = 0x9481201C43F8E7A3)]
    public class GcMissionConditionStatChange : NMSTemplate
    {
        [NMS(Size = 0x10)]
        public string Stat;
        [NMS(Size = 0x10)]
        public string StatGroup;
    }
}
