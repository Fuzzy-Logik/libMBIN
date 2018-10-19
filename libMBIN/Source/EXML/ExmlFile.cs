using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace libMBIN {

    using EXML;

    public static class ExmlFile {

        private static readonly XmlSerializer Serializer = new XmlSerializer( typeof( ExmlData ) );
        private static readonly XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces( new[] { new XmlQualifiedName( "", "" ) } );

        private static XmlReaderSettings readerSettings = new XmlReaderSettings();


        public static NMSTemplate ReadTemplate( string filePath ) {
            using ( var input = File.OpenRead( filePath ) ) {
                return ReadTemplateFromStream( input );
            }
        }

        public static NMSTemplate ReadTemplateFromString( string xml ) {
            var origCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            readerSettings.IgnoreComments = false;

            using ( var reader = new StringReader( xml ) )
            using ( var xmlReader = XmlReader.Create( reader, readerSettings ) ) {
                var template = ReadTemplateFromXmlReader( xmlReader );
                Thread.CurrentThread.CurrentCulture = origCulture;
                return template;
            }
        }

        public static NMSTemplate ReadTemplateFromStream( Stream input ) {
            var origCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            readerSettings.IgnoreComments = false;

            using ( var reader = XmlReader.Create( input, readerSettings ) ) {
                var template = ReadTemplateFromXmlReader( reader );
                Thread.CurrentThread.CurrentCulture = origCulture;
                return template;
            }
        }

        private static NMSTemplate ReadTemplateFromXmlReader( XmlReader reader ) {
            ExmlData root = (ExmlData) Serializer.Deserialize( reader );
            NMSTemplate rootTemplate = DeserializeEXML.DeserializeEXml( root );
            return rootTemplate;
        }

        public static ExmlData ReadExmlDataFromString( string xml ) {
            var origCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            readerSettings.IgnoreComments = false;

            using ( var reader = new StringReader( xml ) )
            using ( var xmlReader = XmlReader.Create( reader, readerSettings ) ) {
                var data = (ExmlData) Serializer.Deserialize( xmlReader );
                Thread.CurrentThread.CurrentCulture = origCulture;
                return data;
            }
        }

        public static string WriteTemplate( NMSTemplate template ) {
            var origCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var xmlSettings = new XmlWriterSettings {
                Indent = true,
                Encoding = Encoding.UTF8
            };

            using ( var stringWriter = new EncodedStringWriter( Encoding.UTF8 ) )
            using ( var xmlTextWriter = XmlWriter.Create( stringWriter, xmlSettings ) ) {
                string ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                xmlTextWriter.WriteComment( String.Format( "File created using MBINCompiler version ({0})", ver.Substring( 0, ver.Length - 2 ) ) );
                var data = SerializeEXML.SerializeEXml( template, false );
                Serializer.Serialize( xmlTextWriter, data, Namespaces );
                xmlTextWriter.Flush();

                var xmlData = stringWriter.GetStringBuilder().ToString();
                Thread.CurrentThread.CurrentCulture = origCulture;
                return xmlData;
            }
        }

        private sealed class EncodedStringWriter : StringWriter {

            public EncodedStringWriter( Encoding encoding ) {
                Encoding = encoding;
            }

            public override Encoding Encoding { get; }

        }

    }

}