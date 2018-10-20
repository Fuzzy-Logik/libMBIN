using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xF7836692C9FF6FC1)]
    public class GcTextPreset : GameComponent {

        public GcFontTypesEnum FontType;
        public Component TextStyle;

        public float Height;
        public Colour Colour;
    }
}
