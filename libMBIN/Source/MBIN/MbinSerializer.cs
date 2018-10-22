using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace libMBIN.MBIN {

    using NMS;

    internal static class MbinSerializer {

        internal static NMSType DeserializeBinaryTemplate( BinaryReader reader, string templateName ) {
            if ( templateName.StartsWith( "c" ) && (templateName.Length > 1) ) templateName = templateName.Substring( 1 );

            NMSType obj = NMSTemplate.TemplateFromName( templateName );

            /*using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"T:\mbincompiler_debug.txt", true))
            {
                file.WriteLine("Deserializing Template: " + templateName);
            }*/

            //DebugLog("Gk Hack: " + "Deserializing Template: " + templateName);

            if ( obj == null ) return null;

            long templatePosition = reader.BaseStream.Position;
            NMSTemplate.DebugLogTemplate( $"[T Start] {templateName}\t0x{templatePosition:X}" );
            using ( var indentScope = new Logger.IndentScope() ) {

                if ( templateName == "VariableSizeString" ) {
                    long stringPos = reader.ReadInt64();
                    int stringLength = reader.ReadInt32();
                    int unkC = reader.ReadInt32();
                    reader.BaseStream.Position = templatePosition + stringPos;
                    ((NMS.VariableSizeString) obj).Value = reader.ReadString( Encoding.UTF8, stringLength ).TrimEnd( '\x00' );
                    reader.BaseStream.Position = templatePosition + 0x10;
                    return obj;
                }

                var fields = NMSTemplate.GetOrderedFields( obj.GetType() );

                foreach ( var field in fields ) {
                    NMSAttribute settings = field.GetCustomAttribute<NMSAttribute>();

                    if ( field.FieldType.IsEnum ) {
                        field.SetValue( obj, Enum.ToObject( field.FieldType, DeserializeValue( reader, field.FieldType, settings, templatePosition, field, obj ) ) );
                    } else {
                        field.SetValue( obj, DeserializeValue( reader, field.FieldType, settings, templatePosition, field, obj ) );
                    }
                    NMSTemplate.DebugLogFieldName( $"[T] {templateName}\t0x{reader.BaseStream.Position:X}\t{field.Name}\t{field.GetValue( obj )}" );
                }

                FinishDeserialize( obj );

            }
            NMSTemplate.DebugLogTemplate( $"[T End]{templateName}\t0x{reader.BaseStream.Position:X}" );

            return obj;
        }

        internal static object DeserializeValue( BinaryReader reader, Type field, NMSAttribute settings, long templatePosition, FieldInfo fieldInfo, NMSType parent ) {
            //Logger.LogDebug( $"{fieldInfo?.DeclaringType.Name}.{fieldInfo?.Name}\ttype:\t{field.Name}\tpos:\t0x{templatePosition:X}" );

            var data = (parent as NMS.IDeserialize)?.OnDeserialize( reader, field, settings, templatePosition, fieldInfo );
            if ( data != null ) return data;

            var fieldType = field.Name;
            switch ( fieldType ) {
                case "String":
                case "Byte[]":
                    int size = settings?.Size ?? 0;
                    MarshalAsAttribute legacySettings = fieldInfo.GetCustomAttribute<MarshalAsAttribute>();
                    size = legacySettings?.SizeConst ?? size;

                    if ( fieldType == "String" ) {
                        // reader.Align(0x4, templatePosition);
                        return reader.ReadString( Encoding.UTF8, size, true );
                    } else {
                        return reader.ReadBytes( size );
                    }
                case "Single":
                    reader.Align( 4, templatePosition );
                    return reader.ReadSingle();
                case "Boolean":
                    return reader.ReadByte() != 0;
                case "Int16":
                case "UInt16":
                    reader.Align( 2, templatePosition );
                    return fieldType == "Int16" ? (object) reader.ReadInt16() : (object) reader.ReadUInt16();
                case "Int32":
                case "UInt32":
                    reader.Align( 4, templatePosition );
                    return fieldType == "Int32" ? (object) reader.ReadInt32() : (object) reader.ReadUInt32();
                case "Int64":
                case "UInt64":
                    reader.Align( 8, templatePosition );
                    return fieldType == "Int64" ? (object) reader.ReadInt64() : (object) reader.ReadUInt64();
                case "List`1":
                    reader.Align( 8, templatePosition );
                    if ( field.IsGenericType && field.GetGenericTypeDefinition() == typeof( List<> ) ) {
                        Type itemType = field.GetGenericArguments()[0];
                        if ( itemType == typeof( NMSType ) )
                            return DeserializeGenericList( reader, templatePosition, parent );
                        else {
                            // todo: get rid of this nastiness
                            MethodInfo method = typeof( MbinSerializer ).GetMethod( "DeserializeList", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic )
                                                         .MakeGenericMethod( new Type[] { itemType } );
                            var list = method.Invoke( null, new object[] { reader, fieldInfo, settings, templatePosition, parent } );
                            return list;
                        }
                    }
                    return null;
                case "Component":
                    reader.Align( 8, templatePosition );
                    long startPos = reader.BaseStream.Position;
                    long offset = reader.ReadInt64();
                    string name = reader.ReadString( Encoding.ASCII, 0x40, true );
                    long endPos = reader.BaseStream.Position;
                    NMSType val = new EmptyNode();
                    if ( offset != 0 && !String.IsNullOrEmpty( name ) ) {
                        reader.BaseStream.Position = startPos + offset;
                        val = DeserializeBinaryTemplate( reader, name );
                        if ( val == null ) throw new DeserializeTemplateException( name );
                    }
                    reader.BaseStream.Position = endPos;
                    return val;
                default:
                    if ( fieldType == "Colour" ) { // unsure if this is needed?
                        reader.Align( 0x10, templatePosition );
                    } else if ( fieldType == "VariableStringSize" || fieldType == "GcRewardProduct" ) { // TODO: I don't think we need to specify GcRewardProduct here explicitly...
                        reader.Align( 0x4, templatePosition );
                    }

                    // todo: align for VariableSizeString?
                    if ( field.IsEnum ) {
                        reader.Align( 4, templatePosition );
                        return fieldType == "Int32" ? (object) reader.ReadInt32() : (object) reader.ReadUInt32();

                    } else if ( field.IsArray ) {
                        var arrayType = field.GetElementType();
                        Array array = Array.CreateInstance( arrayType, settings.Size );
                        for ( int i = 0; i < settings.Size; ++i ) {
                            array.SetValue( DeserializeValue( reader, field.GetElementType(), settings, templatePosition, fieldInfo, parent ), i );
                        }
                        return array;

                    } else {
                        reader.Align( 0x4, templatePosition );
                        return DeserializeBinaryTemplate( reader, fieldType );
                    }
            }
        }

        private static List<NMSType> DeserializeGenericList( BinaryReader reader, long templateStartOffset, NMSType parent ) {
            long listPosition = reader.BaseStream.Position;
            NMSTemplate.DebugLogTemplate( $"[G Start]\t0x{listPosition:X}" );

            long templateNamesOffset = reader.ReadInt64();
            int numTemplates = reader.ReadInt32();
            uint listMagic = reader.ReadUInt32();
            if ( (listMagic & 0xFF) != 1 ) throw new InvalidListException( listMagic );

            long listEndPosition = reader.BaseStream.Position;

            reader.BaseStream.Position = listPosition + templateNamesOffset;
            var list = new List<NMSType>();
            if ( numTemplates > 0 ) {
                //Dictionary<long, string> templates = new Dictionary<long, string>();
                List<KeyValuePair<long, String>> templates = new List<KeyValuePair<long, String>>();
                for ( int i = 0; i < numTemplates; i++ ) {
                    long nameOffset = reader.BaseStream.Position;
                    long templateOffset = reader.ReadInt64();
                    var name = reader.ReadString( Encoding.UTF8, 0x40, true );

                    if ( templateOffset == 0 ) {
                        // sometimes there are lists which have n values, but less than n actual structs in them. We replace the empty thing with EmptyNode
                        templates.Add( new KeyValuePair<long, string>( nameOffset + templateOffset, "EmptyNode" ) );
                    } else {
                        templates.Add( new KeyValuePair<long, string>( nameOffset + templateOffset, name ) );
                    }
                }

                long pos = reader.BaseStream.Position;

                foreach ( KeyValuePair<long, string> templateInfo in templates ) {
                    reader.BaseStream.Position = templateInfo.Key;
                    var template = DeserializeBinaryTemplate( reader, templateInfo.Value );
                    if ( template == null ) throw new DeserializeTemplateException( templateInfo.Value );

                    list.Add( template );
                }
            }

            NMSTemplate.DebugLogTemplate( $"[G End]\t0x{listEndPosition:X}" );
            reader.BaseStream.Position = listEndPosition;
            reader.Align( 0x8, 0 );

            return list;
        }

        private static List<T> DeserializeList<T>( BinaryReader reader, FieldInfo field, NMSAttribute settings, long templateStartOffset, NMSType parent ) {
            long listPosition = reader.BaseStream.Position;
            NMSTemplate.DebugLogTemplate( $"[L Start]\t0x{listPosition:X}" );

            long listStartOffset = reader.ReadInt64();
            int numEntries = reader.ReadInt32();
            uint listMagic = reader.ReadUInt32();
            if ( (listMagic & 0xFF) != 1 ) throw new InvalidListException( listMagic );

            long listEndPosition = reader.BaseStream.Position;

            reader.BaseStream.Position = listPosition + listStartOffset;
            var list = new List<T>();
            for ( int i = 0; i < numEntries; i++ ) {
                // todo: get rid of DeserializeGenericList? this seems like it would work fine with List<NMSType>
                var template = DeserializeValue( reader, field.FieldType.GetGenericArguments()[0], settings, templateStartOffset, field, parent );
                if ( template == null ) throw new DeserializeTypeException( typeof( T ), $"{field.Name}: item {i}" );

                if ( template.GetType().IsSubclassOf( typeof( NMSType ) ) ) FinishDeserialize( (NMSType) template );

                list.Add( (T) template );
            }

            NMSTemplate.DebugLogTemplate( $"[L End]\t0x{listEndPosition:X}" );
            reader.BaseStream.Position = listEndPosition;
            reader.Align( 0x8, 0 );

            return list;
        }

        // func thats run after template is deserialized, can be used for checks etc
        internal static void FinishDeserialize( NMSType template ) {
#if DEBUG
            // check enums are valid
            var fields = NMSTemplate.GetOrderedFields( template.GetType() );
            foreach ( var field in fields ) {
                var fieldType = field.FieldType.Name;
                if ( fieldType != "Int32" ) continue;

                var valuesMethod = template.GetType().GetMethod( field.Name + "Values" ); // if we have an "xxxValues()" method in the struct, use that to get the value name
                if ( valuesMethod == null ) continue;

                int value = (int) field.GetValue( template );
                if ( value == -1 ) continue;

                string[] values = (string[]) valuesMethod.Invoke( template, null );
                try {
                    string valueStr = values[(int) value];
                } catch ( IndexOutOfRangeException ) {
                    throw new IndexOutOfRangeException( "Values index out of Range. Struct: " + template.GetType() + " field: " + field.Name );
                }

            }
#endif
        }

    internal static byte[] SerializeBytes( NMSType template ) {
            using ( var stream = new MemoryStream() )
            using ( var writer = new BinaryWriter( stream, Encoding.ASCII ) ) {
                var additionalData = new List<Tuple<long, object>>();

                UInt32 listEnding = 0xAAAAAA01;

                var type = template.GetType();
                if ( type == typeof( NMS.Toolkit.TkAnimMetadata ) ) {
                    listEnding = 0xFEFEFE01;
                } else if ( type == typeof( NMS.Toolkit.TkGeometryStreamData ) || type == typeof( NMS.Toolkit.TkGeometryData ) ) {
                    listEnding = 0x00000001;
                }

                int i = 0;
                // write primary template + any embedded templates
                AppendToWriter( template, writer, ref additionalData, ref i, type, listEnding );

                // now write values of lists etc
                for ( i = 0; i < additionalData.Count; i++ ) {
                    var data = additionalData[i];
                    //DebugLog($"Current i: {i}");
                    //DebugLog(data);
                    //writer.BaseStream.Position = additionalDataOffset; // addtDataOffset gets updated by child templates
                    long origPos = writer.BaseStream.Position;

                    // get the custom alignment value from the class if it has one
                    // If the class has no alignment value associated with it, just set as default value of 4
                    NMSAttribute settings = data.Item2?.GetType().GetCustomAttribute<NMSAttribute>();
                    int alignment = settings?.Alignment ?? 0x4;

                    if ( data.Item2 != null ) {
                        // we don't want alignment if the data is purely byte[] data
                        if ( data.Item2.GetType() != typeof( byte[] ) ) {
                            // if we have an empty list we do not want to do alignment otherwise it can put off other things
                            if ( data.Item2.GetType().IsGenericType && data.Item2.GetType().GetGenericTypeDefinition() == typeof( List<> ) ) {
                                IList lst = (IList) data.Item2;
                                if ( lst.Count != 0 ) writer.Align( alignment, 0 );
                            } else {
                                writer.Align( alignment, 0 );
                            }
                        }

                        if ( data.Item2.GetType() == typeof( NMS.VariableSizeString ) ) {
                            //DebugLog(alignment);
                            writer.BaseStream.Position = origPos; // no alignment for dynamicstrings

                            var str = (NMS.VariableSizeString) data.Item2;

                            var stringPos = writer.BaseStream.Position;
                            writer.WriteString( str.Value, Encoding.UTF8, null, true );
                            var stringEndPos = writer.BaseStream.Position;

                            writer.BaseStream.Position = data.Item1;
                            writer.Write( stringPos - data.Item1 );
                            writer.Write( (Int32) (stringEndPos - stringPos) );
                            writer.Write( listEnding );

                            writer.BaseStream.Position = stringEndPos;
                        } else if ( data.Item2.GetType().IsSubclassOf( typeof( NMSType ) ) ) {
                            var pos = writer.BaseStream.Position;
                            var template2 = (NMSType) data.Item2;
                            int i2 = i + 1;
                            AppendToWriter( template2, writer, ref additionalData, ref i2, type, listEnding );
                            var endPos = writer.BaseStream.Position;
                            writer.BaseStream.Position = data.Item1;
                            writer.Write( pos - data.Item1 );
                            writer.WriteString( "c" + template2.GetType().Name, Encoding.UTF8, 0x40 );
                            writer.BaseStream.Position = endPos;
                        } else if ( data.Item2.GetType().IsGenericType && data.Item2.GetType().GetGenericTypeDefinition() == typeof( List<> ) ) {
                            // this will serialise a dynamic length list of either a generic type, or a specific type
                            Type itemType = data.Item2.GetType().GetGenericArguments()[0];
                            if ( itemType == typeof( NMSType ) ) {
                                // this is serialising a list of generic type
                                SerializeGenericList( template, writer, (IList) data.Item2, data.Item1, ref additionalData, i + 1, listEnding );
                            } else {
                                // this is serialising a list if a particular type
                                SerializeList( template, writer, (IList) data.Item2, data.Item1, ref additionalData, i + 1, listEnding );
                            }
                        } else if ( data.Item2.GetType() == typeof( byte[] ) ) {
                            // write the offset in the list header
                            long dataPosition = writer.BaseStream.Position;
                            writer.BaseStream.Position = data.Item1;
                            writer.Write( dataPosition - data.Item1 );
                            writer.BaseStream.Position = dataPosition;
                            SerializeValue( template, writer, data.Item2.GetType(), data.Item2, settings, null, 0, ref additionalData, ref i );     // passing i here *should* be fine as we will only be writing bytes which can't affect i
                        } else {
                            throw new UnknownTypeException( data.Item2.GetType() );
                        }
                    } else {
                        writer.Align( alignment, 0 );
                        SerializeList( template, writer, new List<int>(), data.Item1, ref additionalData, i + 1, listEnding );  // pass an empty list. Data type doesn't matter...
                    }

                }

                return stream.ToArray();
            }
        }

        internal static void AppendToWriter( NMSType template, BinaryWriter writer, ref List<Tuple<long, object>> additionalData, ref int addtDataIndex, Type parent, UInt32 listEnding = 0xAAAAAA01 ) {
            long templatePosition = writer.BaseStream.Position;
            NMSTemplate.DebugLogTemplate( $"[A] {template.GetType().Name}\t0x{templatePosition:X4}\t{parent.Name}" );
            var type = template.GetType();
            var fields = NMSTemplate.GetOrderedFields( type );

            // todo: remove struct length?? Not needed any more I think...
            NMSAttribute attribute = type.GetCustomAttribute<NMSAttribute>();
            // If the class has no size associate with it, just ignore it
            int structLength = attribute?.Size ?? 0;

            //var entryOffsetNamePairs = new Dictionary<long, string>();
            //List<KeyValuePair<long, String>> entryOffsetNamePairs = new List<KeyValuePair<long, String>>();

            if ( type.Name != "EmptyNode" ) {
                foreach ( var field in fields ) {
                    var fieldAddr = writer.BaseStream.Position - templatePosition;      // location of the data within the struct
                    //Logger.LogDebug($"fieldAddr: 0x{fieldAddr:X}, templatePos: 0x{templatePosition:X}, name: {field.FieldType.Name}, value: {field.GetValue(this)}");
                    NMSAttribute settings = field.GetCustomAttribute<NMSAttribute>();
                    var fieldData = field.GetValue( template );
                    SerializeValue( template, writer, field.FieldType, fieldData, settings, field, templatePosition, ref additionalData, ref addtDataIndex, structLength, listEnding );
                }
            } else {
                SerializeValue( template, writer, type, null, null, null, templatePosition, ref additionalData, ref addtDataIndex, structLength, listEnding );
            }
        }

        private static void SerializeValue( NMSType template, BinaryWriter writer, Type fieldType, object fieldData, NMSAttribute settings, FieldInfo field, long startStructPos, ref List<Tuple<long, object>> additionalData, ref int addtDataIndex, int structLength = 0, UInt32 listEnding = 0xAAAAAA01 ) {
            NMSTemplate.DebugLogTemplate( $"[V] {field?.DeclaringType.Name}.{field?.Name}\t{fieldType.Name}\t{additionalData?.Count ?? 0}\t{addtDataIndex}" );

            if ( (template as NMS.ISerialize)?.OnSerialize( writer, fieldType, fieldData, settings, field, ref additionalData, ref addtDataIndex ) ?? false ) return;

            if ( settings?.DefaultValue != null ) fieldData = settings.DefaultValue;
            switch ( fieldType.Name ) {
                case "String":
                case "Byte[]":
                    int size = settings?.Size ?? 0;
                    byte stringPadding = settings?.Padding ?? 0;
                    bool ignore = settings?.Ignore ?? true;
                    MarshalAsAttribute legacySettings = field?.GetCustomAttribute<MarshalAsAttribute>();
                    if ( legacySettings != null ) size = legacySettings.SizeConst;

                    if ( fieldType.Name == "String" ) {
                        writer.WriteString( (string) fieldData, Encoding.UTF8, size, false, stringPadding );
                    } else {
                        byte[] bytes = (byte[]) fieldData;
                        if ( ignore != true )//(bytes.Length != 0)
                        {
                            size = bytes.Length;
                        }
                        Array.Resize( ref bytes, size );
                        writer.Write( bytes );
                    }
                    break;
                case "Byte":
                    writer.Write( (Byte) fieldData );
                    break;
                case "Single":
                    writer.Align( 4, startStructPos );
                    writer.Write( (Single) fieldData );
                    break;
                case "Boolean":
                    var value = (bool) fieldData;
                    writer.Write( value ? (byte) 1 : (byte) 0 );
                    break;
                case "Int16":
                case "UInt16":
                    writer.Align( 2, startStructPos );
                    if ( fieldType.Name == "Int16" ) {
                        writer.Write( (Int16) fieldData );
                    } else {
                        writer.Write( (UInt16) fieldData );
                    }
                    break;
                case "Int32":
                case "UInt32":
                    writer.Align( 4, startStructPos );
                    if ( fieldType.Name == "Int32" ) {
                        writer.Write( (Int32) fieldData );
                    } else {
                        writer.Write( (UInt32) fieldData );
                    }
                    break;
                case "Int64":
                case "UInt64":
                    writer.Align( 8, startStructPos );
                    if ( fieldType.Name == "Int64" ) {
                        writer.Write( (Int64) fieldData );
                    } else {
                        writer.Write( (UInt64) fieldData );
                    }
                    break;
                case "List`1":
                    writer.Align( 8, startStructPos );
                    if ( field != null && field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof( List<> ) ) {
                        // write empty list header
                        long listPos = writer.BaseStream.Position;
                        writer.Write( (Int64) 0 ); // listPosition
                        writer.Write( (Int32) 0 ); // listCount
                        writer.Write( listEnding );
                        IList listData = (IList) fieldData;
                        if ( listData.Count != 0 ) {
                            var data = new Tuple<long, object>( listPos, listData );
                            if ( addtDataIndex >= additionalData.Count ) {
                                additionalData.Add( data );
                            } else {
                                additionalData.Insert( addtDataIndex, data );
                            }
                            addtDataIndex++;
                        }

                    }

                    break;
                case "EmptyNode":
                    break;

                case "Component":
                    writer.Align( 8, startStructPos );
                    long refPos = writer.BaseStream.Position;

                    template = (NMSType) fieldData;
                    if ( template == null || template.GetType().Name == "EmptyNode" ) {
                        writer.Write( (Int64) 0 ); // listPosition
                        writer.WriteString( "", Encoding.UTF8, 0x40 );
                    } else {
                        writer.Write( (Int64) 0 );      // default value to be overridden later anyway
                        writer.WriteString( "c" + template.GetType().Name, Encoding.UTF8, 0x40 );
                        var data = new Tuple<long, object>( refPos, template );
                        if ( addtDataIndex >= additionalData.Count ) {
                            additionalData.Add( data );
                        } else {
                            additionalData.Insert( addtDataIndex, data );
                        }
                        addtDataIndex++;
                    }
                    break;
                // TODO: remove
                case "Dictionary`2":
                    // have something defined so that it just ignores it
                    break;
                default:
                    if ( fieldType.Name == "Colour" ) writer.Align( 0x10, startStructPos ); // TODO: make an attribute

                    // todo: align for VariableSizeString?
                    if ( fieldType.Name == "VariableSizeString" ) {
                        // write empty DynamicString header
                        long fieldPos = writer.BaseStream.Position;
                        writer.Write( (Int64) 0 ); // listPosition
                        writer.Write( (Int32) 0 ); // listCount
                        writer.Write( listEnding );

                        var fieldValue = (NMS.VariableSizeString) fieldData;
                        additionalData.Insert( addtDataIndex++, new Tuple<long, object>( fieldPos, fieldValue ) );

                    } else if ( fieldType.IsArray ) {
                        var arrayType = fieldType.GetElementType();
                        Array array = (Array) fieldData;
                        if ( array == null ) array = Array.CreateInstance( arrayType, (int) settings.Size );

                        foreach ( var obj in array ) {
                            long fieldPos = writer.BaseStream.Position;
                            var realObj = obj;
                            if ( realObj == null ) realObj = Activator.CreateInstance( arrayType );

                            SerializeValue( template, writer, realObj.GetType(), realObj, settings, field, fieldPos, ref additionalData, ref addtDataIndex, structLength, listEnding );
                        }
                    } else if ( fieldType.IsEnum ) {
                        writer.Align( 4, startStructPos );
                        writer.Write( (int) Enum.Parse( field.FieldType, fieldData.ToString() ) );

                    } else if ( fieldType.IsSubclassOf( typeof( NMSType ) ) ) {
                        int alignment = settings?.Alignment ?? 0x4;     // this isn't 0x10 for Colour's??
                        writer.Align( alignment, startStructPos );
                        var realData = (NMSType) fieldData;
                        if ( realData == null ) realData = (NMSType) Activator.CreateInstance( fieldType );
                        AppendToWriter( realData, writer, ref additionalData, ref addtDataIndex, template.GetType(), listEnding );

                    } else {
                        throw new UnknownTypeException( fieldType, field?.Name );
                    }
                    break;
            }
        }

        // This serialises a List of GameComponents
        private static void SerializeGenericList( NMSType template, BinaryWriter writer, IList list, long listHeaderPosition, ref List<Tuple<long, object>> additionalData, int addtDataIndex, UInt32 listEnding ) {
            writer.Align( 0x8, 0 );       // Make sure that all c~ names are offset at 0x8.     // make rel to listHeaderPosition?
            long listPosition = writer.BaseStream.Position;

            NMSTemplate.DebugLogTemplate( $"[G Start] {template.GetType().Name}\t{$"0x{listPosition:X},",-10}\t{$"0x{listHeaderPosition:X},",-10}\t{list.Count}" );

            writer.BaseStream.Position = listHeaderPosition;

            // write the list header into the template
            if ( list.Count > 0 ) {
                //DebugLog($"SerializeList start 0x{listPosition:X}, header 0x{listHeaderPosition:X}");
                writer.Write( listPosition - listHeaderPosition );
            } else {
                writer.Write( (long) 0 ); // lists with 0 entries have offset set to 0
            }

            writer.Write( (Int32) list.Count );
            writer.Write( listEnding );

            // reserve space for list offsets+names
            writer.BaseStream.Position = listPosition;
            writer.Write( new byte[list.Count * 0x48] );              // this seems to need to be reserved even if there are no elements (check?)

            int addtDataIndexThis = 0;

            //var entryOffsetNamePairs = new Dictionary<long, string>();
            List<KeyValuePair<long, String>> entryOffsetNamePairs = new List<KeyValuePair<long, String>>();
            foreach ( var entry in list ) {
                int alignment = entry.GetType().GetCustomAttribute<NMSAttribute>()?.Alignment ?? 0x8;       // this will generally return 4 because it is the default...

                writer.Align( 8, 0 );
                //Logger.LogDebug($"pos 0x{writer.BaseStream.Position:X}");
                //Logger.LogDebug(entry.GetType().Name);
                entryOffsetNamePairs.Add( new KeyValuePair<long, string>( writer.BaseStream.Position, entry.GetType().Name ) );

                var templateEntry = (NMSType) entry;
                var listObjects = new List<Tuple<long, object>>();     // new list of objects so that this data is serialised first
                var addtData = new Dictionary<long, object>();
                NMSTemplate.DebugLogTemplate( $"[G] {template.GetType().Name}\t0x{writer.BaseStream.Position:X}" );
                // pass the new listObject object in place of additionalData so that this branch is serialised before the whole layer
                AppendToWriter( templateEntry, writer, ref listObjects, ref addtDataIndexThis, template.GetType() );
                for ( int i = 0; i < listObjects.Count; i++ ) {
                    var data = listObjects[i];
                    //writer.BaseStream.Position = additionalDataOffset; // addtDataOffset gets updated by child templates
                    writer.Align( 0x8, 0 ); // todo: check if this alignment is correct
                    long origPos = writer.BaseStream.Position;
                    if ( data.Item2.GetType().IsGenericType && data.Item2.GetType().GetGenericTypeDefinition() == typeof( List<> ) ) {
                        //DebugLog("blahblah");
                        Type itemType = data.Item2.GetType().GetGenericArguments()[0];

                        if ( itemType == typeof( NMSType ) ) {
                            SerializeGenericList( template, writer, (IList) data.Item2, data.Item1, ref listObjects, i + 1, listEnding );
                        } else {
                            SerializeList( template, writer, (IList) data.Item2, data.Item1, ref listObjects, i + 1, listEnding );
                        }
                    } else {
                        //DebugLog("this is it!!!");
                        //DebugLog($"0x{origPos:X}");
                        // first, write the correct offset at the correct location
                        long headerPos = data.Item1;
                        writer.BaseStream.Position = headerPos;
                        long offset = origPos - headerPos;
                        writer.Write( offset );
                        writer.BaseStream.Position = origPos;
                        var GenericObject = data.Item2;
                        int newDataIndex = i + 1;
                        SerializeValue( template, writer, GenericObject.GetType(), GenericObject, null, null, origPos, ref listObjects, ref newDataIndex, 0, listEnding );
                    }
                }

            }

            long dataEndOffset = writer.BaseStream.Position;

            writer.BaseStream.Position = listPosition;
            foreach ( KeyValuePair<long, string> kvp in entryOffsetNamePairs ) {
                // Iterate through the list headers and write the correct data
                if ( kvp.Value != "EmptyNode" ) {
                    //long tempPos = writer.BaseStream.Position;
                    writer.Align( 0x8, 0 );
                    //long correction = writer.BaseStream.Position - tempPos;
                    // in this case, we have an actual non-empty header.
                    long position = writer.BaseStream.Position;
                    long offset = kvp.Key - position; // get offset of this entry from the current offset
                    writer.Write( offset );
                    //DebugLog(kvp.Value);
                    writer.WriteString( "c" + kvp.Value, Encoding.UTF8, 0x40 );
                    //DebugLog(kvp.Value);
                    //DebugLog(offset);
                } else {
                    // this is called when the header 0x48 bytes is empty because it is an empty node.
                    writer.WriteString( "", Encoding.UTF8, 0x48 );
                }
            }

            writer.BaseStream.Position = dataEndOffset;
        }

        private static void SerializeList( NMSType template, BinaryWriter writer, IList list, long listHeaderPosition, ref List<Tuple<long, object>> additionalData, int addtDataIndex, UInt32 listEnding = (UInt32) 0xAAAAAA01 ) {
            // first thing we want to do is align the writer with the location of the first element of the list
            if ( list.Count != 0 ) {
                // if the class has no alignment value associated with it, set a default value
                int alignment = alignment = list[0].GetType().GetCustomAttribute<NMSAttribute>()?.Alignment ?? 0x8;
                writer.Align( 8, 0 );
            }

            long listPosition = writer.BaseStream.Position;
            NMSTemplate.DebugLogTemplate( $"[L Start] {template.GetType().Name}\t{$"0x{listPosition:X},",-10}\t{$"0x{listHeaderPosition:X},",-10}\t{list.Count}" );

            writer.BaseStream.Position = listHeaderPosition;

            // write the list header into the template
            if ( list.Count > 0 ) {
                writer.Write( listPosition - listHeaderPosition );
            } else {
                writer.Write( (long) 0 ); // lists with 0 entries have offset set to 0
            }
            writer.Write( (Int32) list.Count );
            writer.Write( listEnding );       // this is where the 4bytes at the end of a list are written

            writer.BaseStream.Position = listPosition;

            var listObjects = new List<Tuple<long, object>>();     // new list of objects so that this data is serialised first

            int addtDataIndexThis = addtDataIndex;

            foreach ( var entry in list ) {
                NMSTemplate.DebugLogTemplate( $"[L] {entry.GetType().Name}\t0x{writer.BaseStream.Position:X}" );
                SerializeValue( template, writer, entry.GetType(), entry, null, null, listPosition, ref additionalData, ref addtDataIndexThis, 0, listEnding );
            }

            if ( list.GetType().GetGenericArguments()[0] == typeof( NMS.Toolkit.TkAnimNodeFrameData ) ) {
                writer.Write( 0xFEFEFEFEFEFEFEFE );
            }
        }

    }

}
