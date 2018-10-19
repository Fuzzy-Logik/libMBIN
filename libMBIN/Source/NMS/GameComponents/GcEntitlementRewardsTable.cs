using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xDA2E856096A99DAE)]
    public class GcEntitlementRewardsTable : GameComponentType {

        public List<GcEntitlementRewardData> Table;
    }
}
