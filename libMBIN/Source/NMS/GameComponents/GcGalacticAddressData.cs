using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xCDC0444569389B85)]
    public class GcGalacticAddressData : GameComponentType {

        public int VoxelX;

        public int VoxelY;

        public int VoxelZ;

        public int SolarSystemIndex;

        public int PlanetIndex;
    }
}
