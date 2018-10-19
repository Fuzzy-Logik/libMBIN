using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x4CFC57AEE31F9054)]
    public class GcTerrainEdit : GameComponentType {

        public int Data;        // this is actually a byte, but mbincompiler can't deseriaise a single byte (would need to add it...)
        public int Position;
    }
}
