using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x688C27CFA51297C7)]
    public class GcRewardDamage : GameComponent {

        [NMS(Size = 0x10)]
        public string PlayerDamage;
    }
}
