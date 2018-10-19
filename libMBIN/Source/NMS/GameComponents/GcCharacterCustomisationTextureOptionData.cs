﻿using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(Size = 0x30, GUID = 0x5794C8176DF00703)]
    public class GcCharacterCustomisationTextureOptionData : GameComponent {

        [NMS(Size = 0x10)]
        public string TextureOptionGroupName;
        [NMS(Size = 0x20)]
        public string TextureOptionName;
    }
}
