using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xDBC20526A13393C3)]
    public class GcSavedEntitlement : GameComponentType {

        [NMS(Size = 0x100)]
        public string EntitlementId;
    }
}
