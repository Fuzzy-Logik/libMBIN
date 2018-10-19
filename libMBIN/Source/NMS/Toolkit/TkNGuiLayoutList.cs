using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.Toolkit
{
	[NMS(GUID = 0x50BDF4B763608D81)]
    public class TkNGuiLayoutList : ToolkitData {

        public List<TkNGuiLayoutListData> Layouts;
    }
}
