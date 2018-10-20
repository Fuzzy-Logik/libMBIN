namespace libMBIN.NMS {

    public class EmptyNode : GameComponent {

        [NMS( Size = 0x0, Ignore = true )]
        public byte[] Padding;

    }

}