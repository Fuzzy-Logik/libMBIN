using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x43D35B6DB970549F)]
    public class GcTexturePrefetchData : GameComponentType {

        public List<NMSString0x80> Textures;
    }
}
