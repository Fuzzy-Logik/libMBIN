using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xF373CC74C47A4AF)]
    public class GcStatDisplayType : GameComponent {

		public enum StatDisplayTypeEnum { None, Sols, Distance }
		public StatDisplayTypeEnum StatDisplayType;
    }
}
