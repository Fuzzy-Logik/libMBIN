using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(Size = 0x8, GUID = 0x889A7E6B75959E0E)]
    public class GcTerrainTextureSettings : GameComponent {

        public float Brightness;
        public float Contrast;
    }
}
