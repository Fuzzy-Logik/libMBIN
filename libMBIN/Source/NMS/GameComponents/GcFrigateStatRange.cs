using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(Size = 0x8, GUID = 0xDB66AE1C6311A9D4)]
    public class GcFrigateStatRange : GameComponentType {

        public int Minimum;
        public int Maximum;
    }
}
