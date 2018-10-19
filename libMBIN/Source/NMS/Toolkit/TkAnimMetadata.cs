using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.Toolkit
{
	[NMS(GUID = 0x3CD7D2192958BA6C)]
    public class TkAnimMetadata : ToolkitData {

        public int FrameCount;
        public int NodeCount;

        public List<TkAnimNodeData> NodeData;
        public List<TkAnimNodeFrameData> AnimFrameData;

        public TkAnimNodeFrameData StillFrameData;
    }
}
