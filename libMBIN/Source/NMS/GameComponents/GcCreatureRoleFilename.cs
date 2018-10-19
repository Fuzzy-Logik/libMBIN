using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xF172C1FA6F2F4771)]
    public class GcCreatureRoleFilename : GameComponent {

        [NMS(Size = 0x80)]
        public string File;

        [NMS(Size = 4, EnumValue = new[] { "Dead", "Low", "Mid", "Full" })]
        public float[] BiomeProbability;
    }
}
