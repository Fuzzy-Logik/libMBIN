using System;
using System.IO;
using System.Runtime.InteropServices;

namespace libMBIN {

    using MBIN;
    using NMS;

    public class MbinFile : IDisposable {

        public MbinHeader header;
        private readonly IO io;
        private readonly string filePath;
        private readonly bool keepOpen;
        public ulong fileLength = 0;

        public static bool IsValid( string path ) {
            if ( !File.Exists( path ) ) return false;
            using ( var mbin = new MbinFile( path ) ) {
                return mbin.LoadHeader() ? mbin.header.IsValid : false;
            }
        }

        public MbinFile( string path ) : this( path, new IO( path, FileMode.OpenOrCreate ), false ) { }
        public MbinFile( Stream stream, bool keepOpen = false ) : this( "/DEV/NULL", new IO( stream ), keepOpen ) { }
        private MbinFile( string path, IO io, bool keepOpen = false ) {
            filePath = path;
            this.io = io;
            this.keepOpen = keepOpen;
        }

        public bool LoadHeader() {
            int len = Marshal.SizeOf<MbinHeader>();
            var bytes = new byte[len];

            io.Stream.Position = 0;
            if ( io.Stream.Read( bytes, 0, len ) != len ) return false;

            GCHandle handle = GCHandle.Alloc( bytes, GCHandleType.Pinned );
            try {
                header = new MbinHeader();
                Marshal.PtrToStructure( handle.AddrOfPinnedObject(), header );
            } finally {
                handle.Free();
            }

            return true;
        }

        public bool SaveHeader() {
            int len = Marshal.SizeOf<MbinHeader>();
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

        public NMSType LoadData() {
            io.Stream.Position = Marshal.SizeOf<MbinHeader>();
            return MbinSerializer.DeserializeBinaryTemplate( io.Reader, header.GetXMLTemplateName() );
        }

        public void SaveData( NMSType template ) {
            int headerLen = Marshal.SizeOf<MbinHeader>();
            io.Stream.SetLength( headerLen );
            io.Stream.Position = headerLen;

            byte[] data = MbinSerializer.SerializeBytes( template );
            io.Writer.Write( data );

            fileLength = (ulong) data.LongLength;

            header.TemplateName = "c" + template.GetType().Name;
        }

        public static implicit operator NMSType( MbinFile mbin ) {
            return mbin.LoadData();
        }

        public void Dispose() {
            if ( io != null && keepOpen == false ) io.Dispose();
        }

    }

}
