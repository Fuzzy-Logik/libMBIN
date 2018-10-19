using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.Toolkit
{
	[NMS(GUID = 0x9777486713E25BA7)]
    public class TkProceduralTextureChosenOptionSampler : ToolkitDataType {

        public List<TkProceduralTextureChosenOption> Options;
    }
}
