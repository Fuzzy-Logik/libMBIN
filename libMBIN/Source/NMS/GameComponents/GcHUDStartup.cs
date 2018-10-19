using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xF8CB4D1699DC9BB0, Broken = true)]
    public class GcHUDStartup : GameComponent {

        /* 0x00 */ public GcAudioWwiseEvents Audio;
        /* 0x04 */ public float Time;
    }
}
