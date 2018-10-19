using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x5CCDC0B70353BB1)]
    public class GcPlayerConflictData : GameComponent {

		public enum ConflictLevelEnum { Low, Default, High }
		public ConflictLevelEnum ConflictLevel;
    }
}
