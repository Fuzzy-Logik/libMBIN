using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xEC55A7DD450FE529)]
    public class GcItemCostTable : GameComponent {

        public List<GcItemCostData> Items;
    }
}
