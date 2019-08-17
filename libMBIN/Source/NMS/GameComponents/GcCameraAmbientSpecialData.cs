﻿using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(Size = 0x28, GUID = 0x711D55B308F4783B, SubGUID = 0xC266DA7C9BDD51E7)]
    public class GcCameraAmbientSpecialData : NMSTemplate
    {
        [NMS(Size = 0x10)]
        /* 0x00 */ public string Animation;
        [NMS(Size = 0x10)]
        /* 0x10 */ public string DroneAnimation;
		public enum CameraOriginEnum { ExternalBase }
		public CameraOriginEnum CameraOrigin;

        /* 0x24 */ public bool UseLookAt;
        /* 0x25 */ public bool AvoidTerrain;

        [NMS(Size = 0x2, Ignore = true)]
        /* 0x26 */ public byte[] EndPadding;
    }
}
