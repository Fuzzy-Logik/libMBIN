using System;

namespace libMBIN.EXML {

    public class ExmlException : APIException {

        public string filePath;

        private const string DEFAULT_MSG = "An error occurred while processing an EXML file.";
        public ExmlException( string msg, string path, Exception innerException = null ) : base( GetString( msg ?? DEFAULT_MSG, path ), innerException ) { filePath = path; }
        public ExmlException(             string path, Exception innerException = null ) : this( null, path, innerException ) { }

        private static string GetString( string msg, string path ) => $"{msg}\n\"{path}\"";

    }

}