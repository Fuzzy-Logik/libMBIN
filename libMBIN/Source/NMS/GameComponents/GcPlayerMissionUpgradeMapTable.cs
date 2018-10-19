using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x54511EBAE64FC462)]
    public class GcPlayerMissionUpgradeMapTable : GameComponentType {

        public List<GcPlayerMissionUpgradeMapEntry> MissionUpgradeTable;
    }
}
