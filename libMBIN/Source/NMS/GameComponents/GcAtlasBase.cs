﻿using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(Size = 0x298, GUID = 0x1512A8A65E002815)]
    public class GcAtlasBase : GameComponent {

        [NMS(Size = 0x200)]
        /* 0x000 */ public string OpaqueData;
        /* 0x200 */ public GcUniverseAddressData UniverseAddress;
        [NMS(Size = 0x80)]
        /* 0x218 */ public string CustomName;
    }
}
