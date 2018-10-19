using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x9DD8603DDE3E0BDF)]
    public class GcPhotoBuilding : GameComponent {

		public enum PhotoBuildingTypeEnum { Shelter, Abandoned, Shop, Outpost, RadioTower, Observatory, Depot, Monolith, Factory, Portal, Ruin, MissionTower }
		public PhotoBuildingTypeEnum PhotoBuildingType;
    }
}
