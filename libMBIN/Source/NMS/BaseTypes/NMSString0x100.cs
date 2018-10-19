namespace libMBIN.NMS {

    public class NMSString0x100 : NMSType {

        [NMS( Size = 0x100 )]
        private string _val;

        public static implicit operator string( NMSString0x100 str ) => str._val;
        public static implicit operator NMSString0x100( string str ) => new NMSString0x100() { _val = str };

    }

}
