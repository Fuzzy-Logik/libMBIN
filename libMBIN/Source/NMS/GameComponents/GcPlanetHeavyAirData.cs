﻿using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(Size = 0x120, GUID = 0xDD82CD3A91965CEC, SubGUID = 0x2E1B1C402A21253)]
    public class GcPlanetHeavyAirData : NMSTemplate
    {
        [NMS(Size = 0x80)]
        public string Filename;
        [NMS(Size = 5)]
        public GcHeavyAirColourData[] Colours;
    }

}
