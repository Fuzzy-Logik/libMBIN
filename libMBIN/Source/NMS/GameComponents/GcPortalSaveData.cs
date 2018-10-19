using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(Size = 0x18, GUID = 0xA9ECFDEF35F6EEDF)]
    public class GcPortalSaveData : GameComponent {

        public GcSeed PortalSeed;
        public ulong LastPortalUA;      // Universal Address
    }
}
