using System.Collections.Generic;

using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0xAD7818FB633DD38)]
    public class GcDefaultMissionProductEnum : GameComponentType {

		public enum DefaultProductTypeEnum { None, PrimaryProduct, SecondaryProduct }
		public DefaultProductTypeEnum DefaultProductType;
    }
}
