using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xCCF2A9A7FC67347F)]
    public class GcMissionConditionMissionMessage : GameComponent {

        [NMS(Size = 0x10)]
        public string Message;
    }
}
