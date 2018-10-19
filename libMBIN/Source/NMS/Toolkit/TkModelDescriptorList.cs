using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.Toolkit
{
	[NMS(GUID = 0xB22E1ABAB4CBE5F8)]
    public class TkModelDescriptorList : ToolkitDataType {

        public List<TkResourceDescriptorList> List;
    }
}
