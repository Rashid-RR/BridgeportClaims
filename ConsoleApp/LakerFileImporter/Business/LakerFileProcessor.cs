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
using c = LakerFileImporter.StringConstants.Constants;

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
                // Query the database.
                var dbFiles = importFileProvider.GetImportFileDtos();
                // Grab the last, processed file from the database.
                var lastProcessedFileFromDatabase = dbFiles?.Where(p => p.Processed).OrderByDescending(x => x.LakerFileDate).FirstOrDefault();
                // Ok, so now we have the latest file that has been processed in the database.
                if (null != lastProcessedFileFromDatabase && cs.AppIsInDebugMode)
                    Logger.Info($"The last processed Laker file in the Database was {lastProcessedFileFromDatabase.FileName}. Recording this from method {methodName} on {now}.");
               
                // Now, let's prepare any necessary folders in the local directory, in preparation for an SFTP operation.
                var ioHelper = new IoHelper();
                var methodSuceeded = ioHelper.CreateMonthAndYearFolderIfNecessary();
                if (!methodSuceeded)
                {
                    if (cs.AppIsInDebugMode)
                        Logger.Error(
                            "Something wrong happened with the creation of the month and year folder. Please read the previous exception message.");
                    return LakerFileProcessResult.MonthYearFolderCouldNotBeCreatedInLocalDirectory;
                }
                // Now, work in the SFTP to download any new files (regardless of whether or not we need them at this point). -- For testing purposes, this should have the option of being skipped.
                var processSftpValue = cs.GetAppSetting(c.ProcessSftpKey)?.ToLower();
                var processSftp = false;
                if (!string.IsNullOrWhiteSpace(processSftpValue))
                    processSftp = Convert.ToBoolean(processSftpValue);
                if (!processSftp)
                {
                    if (cs.AppIsInDebugMode)
                        Logger.Info(
                            "We are NOT doing any SFTP processing because the flag in the App.Config told us not to.");
                }
                else
                {
                    // Do any necessary SFTP downloading...
                    if (cs.AppIsInDebugMode)
                        Logger.Info("Ok, beginning SFTP operation(s)...");
                    var proxyProvider = new SftpProxyProvider();
                    proxyProvider.ProcessSftpOperation();
                }

                // Now that we've downloaded all SFTP files, we can simply traverse the local directory for the latest file (as we were originally doing) -
                var newestFileInLocalDirectory = ioHelper.BrowseDirectoryToLocateFile();
                if (null == newestFileInLocalDirectory)
                {
                    if (cs.AppIsInDebugMode)
                        Logger.Info("No file was found in the local directory to process. You might want to make sure we're pointing to the right place. " +
                                    $"This was recorded in the {methodName} method on {now}.");
                    return LakerFileProcessResult.NoFilesFoundInFileDirectory;
                }
                if (cs.AppIsInDebugMode)
                    Logger.Info($"The newest file found in the local directory is {newestFileInLocalDirectory.FullFileName}. This was recorded in the {methodName} method on {now}.");
                var newLakerFileNeededForProcessing =
                    null == lastProcessedFileFromDatabase || newestFileInLocalDirectory.LakerFileDate > lastProcessedFileFromDatabase.LakerFileDate;
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