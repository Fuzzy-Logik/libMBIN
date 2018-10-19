using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x3F821EFA320B5781)]
    public class GcInventoryGenerationBaseStatClassData : GameComponent {

        public List<GcInventoryGenerationBaseStatDataEntry> BaseStats;
    }
}
