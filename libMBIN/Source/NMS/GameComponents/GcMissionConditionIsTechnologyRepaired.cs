using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xF058A2DC4DC3CFDF, Broken = true)]
    public class GcMissionConditionIsTechnologyRepaired : GameComponentType {

        public GcTechnologyTableEnum Technology;        // I bet this has got bigger...
        public int RepairedComponents;
    }
}
