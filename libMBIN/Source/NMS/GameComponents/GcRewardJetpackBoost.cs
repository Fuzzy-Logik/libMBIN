using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x16CDB89E560B4D9A)]
    public class GcRewardJetpackBoost : GameComponentType {

        public float Duration;
        public float ForwardBoost;
        public float UpBoost;
        public float IgnitionBoost;
    }
}
