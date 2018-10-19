using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x41FEBAC9A23EF9D6)]
    public class GcDiscoveryHelperTimings : GameComponentType {


        public float DiscoverPlanetTotalTime;           // 41200000h
        public float DiscoverPlanetMessageWait;         // 3F800000h
        public float DiscoverPlanetMessageTime;         // 40E00000h
    }
}
