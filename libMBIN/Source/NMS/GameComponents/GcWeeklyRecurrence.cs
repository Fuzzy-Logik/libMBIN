using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(Size = 0x8C, GUID = 0xE475AF3D8A81BB1F)]
    public class GcWeeklyRecurrence : GameComponentType {

        public int RecurrenceMinute;
        public int RecurrenceHour;
        public GcDay RecurrenceDay;
        [NMS(Size = 0x80)]
        public string DebugText;
    }
}
