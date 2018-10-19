namespace libMBIN.NMS {

    public class NMSString0x80 : NMSTemplate {

        [NMS( Size = 0x80 )]
        private string _val;

        public static implicit operator string( NMSString0x80 str ) => str._val;
        public static implicit operator NMSString0x80( string str ) => new NMSString0x80() { _val = str };

    }

}
