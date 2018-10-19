using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xE3FCFF5B621FA649)]
    public class GcRewardShipAssistance : GameComponent {

		public enum AssistanceTypeEnum { Police, Wingmen }
		public AssistanceTypeEnum AssistanceType;

        public float Time;
    }
}
