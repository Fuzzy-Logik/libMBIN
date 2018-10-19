using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xC43CCD85B6DEFABA)]
    public class GcMissionConditionIsLookingAtAnomaly : GameComponent {

        public float FOV;
        public float MaxDistance;
    }
}
