using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xFB6953103434EDD8)]
    public class GcAtlasDiscoveryMetadata : GameComponentType {

        [NMS(Size = 0x80)]
        public string CustomName;
    }
}
