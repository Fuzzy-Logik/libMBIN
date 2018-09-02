using System.Collections.Generic;

namespace MBINCompiler.Scan {

    internal class AssetScanner : AbstractScanner {

        private string oldDir, newDir;
        private ScanInfo scanInfo;
        private List<ScanInfo> pendingFiles;

        public enum ScanStatus { Unchanged, Modified, Added, Removed }

        public class ScanInfo {
            public string filePath;
            public ScanStatus status;
            public bool isMBIN;
            public bool isStructChanged;
            public ulong guid;

            public ScanInfo( string path ) {
                this.filePath = path;
                this.status = ScanStatus.Unchanged;
                this.isMBIN = false;
                this.isStructChanged = false;
                this.guid = 0;
            }
        }

        //private new class WorkerThread : AbstractScanner.WorkerThread {

        //    public WorkerThread( AssetScanner scanner ) : base( scanner ) { }

        //    public WorkerThread( string oldDir, string newDir, ScanInfo scanInfo, OnThreadFinished onFinishedCallback ) {
        //        this.oldDir = oldDir;
        //        this.newDir = newDir;
        //        this.scanInfo = scanInfo;
        //    }

        //    public override void Execute() {
        //        while ( scanInfo != null ) {
        //            scanInfo = onFinished?.Invoke( CompareFiles( oldDir, newDir, scanInfo ) );
        //            Thread.Yield();
        //        }
        //    }

        //}

        //private ScanInfo ThreadFinishedHandler( ScanInfo scanInfo ) {
        //    lock ( this ) {
        //        if ( pendingFiles.Count > 0 ) {
        //            scanInfo = pendingFiles[0];
        //            pendingFiles.RemoveAt( 0 );
        //            return scanInfo;
        //        }
        //    }
        //    numWorkers--;
        //    return null;
        //}

        protected override bool OnExecute( Scan.WorkerThread worker ) {
            throw new System.NotImplementedException();
        }

        private AssetScanner() : base() {
        }

    }

}
