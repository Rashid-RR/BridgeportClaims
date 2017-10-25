using System.ComponentModel;
using BridgeportClaims.Common.Disposable;

namespace BridgeportClaims.Web.BackgroundWork
{
    public static class BackgroundWorkerProvider
    {
        public static void Run(DoWorkEventHandler doWork,
            RunWorkerCompletedEventHandler completed = null,
            ProgressChangedEventHandler progressChanged = null)
        {
            DisposableService.Using(() => new BackgroundWorker(), backgroundWorker =>
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
            });
        }
    }
}