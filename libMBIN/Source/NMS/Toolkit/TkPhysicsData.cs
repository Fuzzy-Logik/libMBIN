using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.Toolkit
{
	[NMS(GUID = 0xE144A5F6E94E6409)]
    public class TkPhysicsData : ToolkitDataType {

        public float Mass;
        public float Friction;
        public float RollingFriction;
        public float AngularDamping;
        public float LinearDamping;
        public float Gravity;
    }
}
