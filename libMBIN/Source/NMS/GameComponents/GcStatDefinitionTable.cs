using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x2412C3A937D62ED5)]
    public class GcStatDefinitionTable : GameComponent {

        public List<GcStatDefinition> StatDefinitionTable;
    }
}
