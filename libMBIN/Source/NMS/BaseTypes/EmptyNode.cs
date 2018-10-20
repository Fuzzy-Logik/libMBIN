namespace libMBIN.NMS {

    public class EmptyNode : Component {

        [NMS( Size = 0x0, Ignore = true )]
        public byte[] Padding;

    }

}