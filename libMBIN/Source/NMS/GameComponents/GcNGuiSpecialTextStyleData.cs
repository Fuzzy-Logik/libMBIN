using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x52ECDFFDB4B7B1CD)]
    public class GcNGuiSpecialTextStyleData : GameComponentType {

        [NMS(Size = 0x10)]
        public string Name;

        public List<NMSTemplate> StyleProperties;
        public GcNGuiStyleAnimationData Animation;
    }
}
