using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x58F8C89CC8A7C2F0)]
    public class GcCreatureRoleDataTable : GameComponent {

        public List<GcCreatureRoleData> AvailableRoles;
    }
}
