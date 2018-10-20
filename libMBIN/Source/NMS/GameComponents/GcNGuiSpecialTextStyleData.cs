using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x52ECDFFDB4B7B1CD)]
    public class GcNGuiSpecialTextStyleData : GameComponent {

        [NMS(Size = 0x10)]
        public string Name;

        public List<Component> StyleProperties;
        public GcNGuiStyleAnimationData Animation;
    }
}
