using NLog;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LakerFileImporter.IO;
using LakerFileImporter.ApiClientCaller;
using LakerFileImporter.DAL.ImportFileProvider;
using LakerFileImporter.Logging;
using cs = LakerFileImporter.ConfigService.ConfigService;

namespace LakerFileImporter.Business
{
    internal class LakerFileProcessor
    {
        private static readonly Logger Logger = LoggingService.Instance.Logger;

        /// <summary>
        /// Boolean returns whether or not the process was even necessary (a asel
        /// </summary>
        /// <returns></returns>
        internal async Task<LakerFileProcessResult> UploadAndProcessLakerFileIfNecessary()
        {
            try
            {
                var methodName = MethodBase.GetCurrentMethod().Name;
                var now = DateTime.Now.ToString("G");
                if (cs.AppIsInDebugMode)
                    Logger.Info($"Entered the {methodName} method on {now}");
                var importFileProvider = new ImportFileProvider();
                var dbFiles = importFileProvider.GetImportFileDtos();
                var lastProcessedFile = dbFiles?.OrderByDescending(x => x.LakerFileDate).FirstOrDefault();
                if (null != lastProcessedFile && cs.AppIsInDebugMode)
                    Logger.Info($"The last processed Laker file in the Database was {lastProcessedFile.FileName}. Recording this from method {methodName} on {now}.");
                // Now, find the latest, possible file in the directory, and use 
                // the file found in the database above to see if it's new or not.
                var ioHelper = new IoHelper();
                var newestFileInLocalDirectory = ioHelper.BrowseDirectoryToLocateFile();

                if (null == newestFileInLocalDirectory)
                {
                    if (cs.AppIsInDebugMode)
                        Logger.Info("No file was found in the directory to process. You might want to make sure we're pointing to the right place. " +
                                    $"This was recorded in the {methodName} method on {now}.");
                    return LakerFileProcessResult.NoFilesFoundInFileDirectory; // If no file is found in the directory to process, return no.
                }
                if (cs.AppIsInDebugMode)
                    Logger.Info($"The newest file found in the local directory is {newestFileInLocalDirectory.FullFileName}. This was recorded in the {methodName} method on {now}.");
                var newLakerFileNeededForProcessing =
                    null == lastProcessedFile || newestFileInLocalDirectory.LakerFileDate > lastProcessedFile.LakerFileDate;
                if (!newLakerFileNeededForProcessing)
                    return LakerFileProcessResult.NoLakerFileProcessingNecessary;
                // Upload and process Laker File on Server.
                var apiClient = new ApiClient();
                var bytes = File.ReadAllBytes(newestFileInLocalDirectory.FullFileName);
                var token = await apiClient.GetAuthenticationBearerTokenAsync();
                var newLakerFileName = newestFileInLocalDirectory.FileName;
                var succeededUpload = await apiClient.UploadFileToApiAsync(bytes, newLakerFileName, token);
                if (!succeededUpload)
                    return LakerFileProcessResult.LakerFileFailedToUpload;
                var succeededProcessing = await apiClient.ProcessLakerFileToApiAsync(newLakerFileName, token);
                return !succeededProcessing
                    ? LakerFileProcessResult.LakerFileFailedToProcess
                    : LakerFileProcessResult.LakerFileProcessStartedSuccessfully;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}