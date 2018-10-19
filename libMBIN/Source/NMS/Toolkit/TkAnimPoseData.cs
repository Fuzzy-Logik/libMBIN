using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.Toolkit
{
	[NMS(GUID = 0x9A55EA80BBEF4621)]
    public class TkAnimPoseData : ToolkitDataType {

        [NMS(Size = 0x10)]
        public string Anim;
        public int FrameStart;
        public int FrameEnd;
    }
}
