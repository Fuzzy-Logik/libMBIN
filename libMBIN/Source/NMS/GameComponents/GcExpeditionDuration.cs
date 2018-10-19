using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x2581475059916919)]
    public class GcExpeditionDuration : GameComponent {

		public enum ExpeditionDurationEnum { VeryShort, Short, Medium, Long, VeryLong }
		public ExpeditionDurationEnum ExpeditionDuration;
    }
}
