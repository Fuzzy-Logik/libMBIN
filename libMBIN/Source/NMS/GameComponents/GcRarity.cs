using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x254FD01AB6B98C6A)]
    public class GcRarity : GameComponentType {


		public enum RarityEnum { Common, Uncommon, Rare, Extraordinary, None }
		public RarityEnum Rarity;
    }
}
