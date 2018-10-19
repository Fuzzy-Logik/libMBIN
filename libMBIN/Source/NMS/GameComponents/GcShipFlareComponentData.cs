using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x1E6DFC2F6689E5A4)]
    public class GcShipFlareComponentData : GameComponent {

		public enum FlareTypeEnum { Default }
		public FlareTypeEnum FlareType;
    }
}
