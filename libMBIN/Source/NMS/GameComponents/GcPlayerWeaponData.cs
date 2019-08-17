﻿using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(Size = 0x10, GUID = 0x9C4672E772EC8C97, SubGUID = 0x29FC3BB581E5C03D)]
    public class GcPlayerWeaponData : NMSTemplate
    {
        [NMS(Size = 0x10)]
        public string Reticle;
    }
}
