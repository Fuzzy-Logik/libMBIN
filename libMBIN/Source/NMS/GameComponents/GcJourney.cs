using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xC7C1C04352D3F6A5)]
    public class GcJourney : GameComponent {

        public List<GcJourneyCategory> Categories;
    }
}
