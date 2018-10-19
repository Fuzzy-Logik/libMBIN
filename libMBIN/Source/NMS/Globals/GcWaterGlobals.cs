namespace libMBIN.NMS.Globals {

    [NMS( GUID = 0x1E1FC5B279A0B148 )]
    public class GcWaterGlobals : GlobalDataType {

        /* 0x00 */ public bool   RenderReflections;
        /* 0x01 */ public bool   UseNewWater;
        /* 0x04 */ public float  WaveHeight;
        /* 0x08 */ public float  WaveFrequency;
        /* 0x0C */ public float  WaveChoppiness;
        /* 0x10 */ public float  WaveCutoff;
        /* 0x14 */ public float  Epsilon;
        /* 0x18 */ public float  FresnelPow;
        /* 0x1C */ public float  FresnelMul;
        /* 0x20 */ public float  FresnelAlpha;
        /* 0x24 */ public float  FresnelBelowPow;
        /* 0x28 */ public float  FresnelBelowMul;
        /* 0x2C */ public float  FresnelBelowAlpha;
        /* 0x30 */ public Colour WaterColourBase;
        /* 0x40 */ public Colour WaterColourAdd;
        /* 0x50 */ public Colour WaterHeavyAirColour;

    }

}