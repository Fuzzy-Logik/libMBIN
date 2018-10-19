using System;
using System.IO;
using System.Runtime.InteropServices;

namespace libMBIN {

    using MBIN;

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
            int len = Marshal.SizeOf<MBINHeader>();
            var bytes = new byte[len];

            io.Stream.Position = 0;
            if ( io.Stream.Read( bytes, 0, len ) != len ) return false;

            GCHandle handle = GCHandle.Alloc( bytes, GCHandleType.Pinned );
            try {
                header = new MBINHeader();
                Marshal.PtrToStructure( handle.AddrOfPinnedObject(), header );
            } finally {
                handle.Free();
            }

            return true;
        }

        public bool SaveHeader() {
            int len = Marshal.SizeOf<MBINHeader>();
            var bytes = new byte[len];

            GCHandle handle = GCHandle.Alloc( bytes, GCHandleType.Pinned );
            try {
                Marshal.StructureToPtr( header, handle.AddrOfPinnedObject(), false );
            } finally {
                handle.Free();
            }

            io.Stream.Position = 0;
            io.Writer.Write( bytes );
            io.Writer.Flush();

            return true;
        }

        public NMSTemplate LoadData() {
            io.Stream.Position = Marshal.SizeOf<MBINHeader>();
            return DeserializeMBIN.DeserializeBinaryTemplate( io.Reader, header.GetXMLTemplateName() );
        }

        public void SaveData( NMSTemplate template ) {
            int headerLen = Marshal.SizeOf<MBINHeader>();
            io.Stream.SetLength( headerLen );
            io.Stream.Position = headerLen;

            byte[] data = SerializeMBIN.SerializeBytes( template );
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
