﻿using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.Toolkit
{
	[NMS(Size = 0x38, GUID = 0xCE2D0D8381C37963, SubGUID = 0xF6987FC027F7291)]
    public class TkInteractiveControlData : NMSTemplate
    {
        [NMS(Size = 0x20)]
        public string Id;
        [NMS(Size = 0x10)]
        public string RewardId;
        public float Cooldown;
        [NMS(Size = 0x4, Ignore = true)]
        public byte[] EndPadding;
    }
}
