using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xD0EAD3840328781D)]
    public class GcFrigateTraitTable : GameComponent {

        public List<GcFrigateTraitData> Traits;
    }
}
