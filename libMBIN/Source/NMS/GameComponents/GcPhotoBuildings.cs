using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(Size = 0x8, GUID = 0xDF5541EE134C5839)]
    public class GcPhotoBuildings : GameComponent {

        public short AmountMin;
        public short AmountMax;
        public GcPhotoBuilding BuildingType;
    }
}
