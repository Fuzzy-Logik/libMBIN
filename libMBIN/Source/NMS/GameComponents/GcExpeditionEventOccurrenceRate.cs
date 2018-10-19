using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(Size = 0x64, GUID = 0x24812B86AFC66CF6)]
    public class GcExpeditionEventOccurrenceRate : GameComponent {

        [NMS(Size = 0x5, EnumValue = new string[] { "Combat", "Exploration", "Mining", "Diplomacy", "Balanced" })]
        public GcExpeditionDurationValues[] ExpeditionCategory;
    }
}
