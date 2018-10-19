using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.Toolkit
{
	[NMS(GUID = 0x209CCBD02527DE07)]
    public class TkInteractivityData : ToolkitDataType {

        public List<TkInteractiveSceneData> Scenes;
    }
}
