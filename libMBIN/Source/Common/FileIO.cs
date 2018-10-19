using System;
using System.IO;

namespace libMBIN {

    /// <summary>
    /// Container class for general file loading methods.
    /// </summary>
    public class FileIO {

        /// <summary>
        /// Loads a file and returns an NMSTemplate which can be either used as-is, or cast to a specific type.
        /// The path to either an .exml or .mbin can be provided here and the correct method will be selected automatically.
        /// </summary>
        /// <param name="file">File path to the .exml or .mbin to be loaded into memory.</param>
        /// <returns>NMSTemplate</returns>
        public static NMSTemplate LoadFile( string file ) {
            // TODO: doesn't handle MBIN.PC files
            if ( Path.HasExtension( file ) ) {
                string x = Path.GetExtension( file ).ToUpper();
                if ( x == ".EXML" ) {
                    return LoadExml( file );
                } else if ( x == ".MBIN" ) {
                    return LoadMbin( file );
                }
            }
            throw new libMBIN.APIException( new InvalidDataException( $"{file} does not have a supported file type. File type must be one of .exml or .mbin" ) );
        }

        /// <summary>
        /// Loads an .mbin file and returns an NMSTemplate which can be either used as-is, or cast to a specific type.
        /// </summary>
        /// <param name="path">File path to the .mbin to be loaded into memory.</param>
        /// <returns>NMSTemplate</returns>
        public static NMSTemplate LoadMbin( string path ) {
            string err = null;
            try {

                MbinFile mbin = new MbinFile( path );
                if ( !mbin.LoadHeader() || !mbin.header.IsValid ) throw new InvalidDataException( "Not a valid MBIN file!" );

                NMSTemplate data = mbin.LoadData();
                err = $"Failed to read {mbin.header.GetXMLTemplateName()} from MBIN";
                if ( data is null ) throw new InvalidDataException( "Invalid MBIN data." );
                return data;

            } catch ( Exception e ) {
                throw new MBIN.MbinException( err, path, e );
            }
        }

        /// <summary>
        /// Loads an .exml file and returns an NMSTemplate which can be either used as-is, or cast to a specific type.
        /// </summary>
        /// <param name="path">File path to the .exml to be loaded into memory.</param>
        /// <returns>NMSTemplate</returns>
        public static NMSTemplate LoadExml( string path ) {
            return ExmlFile.ReadTemplate( path );
        }

    }

}
