using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace libMBIN.NMS.Toolkit {

    [NMS( Size = 0xA0, GUID = 0x990A0508C5DCEEE6 )]
    public class TkMeshData : ToolkitComponent, ISerialize, IDeserialize {

        [NMS( Size = 0x80, Padding = 0xFE )]
        /* 0x00 */ public string IdString;
        /* 0x80 */ public ulong Hash;
        /* 0x88 */ public int VertexDataSize;
        /* 0x8C */ public int IndexDataSize;
        /* 0x90 */ public byte[] MeshDataStream;

        public object OnDeserialize( BinaryReader reader, Type field, NMSAttribute settings, long templatePosition, FieldInfo fieldInfo ) {
            if ( fieldInfo.Name != nameof( MeshDataStream ) ) return null;

            long listPosition = reader.BaseStream.Position;

            long listStartOffset = reader.ReadInt64();
            int numEntries = reader.ReadInt32();
            uint listMagic = reader.ReadUInt32();
            if ( (listMagic & 0xFF) != 1 ) throw new InvalidListException( listMagic );
            long listEndPosition = reader.BaseStream.Position;

            reader.BaseStream.Position = listPosition + listStartOffset;
            byte[] data = new byte[numEntries];
            data = reader.ReadBytes( numEntries );
            reader.BaseStream.Position = listEndPosition;
            return data;
        }

        public bool OnSerialize( BinaryWriter writer, Type field, object fieldData, NMSAttribute settings, FieldInfo fieldInfo, ref List<Tuple<long, object>> additionalData, ref int addtDataIndex ) {
            if ( fieldInfo.Name != nameof( MeshDataStream ) ) return false;

            writer.Align( 8, 0 );

            // write empty list header
            long listPos = writer.BaseStream.Position;
            writer.Write( (Int64) 0 ); // listPosition
            writer.Write( (Int32) MeshDataStream.Length ); // size of data chunk in bytes
            writer.Write( (UInt32) 0xFEFEFE01 );

            additionalData.Insert( addtDataIndex, new Tuple<long, object>( listPos, fieldData ) );
            addtDataIndex++;
            return true;
        }

    }

}
