using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xC7A12F4728E8043)]
    public class GcCreatureRoleFrequencyModifier : GameComponentType {

		public enum CreatureRoleFrequencyModifierEnum { Never, Low, Normal, High }
		public CreatureRoleFrequencyModifierEnum CreatureRoleFrequencyModifier;
    }
}
