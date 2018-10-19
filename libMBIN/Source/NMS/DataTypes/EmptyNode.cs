namespace libMBIN.NMS {

    public class EmptyNode : NMSType {

        [NMS( Size = 0x0, Ignore = true )]
        public byte[] Padding;

    }

}