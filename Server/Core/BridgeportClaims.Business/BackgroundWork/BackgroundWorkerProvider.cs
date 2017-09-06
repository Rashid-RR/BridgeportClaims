using System.ComponentModel;

namespace BridgeportClaims.Business.BackgroundWork
{
    public static class BackgroundWorkerProvider
    {
        public static void Run(DoWorkEventHandler doWork,
            RunWorkerCompletedEventHandler completed = null,
            ProgressChangedEventHandler progressChanged = null)
        {
            using (var backgroundWorker = new BackgroundWorker())
            {
                backgroundWorker.DoWork += doWork;
                if (completed != null)
                    backgroundWorker.RunWorkerCompleted += completed;
                if (progressChanged != null)
                {
                    backgroundWorker.WorkerReportsProgress = true;
                    backgroundWorker.ProgressChanged += progressChanged;
                }
                backgroundWorker.RunWorkerAsync();
            }
        }
    }
}