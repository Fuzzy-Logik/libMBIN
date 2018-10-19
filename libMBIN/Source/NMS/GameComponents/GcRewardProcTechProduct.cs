using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(Size = 0x30, Alignment = 0x8, GUID = 0x89A0651B13A26E32)]
    public class GcRewardProcTechProduct : GameComponentType {

        [NMS(Size = 0x20)]
        public string Group;
        public int WeightedChanceNormal;
        public int WeightedChanceRare;
        public int WeightedChanceEpic;
        public int WeightedChanceLegendary;
    }
}
