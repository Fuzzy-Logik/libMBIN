using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x6F5BDF62AC518480)]
    public class GcAlienSpeechTable : GameComponentType {

        public List<GcAlienSpeechEntry> Table;
    }
}
