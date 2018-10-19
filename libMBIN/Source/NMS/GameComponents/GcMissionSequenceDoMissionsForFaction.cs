using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.GameComponents
{
	[NMS(GUID = 0x7201A77CBA1AD625)]
    public class GcMissionSequenceDoMissionsForFaction : GameComponentType {

        [NMS(Size = 0x80)]
        /* 0x000 */ public string MessageGetToSpace;
        /* 0x080 */ public GcFactionSelectOptions SelectFrom;

        /* 0x088 */ public int AmountMine;
        /* 0x08C */ public int AmountMax;
        [NMS(Size = 0x80)]
        /* 0x190 */ public string DebugText;
    }
}
