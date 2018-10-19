using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xC61664D2E822E1ED)]
    public class GcBiomeFileListOptions : GameComponentType {

        public List<NMSString0x80> BiomeFiles;
    }
}
