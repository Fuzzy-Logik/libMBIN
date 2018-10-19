namespace libMBIN.NMS {

    public class NMSString0x40 : NMSType {

        [NMS( Size = 0x40 )]
        private string _val;

        public static implicit operator string( NMSString0x40 str ) => str._val;
        public static implicit operator NMSString0x40( string str ) => new NMSString0x40() { _val = str };

    }

}
