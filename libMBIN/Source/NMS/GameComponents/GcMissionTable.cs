using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x3F26458651227966)]
    public class GcMissionTable : GameComponent {

        public List<GcGenericMissionSequence> Missions;
    }
}
