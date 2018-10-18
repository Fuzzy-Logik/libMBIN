using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS
{
    // todo: maybe get rid of this and just read strings directly into the list, if all strings are the same size?
    public class NMSString0x80 : NMSTemplate
    {
        [NMS(Size = 0x80)]
        private string _val;

        public static implicit operator string(NMSString0x80 str) => str._val;
        public static implicit operator NMSString0x80(string str) => new NMSString0x80() { _val = str };
    }
}
