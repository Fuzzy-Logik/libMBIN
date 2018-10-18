using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS
{
    public class NMSString0x20 : NMSTemplate
    {
        [NMS(Size = 0x20)]
        private string _val;

        public static implicit operator string(NMSString0x20 str) => str._val;
        public static implicit operator NMSString0x20(string str) => new NMSString0x20() { _val = str };
    }
}
