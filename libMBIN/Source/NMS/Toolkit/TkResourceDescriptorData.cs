﻿using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.Toolkit
{
	[NMS(GUID = 0xABD3B610D1E296E6)]
    public class TkResourceDescriptorData : ToolkitComponent {

        [NMS(Size = 0x20)]
        public string Id;
        [NMS(Size = 0x80)]
        public string Name;
        public List<NMSString0x80> ReferencePaths;
        public float Chance;
        public List<NMSTemplate> Children;
    }
}
