using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x628C2F4F61CC6005)]
    public class GcRewardStanding : GameComponentType {

        public GcAlienRace Race;
        public int AmountMin;
        public int AmountMax;
    }
}
