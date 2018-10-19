using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(Size = 0x30, GUID = 0xB1830E142A34433B, Alignment = 0x8)]
    public class GcCustomisationDescriptorGroupOptions : GameComponentType {

        [NMS(Size = 0x20)]
        public string GroupTitle;
        public List<GcCustomisationDescriptorGroupOption> DescriptorGroupOptions;
    }
}
