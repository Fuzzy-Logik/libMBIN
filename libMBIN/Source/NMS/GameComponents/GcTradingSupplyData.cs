using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x47465FFF4364C345)]
    public class GcTradingSupplyData : GameComponent {

        public ulong GalacticAddress;
        public float Supply;
        public float Demand;
        [NMS(Size = 0x10)]
        public string Product;
        public ulong Timestamp;
    }
}
