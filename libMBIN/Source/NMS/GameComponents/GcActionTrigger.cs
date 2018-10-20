using System.Collections.Generic;

namespace libMBIN.NMS.GameComponents {

    [NMS( Size = 0x58, GUID = 0x1DFBCEEAFDE9D7C6 )]
    public class GcActionTrigger : GameComponent {

        public Component Trigger;
        public List<Component> Action;

    }

}
