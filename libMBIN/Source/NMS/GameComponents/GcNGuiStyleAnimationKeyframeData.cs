using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x9108CA5BEA846FD8)]
    public class GcNGuiStyleAnimationKeyframeData : GameComponent {

        public float Position;
        public List<GameComponent> StyleProperties;
    }
}
