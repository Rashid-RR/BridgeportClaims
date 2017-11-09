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
using LakerFileImporter.SftpProxy;
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
                var lastProcessedFile = dbFiles?.Where(p => p.Processed).OrderByDescending(x => x.LakerFileDate).FirstOrDefault();
                // Ok, so now we have the latest file that has been processed in the database.
                if (null != lastProcessedFile && cs.AppIsInDebugMode)
                    Logger.Info($"The last processed Laker file in the Database was {lastProcessedFile.FileName}. Recording this from method {methodName} on {now}.");
               
                // Now, let's find the latest possible file in the local directory, so that we know whether or
                // not we need to download a new file from the SFTP directory.
                var ioHelper = new IoHelper();
                var newestFileInLocalDirectory = ioHelper.BrowseDirectoryToLocateFile();

                // Now, whether we found a file in the local directory or not, we need to engage the SFTP location to download
                // a file that could be newer, and continue on from there.
                var sftpProxyProvider = new SftpProxyProvider();
                var absoluteNewestFile = sftpProxyProvider.GetLatestFileFromFilePathOrSftp(newestFileInLocalDirectory);

                if (null == absoluteNewestFile)
                {
                    if (cs.AppIsInDebugMode)
                        Logger.Info("No file was found in the directory to process. You might want to make sure we're pointing to the right place. " +
                                    $"This was recorded in the {methodName} method on {now}.");
                    return LakerFileProcessResult.NoFilesFoundInFileDirectory; // If no file is found in the directory to process, return no.
                }
                if (cs.AppIsInDebugMode)
                    Logger.Info($"The newest file found in the local directory is {absoluteNewestFile.FullFileName}. This was recorded in the {methodName} method on {now}.");
                var newLakerFileNeededForProcessing =
                    null == lastProcessedFile || absoluteNewestFile.LakerFileDate > lastProcessedFile.LakerFileDate;
                if (!newLakerFileNeededForProcessing)
                    return LakerFileProcessResult.NoLakerFileProcessingNecessary;
                // Upload and process Laker File on Server.
                var apiClient = new ApiClient();
                var bytes = File.ReadAllBytes(absoluteNewestFile.FullFileName);
                var token = await apiClient.GetAuthenticationBearerTokenAsync();
                var newLakerFileName = absoluteNewestFile.FileName;
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