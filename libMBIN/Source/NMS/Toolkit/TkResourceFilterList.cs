using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.Toolkit
{
	[NMS(GUID = 0xD292EC640A03C7BF)]
    public class TkResourceFilterList : ToolkitComponent {

        public List<TkResourceFilterData> Filters;

    }
}
