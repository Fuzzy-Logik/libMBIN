using System;
using System.IO;

using libMBIN;

namespace MBINCompiler {

    using static CommandLineOptions;
    using Commands;

    public enum ErrorCode {
        Success =  0,
        Unknown,
        CommandLine,
        FileNotFound,
        FileExists,
        FileInvalid,
    }

    internal class Program
    {

        public static int Main( string[] args )
        {

            CommandLine.Initialize();

            Logger.Open( Path.ChangeExtension( Utils.GetExecutablePath(), ".log" ) );
            Logger.EnableTraceLogging = true;

#if DEBUG_STDOUT
            Logger.LogToConsole = true;
#endif

            Logger.LogMessage( "VERSION", $"MBINCompiler v{Version.GetVersionStringCompact()}" );
            Logger.LogMessage( "ARGS", $"\"{string.Join( "\" \"", args )}\"\n" );
            using ( var indent = new Logger.IndentScope() ) {
                Logger.LogMessage( "If you encounter any errors, please submit a bug report and include this log file.\n" +
                                   "Please check that there isn't already a similar issue open before creating a new one.\n" +
                                   "https://github.com/monkeyman192/MBINCompiler/issues\n" );
            }

            var options = new CommandLineParser( args );
            options.AddOptions( null,      OPTIONS_GENERAL );
            options.AddOptions( "help",    OPTIONS_HELP    );
            options.AddOptions( "version", OPTIONS_VERSION );
            options.AddOptions( "scan",    OPTIONS_SCAN    );
            options.AddOptions( "convert", OPTIONS_CONVERT );

            // save the error state
            bool invalidArguments = !options.Parse( "convert" );

            // get the Quiet option first, before we emit anything
            Quiet = options.GetOptionSwitch( "quiet" );
            if ( Quiet ) {
                Console.SetOut( new StreamWriter( Stream.Null ) );
                Console.SetError( Console.Out );
            }

            // now we can emit an error if we need to
            if ( invalidArguments ) return CommandLine.ShowInvalidCommandLineArg( options );

            // initialize remaining global options
            DebugMode = options.GetOptionSwitch( "debug" );

            // execute the appropriate mode
            try {
                switch (options.Verb) {
                    case "help":    return HelpCommand.Execute( options );
                    case "version": return VersionCommand.Execute( options );
                    case "scan":    return HandleScanMode( options );
                    default:        return ConvertCommand.Execute( options );
                }
            } catch ( System.Exception e ) {
                return CommandLine.ShowException( e );
            }

        }

        private static int HandleScanMode( CommandLineParser options ) {
            string oldDir = null;
            string newDir = null;

            var paths = options.GetFileParams();

            var full = false;

            var switchList = options.GetOptionSwitch( "list" );
            if ( switchList ) {
                if ( paths.Count < 1 ) return Console.ShowCommandLineError( "Missing required <GameData Directory> argument." );
                newDir = paths[0];
                paths.RemoveAt( 0 );
            } else {
                full = options.GetOptionSwitch( "full" );
                if ( paths.Count < 1 ) return Console.ShowCommandLineError( "Missing required <Old GameData Directory> argument." );
                if ( paths.Count < 2 ) return Console.ShowCommandLineError( "Missing required <New GameData Directory> argument." );
                oldDir = paths[0];
                newDir = paths[1];
                paths.RemoveAt( 0 );
                paths.RemoveAt( 0 );
            }

            if (options.Args.Count > 0) return Console.ShowInvalidCommandLineArg( options.Args[0] );
            if ( paths.Count > 0 ) return Console.ShowInvalidCommandLineArg( paths[0] );

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            if ( full ) {
                var results = ModeScan.ScanAssets( oldDir, newDir );
                Logger.WriteLine( $"Elapsed time: {stopwatch.Elapsed}" );
                ModeScan.EmitResults( results );
            } else {
                var results = switchList ? ModeScan.ScanGUID( newDir ) : ModeScan.ScanGUID( oldDir, newDir );
                Logger.WriteLine( $"Elapsed time: {stopwatch.Elapsed}" );
                ModeScan.EmitResults( results );
            }

            return (int) ErrorCode.Success;
        }

    }
}
