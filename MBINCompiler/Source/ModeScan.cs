using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using libMBIN;

namespace MBINCompiler {

    internal class ModeScan {

        public class ResultsGUID : Dictionary<string, ulong> {
            public ResultsGUID() : base() { }
        }

        private class GuidScanner : AbstractScanner {
            private string[] filesGUID;
            private ResultsGUID resultsGUID;

            private static readonly string[] suffixes = new string[] {
                ".ANIM.MBIN",
                ".ENTITY.MBIN",
                ".GEOMETRY.DATA.MBIN",
                ".GEOMETRY.MBIN.PC",
                ".MATERIAL.MBIN",
                ".DESCRIPTOR.MBIN",
                ".PARTICLE.MBIN",
                ".TEXTURE.MBIN",
                ".SCENE.MBIN",
            };

            private static bool[] skip = new bool[suffixes.Length];

            public GuidScanner() : base() { }

        }

        private AssetScanner.ScanInfo[] RunScanAssets( string oldDir, string newDir ) {
            oldDir = Path.GetFullPath( oldDir );
            newDir = Path.GetFullPath( newDir );

            List<string> oldFiles = new List<string>( Directory.GetFiles( oldDir, "*", SearchOption.AllDirectories ) );
            List<string> newFiles = new List<string>( Directory.GetFiles( newDir, "*", SearchOption.AllDirectories ) );

            var scanFiles = new AssetScanner.ScanInfo[Math.Max( oldFiles.Count, newFiles.Count )];
            int fileCount = 0;

            for ( int i = 0; i < oldFiles.Count; i++ ) oldFiles[i] = NormalizeFilePath( oldDir, oldFiles[i] );

            for ( int i = 0; i < newFiles.Count; i++ ) {
                var scanInfo = new AssetScanner.ScanInfo( NormalizeFilePath( newDir, newFiles[i] ) );
                if ( !oldFiles.Remove( scanInfo.filePath ) ) { // added
                    scanInfo.status = AssetScanner.ScanStatus.Added;
                    using ( var mbin = new MBINFile( newFiles[i] ) ) {
                        if ( mbin.Load() && mbin.Header.IsValid ) {
                            scanInfo.isMBIN = true;
                            scanInfo.isStructChanged = true;
                            scanInfo.guid = mbin.Header.TemplateGUID;
                        }
                    }
                } else { // modified or unchanged
                    pendingFiles.Add( scanInfo );
                }

                scanFiles[fileCount++] = scanInfo;
            }

            foreach ( var file in oldFiles ) { // removed
                var scanInfo = new AssetScanner.ScanInfo( file );
                scanInfo.status = AssetScanner.ScanStatus.Removed;
                using ( var mbin = new MBINFile( file ) ) {
                    if ( mbin.Load() && mbin.Header.IsValid ) {
                        scanInfo.isMBIN = true;
                        scanInfo.guid = mbin.Header.TemplateGUID;
                    }
                }
                scanFiles[fileCount++] = scanInfo;
            }

            //int maxWorkers = Math.Min( workers.Length, pendingFiles.Count );
            //for ( int i = 0; i < maxWorkers; i++ ) {
            //    numWorkers++;
            //    var scanInfo = pendingFiles[0];
            //    pendingFiles.RemoveAt( 0 );
            //    workers[i] = new WorkerThread( oldDir, newDir, scanInfo, ThreadFinishedHandler );
            //    new Thread( new ThreadStart( workers[i].Execute ) ).Start();
            //}

            //while ( numWorkers > 0 ) Thread.Yield();;

            foreach ( var file in pendingFiles ) CompareFiles( oldDir, newDir, file );

            return scanFiles;
        }

        private ResultsGUID RunScanGUID( string path ) {
            var files = Directory.GetFiles( path, "*.MBIN*", SearchOption.AllDirectories );
            resultsGUID = new ResultsGUID();

            skip = new bool[suffixes.Length];

            foreach ( var file in files ) {
                int i = 0;
                for ( ; i < suffixes.Length; i++ ) {
                    if ( !file.EndsWith( suffixes[i] ) ) continue;
                    if ( skip[i] ) break;
                    skip[i] = true;
                }
                if ( i < suffixes.Length ) continue;

                using ( var mbin = new MBINFile( file ) ) {
                    if ( mbin.Load() && mbin.Header.IsValid ) results[mbin.Header.TemplateName] = mbin.Header.TemplateGUID;
                }
            }

            return results;
        }

        private string NormalizeFilePath( string dir, string path ) => path.Substring( dir.Length + 1 ).ToUpper();

        public static AssetScanner.ScanInfo[] ScanAssets( string oldDir, string newDir ) {
            return new ModeScan().RunScanAssets( oldDir, newDir );
        }

        private static AssetScanner.ScanInfo CompareFiles( string oldDir, string newDir, AssetScanner.ScanInfo scanInfo ) {
            var oldFile = new FileInfo( Path.Combine( oldDir, scanInfo.filePath ) );
            var newFile = new FileInfo( Path.Combine( newDir, scanInfo.filePath ) );

            using ( var fOld = new FileStream( oldFile.FullName, FileMode.Open, FileAccess.Read ) )
            using ( var fNew = new FileStream( newFile.FullName, FileMode.Open, FileAccess.Read ) ) {

                // if it's an MBIN file we need to compare the GUIDs and skip the header in the hash
                var oldMBIN = new MBINFile( fOld, true );
                var newMBIN = new MBINFile( fNew, true );
                if ( (oldMBIN.Load() && oldMBIN.Header.IsValid) && (newMBIN.Load() && newMBIN.Header.IsValid) ) {
                    scanInfo.isMBIN = true;
                    scanInfo.isStructChanged = (oldMBIN.Header.TemplateGUID != newMBIN.Header.TemplateGUID);
                    scanInfo.guid = newMBIN.Header.TemplateGUID;
                    scanInfo.status = scanInfo.isStructChanged ? AssetScanner.ScanStatus.Modified : scanInfo.status;
                    if ( scanInfo.isStructChanged ) return scanInfo;

                    fOld.Position = fNew.Position = 0x60;
                }

                // if the files are the same length we need to checksum both files and compare the hashes
                if ( newFile.Length == oldFile.Length ) {
                    Thread.Yield();
                    var oldHash = BitConverter.ToString( System.Security.Cryptography.SHA1.Create().ComputeHash( fOld ) ).Replace( "-", "" );
                    Thread.Yield();
                    var newHash = BitConverter.ToString( System.Security.Cryptography.SHA1.Create().ComputeHash( fNew ) ).Replace( "-", "" );
                    if ( oldHash != newHash ) scanInfo.status = AssetScanner.ScanStatus.Modified;
                }

            }

            return scanInfo;
        }

        public static void EmitResults( AssetScanner.ScanInfo[] results ) {
            if ( results == null ) return;

            Logger.WriteLine(     "Added Files:\t=COUNTIF( A:A, \"ADDED\" )" );
            Logger.WriteLine(   "Removed Files:\t=COUNTIF( A:A, \"REMOVED\" )" );
            Logger.WriteLine(  "Modified Files:\t=COUNTIF( A:A, \"MODIFIED\" )" );
            Logger.WriteLine( "Unchanged Files:\t=COUNTIF( A:A, \"UNCHANGED\" )" );
            Logger.WriteLine( "Total Files:\t=SUM( B1:B4 )" );
            Logger.WriteLine( "Modified Structs:\t=COUNTIF( D:D, TRUE )" );
            Logger.WriteLine( "" );
            Logger.WriteLine( "Status\tPath\tMBIN?\tStruct Changed?\tGUID" );

            foreach (var result in results) {
                string status = result.status.ToString().ToUpper();
                string guid = BitConverter.ToString( BitConverter.GetBytes( result.guid ) ).Replace( "-", "" );
                Logger.WriteLine( $"{status}\t{result.filePath}\t{result.isMBIN}\t{result.isStructChanged}\t{guid}" );
            }

        }

        public static Dictionary<string, ulong> ScanGUID( string gamedataPath ) {
            return new ModeScan().RunScanGUID( gamedataPath );
        }

        //public static Dictionary<string, ulong> ScanGUID( string gamedataPath ) {
        //    var files = Directory.GetFiles( gamedataPath, "*.MBIN*", SearchOption.AllDirectories );
        //    var results = new Dictionary<string, ulong>();

        //    var suffixes = new string[] {
        //        ".ANIM.MBIN",         
        //        ".ENTITY.MBIN",       
        //        ".GEOMETRY.DATA.MBIN",
        //        ".GEOMETRY.MBIN.PC",  
        //        ".MATERIAL.MBIN",     
        //        ".DESCRIPTOR.MBIN",   
        //        ".PARTICLE.MBIN",     
        //        ".TEXTURE.MBIN",      
        //        ".SCENE.MBIN",        
        //    };

        //    var skip = new bool[suffixes.Length];

        //    foreach ( var file in files ) {
        //        int i = 0;
        //        for ( ; i < suffixes.Length; i++ ) {
        //            if ( !file.EndsWith( suffixes[i] ) ) continue;
        //            if ( skip[i] ) break;
        //            skip[i] = true;
        //        }
        //        if ( i < suffixes.Length ) continue;

        //        using ( var mbin = new MBINFile( file ) ) {
        //            if (mbin.Load() && mbin.Header.IsValid) results[mbin.Header.TemplateName] = mbin.Header.TemplateGUID;
        //        }
        //    }

        //    return results;
        //}

        public static Dictionary<string, ulong> ScanGUID( string oldPath, string newPath ) {
            var oldResults = ScanGUID( oldPath );
            var newResults = ScanGUID( newPath );
            var results = new Dictionary<string, ulong>();
            foreach ( var entry in newResults ) {
                if ( oldResults.TryGetValue( entry.Key, out ulong guid ) && ( entry.Value == guid ) ) continue;
                results[entry.Key] = entry.Value;
            }
            return results;
        }

        public static void EmitResults( Dictionary<string, ulong> results ) {
            if ( results == null ) return;
            Logger.WriteLine( "Template\tGUID" );
            foreach ( var entry in results) Logger.WriteLine( String.Format( "{0}\t{1:X16}", entry.Key, entry.Value ) );
        }

    }

}
