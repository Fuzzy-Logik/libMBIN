using System;
using System.IO;

namespace libMBIN {

    public class MBINFile : IDisposable {

        public MBINHeader header;
        private readonly IO io;
        private readonly string filePath;
        private readonly bool keepOpen;
        public ulong fileLength = 0;

        public static bool IsValid( string path ) {
            if ( !File.Exists( path ) ) return false;
            using ( var mbin = new MBINFile( path ) ) {
                return mbin.LoadHeader() ? mbin.header.IsValid : false;
            }
        }

        public MBINFile( string path ) : this( path, new IO( path, FileMode.OpenOrCreate ), false ) { }
        public MBINFile( Stream stream, bool keepOpen = false ) : this( "/DEV/NULL", new IO( stream ), keepOpen ) { }
        private MBINFile( string path, IO io, bool keepOpen = false ) {
            filePath = path;
            this.io = io;
            this.keepOpen = keepOpen;
        }

        public bool LoadHeader() {
            if ( io.Stream.Length < 0x60 ) return false;
            io.Stream.Position = 0;
            header = (MBINHeader) NMSTemplate.DeserializeBinaryTemplate( io.Reader, "MBINHeader" );
            return true;
        }

        public bool SaveHeader() {
            io.Stream.Position = 0;
            io.Writer.Write( header.SerializeBytes() );
            io.Writer.Flush();

            return true;
        }

        public NMSTemplate LoadData() {
            io.Stream.Position = 0x60;
            return NMSTemplate.DeserializeBinaryTemplate( io.Reader, header.GetXMLTemplateName() );
        }

        public void SaveData( NMSTemplate template ) {
            io.Stream.SetLength( 0x60 );
            io.Stream.Position = 0x60;

            byte[] data = template.SerializeBytes();
            io.Writer.Write( data );

            fileLength = (ulong) data.LongLength;

            header.TemplateName = "c" + template.GetType().Name;
        }

        public static implicit operator NMSTemplate( MBINFile mbin ) {
            return mbin.LoadData();
        }

        public void Dispose() {
            if ( io != null && keepOpen == false ) io.Dispose();
        }

    }

}
