using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xD2B07B00F953DE26)]
    public class GcMissionIDEpochPair : GameComponent {

        [NMS(Size = 0x10)]
        public string MissionID;
        public ulong RecurrenceDeadline;
    }
}
