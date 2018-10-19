namespace libMBIN.NMS.Globals {

    [NMS( GUID = 0xF07A5559350356C7 )]
    public class GcAtlasGlobals : GlobalDataStruct {

        public int ChanceOfDisconnect;
        public int TimeoutSecNameResolution;
        public int TimeoutSecConnection;
        public int TimeoutSecSendRecv;

    }

}
