namespace MBINCompiler.Scan {

    internal abstract class AbstractScanner {
        protected WorkerThread[] workers;
        protected int numWorkers;

        public AbstractScanner() {
            workers = new WorkerThread[System.Environment.ProcessorCount];
            numWorkers = 0;
        }

        /// <summary>
        /// Called in a loop from worker.Execute().
        /// The loop keeps repeating as long as this returns true.
        /// </summary>
        /// <param name="worker">The worker thread that is executing.</param>
        /// <returns>true to continue thread execution. false to terminate.</returns>
        protected abstract bool OnExecute( WorkerThread worker );

    }

    internal class WorkerThread {

        /// <summary></summary>
        /// <param name="worker">this instance</param>
        /// <returns>false to cancel thread execution. true to continue.</returns>
        public delegate bool OnExecuteCallback( WorkerThread worker );

        protected object obj = null;
        protected OnExecuteCallback onExecute = null;

        public WorkerThread( object obj , OnExecuteCallback cbExecute ) {
            this.obj = obj;
            this.onExecute = cbExecute;
        }

        public void Execute() {
            while ( onExecute?.Invoke( this ) ?? false ) ;
        }
    }

}
