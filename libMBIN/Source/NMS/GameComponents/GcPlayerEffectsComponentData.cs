using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xB5428717C07E10EE)]
    public class GcPlayerEffectsComponentData : GameComponent {

        public float VehicleInOutTime;
        public float VehicleInOutEffectDelay;
        public float VehicleInOutDissolveDelay;
    }
}
