using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xDDE56711938E1BC7)]
    public class GcInventoryLayoutGenerationDataEntry : GameComponentType {

        public int MinSlots;            // 1
        public int MaxSlots;            // 5
        public int MinExtraTech;        // 1
        public int MaxExtraTech;        // 3
    }
}
