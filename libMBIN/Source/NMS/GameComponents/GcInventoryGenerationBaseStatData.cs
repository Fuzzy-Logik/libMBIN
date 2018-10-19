using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xEE43876427F39D95)]
    public class GcInventoryGenerationBaseStatData : GameComponent {

        [NMS(Size = 0x4, EnumValue = new[] { "C", "B", "A", "S" })]
        public GcInventoryGenerationBaseStatClassData[] BaseStatsPerClass;

    }
}
