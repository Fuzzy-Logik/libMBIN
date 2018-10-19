namespace libMBIN.NMS {

    public class EmptyNode : NMSTemplate {

        [NMS( Size = 0x0, Ignore = true )]
        public byte[] Padding;

    }

}