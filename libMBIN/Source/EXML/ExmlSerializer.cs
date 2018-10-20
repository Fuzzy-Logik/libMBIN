using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace libMBIN.EXML {

    using NMS;

    internal static class ExmlSerializer {

        // this code is run to parse over the exml file and put it into a data structure that is processed by SerializeBytes() (I think...)
        internal static NMSType DeserializeEXml( ExmlBase xmlData ) {     // this is the inital code that is run when converting exml to mbin.
            NMSType template = null;

            //DebugLog(xmlData.Name);
            //DebugLog(xmlData.GetType().ToString());

            if ( xmlData.GetType() == typeof( ExmlData ) ) {
                template = NMSTemplate.TemplateFromName( ((ExmlData) xmlData).Template );
            } else if ( xmlData.GetType() == typeof( ExmlProperty ) ) {
                template = NMSTemplate.TemplateFromName( ((ExmlProperty) xmlData).Value.Replace( ".xml", "" ) );
            } else if ( xmlData.GetType() == typeof( ExmlMeta ) ) {
                NMSTemplate.DebugLogComment( ((ExmlMeta) xmlData).Comment );
            }

            /*
            DebugLog("Getting types");
            foreach (var property in xmlData.Elements)
            {
                DebugLog(property.GetType());
            }*/

            if ( template == null ) return null;

            Type templateType = template.GetType();
            var templateFields = NMSTemplate.GetOrderedFields( templateType );

            foreach ( var templateField in templateFields ) {
                // check to see if the object has a default value in the struct
                NMSAttribute settings = templateField.GetCustomAttribute<NMSAttribute>();
                if ( settings?.DefaultValue != null ) templateField.SetValue( template, settings.DefaultValue );
            }

            foreach ( var xmlElement in xmlData.Elements ) {
                if ( xmlElement.GetType() == typeof( ExmlProperty ) ) {
                    ExmlProperty xmlProperty = (ExmlProperty) xmlElement;
                    FieldInfo field = templateType.GetField( xmlProperty.Name );
                    object fieldValue = null;
                    NMSTemplate.DebugLogPropertyName( xmlProperty.Name );
                    if ( field.FieldType == typeof( NMSType ) || field.FieldType.IsSubclassOf( typeof( NMSType ) ) ) {
                        fieldValue = DeserializeEXml( xmlProperty );
                    } else {
                        Type fieldType = field.FieldType;
                        NMSAttribute settings = field.GetCustomAttribute<NMSAttribute>();
                        fieldValue = DeserializeEXmlValue( template, fieldType, field, xmlProperty, templateType, settings );
                    }
                    field.SetValue( template, fieldValue );
                } else if ( xmlElement.GetType() == typeof( ExmlData ) ) {
                    ExmlData innerXmlData = (ExmlData) xmlElement;
                    FieldInfo field = templateType.GetField( innerXmlData.Name );
                    NMSType innerTemplate = DeserializeEXml( innerXmlData );
                    field.SetValue( template, innerTemplate );
                } else if ( xmlElement.GetType() == typeof( ExmlMeta ) ) {
                    ExmlMeta xmlMeta = (ExmlMeta) xmlElement;
                    string comment = xmlMeta.Comment;
                    NMSTemplate.DebugLogComment( comment );
                }
            }
            /*
            foreach ( var xmlProperty in xmlData.Elements.OfType<EXmlProperty>() ) {
                FieldInfo field = templateType.GetField(xmlProperty.Name);
                object fieldValue = null;
                if (field.FieldType == typeof(NMSType) || field.FieldType.IsSubClassOf( typeof( NMSType ) ) ) {
                    fieldValue = DeserializeEXml(xmlProperty);
                } else {
                    Type fieldType = field.FieldType;
                    NMSAttribute settings = field.GetCustomAttribute<NMSAttribute>();
                    fieldValue = DeserializeEXmlValue(template, fieldType, field, xmlProperty, templateType, settings);
                }
                field.SetValue(template, fieldValue);
            }

            foreach ( EXmlData innerXmlData in xmlData.Elements.OfType<EXmlData>() ) {
                FieldInfo field = templateType.GetField(innerXmlData.Name);
                NMSType innerTemplate = DeserializeEXml(innerXmlData);
                field.SetValue(template, innerTemplate);
            }

            foreach ( var xmlProperty in xmlData.Elements.OfType<EXmlMeta>() ) {
                DebugLog("Hello!!!");
                string comment = xmlProperty.Comment;
                DebugLog(comment);
            }
            */

            return template;
        }

        private static object DeserializeEXmlValue( NMSType template, Type fieldType, FieldInfo field, ExmlProperty xmlProperty, Type templateType, NMSAttribute settings ) {
            switch ( fieldType.Name ) {
                case "String":
                    return xmlProperty.Value;
                case "Single":
                    return float.Parse( xmlProperty.Value );
                case "Boolean":
                    return bool.Parse( xmlProperty.Value );
                case "Int16":
                    return short.Parse( xmlProperty.Value );
                case "UInt16":
                    return ushort.Parse( xmlProperty.Value );
                case "Int32":
                    var valuesMethod = templateType.GetMethod( field.Name + "Values" );
                    // TODO: remove this dictionary stuff
                    var dictData = templateType.GetMethod( field.Name + "Dict" );

                    if ( dictData != null ) {
                        if ( String.IsNullOrEmpty( xmlProperty.Value ) ) return -1;

                        Dictionary<int, string> dataDict = (Dictionary<int, string>) dictData.Invoke( template, null );
                        int key = dataDict.Where( kvp => kvp.Value == xmlProperty.Value ).Select( kvp => kvp.Key ).FirstOrDefault();
                        return key;
                    }

                    if ( valuesMethod != null ) {
                        if ( String.IsNullOrEmpty( xmlProperty.Value ) ) return -1;

                        string[] values = (string[]) valuesMethod.Invoke( template, null );
                        return Array.FindIndex( values, v => v == xmlProperty.Value );
                        //} else if (settings?.EnumValue != null) {
                        //    if (String.IsNullOrEmpty(xmlProperty.Value)) return -1;
                        //    return Array.FindIndex(settings.EnumValue, v => v == xmlProperty.Value);
                    } else {
                        return int.Parse( xmlProperty.Value );
                    }
                case "UInt32":
                    return uint.Parse( xmlProperty.Value );
                case "Int64":
                    return long.Parse( xmlProperty.Value );
                case "UInt64":
                    return ulong.Parse( xmlProperty.Value );
                case "Byte[]":
                    return xmlProperty.Value == null ? null : Convert.FromBase64String( xmlProperty.Value );
                case "List`1":
                    Type elementType = fieldType.GetGenericArguments()[0];
                    Type listType = typeof( List<> ).MakeGenericType( elementType );
                    IList list = (IList) Activator.CreateInstance( listType );
                    foreach ( var innerXmlData in xmlProperty.Elements ) // child templates
                    {
                        object element = null;

                        var type = innerXmlData.GetType();
                        var data = innerXmlData as ExmlProperty;
                        type = (data?.Value.EndsWith( ".xml" ) ?? false) ? typeof( ExmlData ) : type;

                        if ( type == typeof( ExmlData ) ) {
                            element = DeserializeEXml( innerXmlData ); // child template if <Data> tag or <Property> tag with value ending in .xml (todo: better way of finding <Property> child templates)
                        } else if ( type == typeof( ExmlProperty ) ) {
                            element = DeserializeEXmlValue( template, elementType, field, (ExmlProperty) innerXmlData, templateType, settings );
                        } else if ( type == typeof( ExmlMeta ) ) {
                            NMSTemplate.DebugLogComment( ((ExmlMeta) innerXmlData).Comment );
                        }

                        if ( element == null ) throw new TemplateException( "element == null ??!" );

                        list.Add( element );
                    }
                    return list;
                default:
                    if ( field.FieldType.IsArray && field.FieldType.GetElementType().IsSubclassOf( typeof( NMSType ) ) ) {
                        Array array = Array.CreateInstance( field.FieldType.GetElementType(), settings.Size );
                        //var data = xmlProperty.Elements.OfType<EXmlProperty>().ToList();
                        List<ExmlBase> data = xmlProperty.Elements.ToList();
                        int numMeta = 0;
                        foreach ( ExmlBase entry in data ) {
                            if ( entry.GetType() == typeof( ExmlMeta ) ) numMeta++;
                        }

                        if ( data.Count - numMeta != settings.Size ) {
                            // todo: add a comment in the XML to indicate arrays (+ their size), also need to do the same for showing valid enum values
                            throw new ArraySizeException( field.Name, (data.Count - numMeta), settings.Size );
                        }

                        for ( int i = 0; i < data.Count; ++i ) {
                            if ( data[i].GetType() == typeof( ExmlProperty ) ) {
                                NMSType element = DeserializeEXml( data[i] );
                                array.SetValue( element, i - numMeta );
                            } else if ( data[i].GetType() == typeof( ExmlMeta ) ) {
                                NMSTemplate.DebugLogComment( ((ExmlMeta) data[i]).Comment );     // don't need to worry about nummeta here since it is already counted above...
                            }
                        }

                        return array;
                    } else if ( field.FieldType.IsArray ) {
                        Array array = Array.CreateInstance( field.FieldType.GetElementType(), settings.Size );
                        //List<EXmlProperty> data = xmlProperty.Elements.OfType<EXmlProperty>().ToList();
                        List<ExmlBase> data = xmlProperty.Elements.ToList();
                        int numMeta = 0;
                        for ( int i = 0; i < data.Count; ++i ) {
                            if ( data[i].GetType() == typeof( ExmlProperty ) ) {
                                object element = DeserializeEXmlValue( template, field.FieldType.GetElementType(), field, (ExmlProperty) data[i], templateType, settings );
                                array.SetValue( element, i - numMeta );
                            } else if ( data[i].GetType() == typeof( ExmlMeta ) ) {
                                NMSTemplate.DebugLogComment( ((ExmlMeta) data[i]).Comment );
                                numMeta += 1;           // increment so that the actual data is still placed at the right spot
                            }
                        }

                        return array;
                    } else if ( field.FieldType.IsEnum ) {
                        return (int) Enum.Parse( field.FieldType, xmlProperty.Value );
                    } else {
                        return fieldType.IsValueType ? Activator.CreateInstance( fieldType ) : null;
                    }
            }
        }

        private static ExmlBase SerializeEXmlValue( NMSType template, Type fieldType, FieldInfo field, NMSAttribute settings, object value ) {
            string t = fieldType.Name;
            int i = 0;
            string valueString = String.Empty;

            if ( settings?.DefaultValue != null ) {
                value = settings.DefaultValue;
            }

            switch ( fieldType.Name ) {
                case "String":
                case "Boolean":
                case "Int16":
                case "UInt16":
                case "Int32":
                case "UInt32":
                case "Int64":
                case "UInt64":
                    valueString = value?.ToString() ?? "";
                    if ( fieldType.Name != "Int32" )
                        break;

                    var valuesMethod = template.GetType().GetMethod( field.Name + "Values" ); // if we have an "xxxValues()" method in the struct, use that to get the value name
                    var dictData = template.GetType().GetMethod( field.Name + "Dict" );
                    if ( dictData != null ) {
                        if ( ((int) value) == -1 )
                            valueString = "";
                        else {
                            Dictionary<int, string> dataDict = (Dictionary<int, string>) dictData.Invoke( template, null );
                            valueString = dataDict[(int) value];
                        }
                    }
                    if ( valuesMethod != null ) {
                        valueString = "";
                        if ( ((int) value) != -1 ) {
                            string[] values = (string[]) valuesMethod.Invoke( template, null );
                            valueString = values[(int) value];
                        }
                    }
                    break;
                case "Single":
                    valueString = ((float) value).ToString( System.Globalization.CultureInfo.InvariantCulture );
                    break;
                case "Double":
                    valueString = ((double) value).ToString( System.Globalization.CultureInfo.InvariantCulture );
                    break;
                case "Byte[]":
                    valueString = value == null ? null : Convert.ToBase64String( (byte[]) value );
                    break;
                case "List`1":
                    var listType = field.FieldType.GetGenericArguments()[0];
                    ExmlProperty listProperty = new ExmlProperty {
                        Name = field.Name
                    };

                    IList items = (IList) value;
                    i = 0;
                    if ( items != null ) {
                        foreach ( var item in items ) {
                            ExmlBase data = SerializeEXmlValue( template, listType, field, settings, item );
                            if ( settings?.EnumValue != null ) {
                                data.Name = settings.EnumValue[i];
                                i++;
                            } else {
                                data.Name = null;
                            }

                            listProperty.Elements.Add( data );
                        }
                    }

                    return listProperty;
                case "Component":
                    if ( value != null ) {
                        template = (NMSType) value;

                        var templateXmlData = SerializeEXml( template, true );
                        templateXmlData.Name = field.Name;

                        return templateXmlData;
                    }
                    return null;
                default:
                    if ( fieldType.IsSubclassOf( typeof( NMSType ) ) ) {
                        if ( value is null ) {
                            template = NMSTemplate.TemplateFromName( fieldType.Name );
                        } else {
                            template = (NMSType) value;
                        }

                        var templateXmlData = SerializeEXml( template, true );
                        templateXmlData.Name = field.Name;

                        return templateXmlData;
                    } else if ( fieldType.IsArray ) {
                        var arrayType = field.FieldType.GetElementType();
                        ExmlProperty arrayProperty = new ExmlProperty {
                            Name = field.Name
                        };

                        Array array = (Array) value;
                        i = 0;
                        foreach ( var element in array ) {
                            ExmlBase data = SerializeEXmlValue( template, arrayType, field, settings, element );
                            if ( settings?.EnumValue != null ) {
                                data.Name = settings.EnumValue[i];
                                i++;
                            } else
                                data.Name = null;

                            arrayProperty.Elements.Add( data );
                        }

                        return arrayProperty;
                    } else if ( fieldType.IsEnum ) {
                        valueString = value?.ToString();
                        break;
                    } else {
                        throw new UnknownTypeException( field.FieldType, field.Name );
                    }
            }

            return new ExmlProperty {
                Name = field.Name,
                Value = valueString
            };
        }

        internal static ExmlBase SerializeEXml( NMSType template, bool isChildTemplate ) {
            Type type = template.GetType();
            ExmlBase xmlData = new ExmlProperty { Value = type.Name + ".xml" };

            if ( !isChildTemplate ) {
                xmlData = new ExmlData { Template = type.Name };
            }

            var fields = NMSTemplate.GetOrderedFields( type );

            foreach ( var field in fields ) {

                NMSAttribute settings = field.GetCustomAttribute<NMSAttribute>();
                if ( settings == null ) settings = new NMSAttribute();
                if ( settings.Ignore ) continue;

                xmlData.Elements.Add( SerializeEXmlValue( template, field.FieldType, field, settings, field.GetValue( template ) ) );
            }

            return xmlData;
        }

    }

}
