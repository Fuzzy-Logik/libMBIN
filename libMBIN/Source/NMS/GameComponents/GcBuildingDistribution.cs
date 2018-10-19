using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x401D8256EF89A26A)]
    public class GcBuildingDistribution : GameComponent {

        [NMS(Size = 0x10)]
        public string Name;
        public int MinDistance;
        public int MaxDistance;
    }
}
