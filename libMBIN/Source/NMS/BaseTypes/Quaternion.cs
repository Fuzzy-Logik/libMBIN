namespace libMBIN.NMS {

    [NMS( Alignment = 0x10 )]
    public class Quaternion : NMSType {

        public float x;
        public float y;
        public float z;
        public float w;

        public Quaternion( float x, float y, float z, float w ) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Quaternion() { }

    }

}
