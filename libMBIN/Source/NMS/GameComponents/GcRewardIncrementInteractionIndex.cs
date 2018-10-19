using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xC077DBEECAE0D84B)]
    public class GcRewardIncrementInteractionIndex : GameComponentType {

        public int InteractionToIncrement;
        public GcAlienRace Race;
    }
}
