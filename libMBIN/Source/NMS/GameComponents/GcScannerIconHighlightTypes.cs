﻿using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x5CC4805D05B121FD, SubGUID = 0x1E73CF8A2A7FA65D)]
    public class GcScannerIconHighlightTypes : NMSTemplate
    {
        // Size: 0x5
		public enum ScannerIconHighlightTypeEnum { Diamond, Hexagon, Tag, Octagon, Circle }
		public ScannerIconHighlightTypeEnum ScannerIconHighlightType;
    }
}
