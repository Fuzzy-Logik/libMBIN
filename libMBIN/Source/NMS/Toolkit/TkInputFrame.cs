using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.Toolkit
{
	[NMS(GUID = 0x46FE92211FE98307)]
    public class TkInputFrame : ToolkitComponent {

        public Vector2f LeftStick;
        public Vector2f RightStick;
        public float LeftTrigger;
        public float RightTrigger;
        public short Buttons;

        [NMS(Size = 2, Ignore = true)]
        public byte[] Padding1A;
    }
}
