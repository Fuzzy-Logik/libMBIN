using System;

namespace libMBIN.MBIN {

    public class MbinException : APIException {

        public string filePath;

        private const string DEFAULT_MSG = "An error occurred while processing an MBIN file.";
        public MbinException( string msg, string path, Exception innerException = null ) : base( GetString( msg ?? DEFAULT_MSG, path ), innerException ) { filePath = path; }
        public MbinException(             string path, Exception innerException = null ) : this( null, path, innerException ) { }

        private static string GetString( string msg, string path ) => $"{msg}\n\"{path}\"";

    }

}
