// These defines require that the project is set to the 'Debug' configuration, not Release.
// They will always be disabled/ignored in Release builds.

// Uncomment to enable debug logging of the template de/serialization.
//#define DEBUG_TEMPLATE

// Uncomment to enable debug logging of XML comments
//#define DEBUG_COMMENTS

// Uncomment to enable debug logging of MBIN field names
//#define DEBUG_FIELD_NAMES

// Uncomment to enable debug logging of XML property names
//#define DEBUG_PROPERTY_NAMES


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace libMBIN {

    public class NMSTemplate {

        internal static readonly Dictionary<string, Type> NMSTemplateMap = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where( t => t.IsSubclassOf( typeof( NMSTemplate ) ) )
                .ToDictionary( t => t.Name );

        #region DebugLog
        // Conditionally compile methods for Release optimization.
        //
        // DEBUG is automatically defined if the project is set to the 'Debug' build configuration.
        // If the project is set to the 'Release' configuration, then DEBUG will not be defined.
        // 
        // Use the NMSTEMPLATE_* defines at the top of this file to enable/disable debug logging.

        // TODO: static could be problematic for threading?
        internal static bool isDebugLogTemplateEnabled = true;

        [Conditional( "DEBUG" )]
        internal static void DebugLogTemplate( string msg ) {
            #if DEBUG_TEMPLATE
                if (isDebugLogTemplateEnabled) Logger.LogDebug( msg );
            #endif
        }

        [Conditional( "DEBUG" )]
        internal static void DebugLogComment( string msg ) {
            #if DEBUG_COMMENTS
                Logger.LogDebug( msg );
            #endif
        }

        [Conditional( "DEBUG" )]
        internal static void DebugLogFieldName( string msg ) {
            #if DEBUG_FIELD_NAMES
                Logger.LogDebug( msg );
            #endif
        }

        [Conditional( "DEBUG" )]
        internal static void DebugLogPropertyName( string msg ) {
            #if DEBUG_PROPERTY_NAMES
                Logger.LogDebug( msg );
            #endif
        }

        #endregion

        internal static NMSTemplate TemplateFromName( string templateName ) {
            Type type;
            if ( !NMSTemplateMap.TryGetValue( templateName, out type ) ) return null; // Template type doesn't exist
            return Activator.CreateInstance( type ) as NMSTemplate;
        }

        internal static int GetDataSize( NMSTemplate template ) {
            if ( template == null ) return 0;

            using ( var ms = new MemoryStream() )
            using ( var bw = new BinaryWriter( ms ) ) {
                var addt = new List<Tuple<long, object>>();
                int addtIdx = 0;

                var prevState = isDebugLogTemplateEnabled;
                isDebugLogTemplateEnabled = false;
                MBIN.MbinSerializer.AppendToWriter( template, bw, ref addt, ref addtIdx, template.GetType() );
                isDebugLogTemplateEnabled = prevState;

                return ms.ToArray().Length;
            }
        }

        internal static int GetDataSize( string templateName ) => GetDataSize( TemplateFromName( templateName ) );

        internal static FieldInfo[] GetOrderedFields( Type type ) {
            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var fields = type.GetFields( bindingFlags ).OrderBy( field => field.MetadataToken );
            return fields.Where( field => !field.IsPrivate || (field.GetCustomAttribute<NMSAttribute>() != null) ).ToArray();
        }

        /// <summary>
        /// Serialises the NMSTemplate object to a .mbin file with default header information.
        /// </summary>
        /// <param name="outputpath">The location to write the .mbin file.</param>
        public static void WriteToMbin<T>( T template, string outputpath ) where T : NMSTemplate {
            using ( var file = new MbinFile( outputpath ) ) {
                file.header = new MBIN.MbinHeader();
                var type = template.GetType();
                file.header.SetDefaults( type );
                file.SaveData( template );
                file.SaveHeader();
            }
        }

        /// <summary>
        /// Writes the NMSTemplate object to an .exml file.
        /// </summary>
        /// <param name="outputpath">The location to write the .exml file.</param>
        public void WriteToExml<T>( T template, string outputpath ) where T : NMSTemplate {
            File.WriteAllText( outputpath, ExmlFile.WriteTemplate( template ) );
        }

        public static string GetID( NMSTemplate template ) {
            var fields = GetOrderedFields( template.GetType() );
            foreach ( var field in fields ) {
                NMSAttribute settings = field.GetCustomAttribute<NMSAttribute>();
                if ( settings?.IDField ?? false ) {
                    return field.GetValue( template ).ToString();
                }
            }
            return null;
        }

    }

}
