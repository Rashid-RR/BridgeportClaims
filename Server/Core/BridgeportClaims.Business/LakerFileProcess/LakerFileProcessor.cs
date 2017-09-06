using NLog;
using BridgeportClaims.Business.BackgroundWork;
using BridgeportClaims.Data.DataProviders.ImportFiles;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Business.LakerFileProcess
{
    public class LakerFileProcessor : ILakerFileProcessor
    {
        private readonly IImportFileProvider _importFileProvider;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public LakerFileProcessor(IImportFileProvider importFileProvider)
        {
            _importFileProvider = importFileProvider;
        }

        public void ProcessOldestLakerFile()
        {
            BackgroundWorkerProvider.Run((s, e) =>
            {
                if (cs.AppIsInDebugMode)
                    Logger.Info("In the belly of the beast, Background worker process.");
                // First, grab the bytes of the file from the database.
                var tuple = _importFileProvider.GetOldestLakerFileBytes();
                // Which we'll return if this whole process is successful.
                var lakerFileName = tuple.Item1;
                // Take the file bytes, and save them to a temporary path in Windows
                var fullLakerFileTemporaryPath = _importFileProvider.GetLakerFileTemporaryPath(tuple);
                // Take a third-party CSV reader, and turn that temporarily saved laker file into a Data Table.
                var dataTable = _importFileProvider.RetreiveDataTableFromLatestLakerFile(fullLakerFileTemporaryPath);
                // Import the new file, into the new Staged Laker File that will be imported into the database
                _importFileProvider.LakerImportFileProcedureCall(dataTable);
                // Finally, use the newly imported file, to Upsert the database.
                _importFileProvider.EtlLakerFile();
                e.Result = lakerFileName;
            }, (s, e) =>
            {
                if (cs.AppIsInDebugMode)
                    Logger.Info("Run Worker Completed event handler");
                if (null != e.Error)
                    Logger.Error(e.Error);
                if (e.Cancelled)
                    Logger.Warn("Background Worker Process for the Laker File cancelled.");
            }, (s, e) =>
            {
                if (!cs.AppIsInDebugMode) return;
                Logger.Info($"Progress percentage: {e.ProgressPercentage}");
                Logger.Info("User state: {e.UserState.ToString()}");
            });
        }
    }
}