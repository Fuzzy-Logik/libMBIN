namespace libMBIN.NMS {

    public class NMSString0x10 : NMSType {

        [NMS( Size = 0x10 )]
        private string _val;

        public static implicit operator string( NMSString0x10 str ) => str._val;
        public static implicit operator NMSString0x10( string str ) => new NMSString0x10() { _val = str };

    }

}
