using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS
{
    // todo: maybe get rid of this and just read strings directly into the list, if all strings are the same size?
    public class NMSString0x100 : NMSTemplate
    {
        [NMS(Size = 0x100)]
        private string _val;

        public static implicit operator string(NMSString0x100 str) => str._val;
        public static implicit operator NMSString0x100(string str) => new NMSString0x100() { _val = str };
    }
}
