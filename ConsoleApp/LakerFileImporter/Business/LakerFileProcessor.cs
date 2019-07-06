using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LakerFileImporter.IO;
using LakerFileImporter.ApiClientCaller;
using LakerFileImporter.DAL.ImportFileProvider;
using LakerFileImporter.Enums;
using LakerFileImporter.Logging;
using LakerFileImporter.SftpProxy;
using cs = LakerFileImporter.ConfigService.ConfigService;
using c = LakerFileImporter.StringConstants.Constants;

namespace LakerFileImporter.Business
{
    internal class LakerFileProcessor
    {
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(() => LoggingService.Instance.Logger);

        /// <summary>
        /// Boolean returns whether or not the process was even necessary (and successful)
        /// </summary>
        /// <returns></returns>
        internal async Task<List<LakerAndEnvisionFileProcessResult>> UploadAndProcessLakerFileIfNecessary()
        {
            try
            {
                var results = new List<LakerAndEnvisionFileProcessResult>();
                var methodName = MethodBase.GetCurrentMethod().Name;
                var now = DateTime.Now.ToString("G");
                if (cs.AppIsInDebugMode)
                {
                    Logger.Value.Info($"Entered the {methodName} method on {now}");
                }
                var importFileProvider = new ImportFileProvider();
                // Query the database.
                var dbFiles = importFileProvider.GetImportFiles();
                // Grab the last, processed files from the database.
                var lastProcessedLakerFileFromDatabase = dbFiles?.Where(p => p.Processed && p.FileType == c.LakerFileTypeName).OrderByDescending(x => x.FileDate).FirstOrDefault();
                var lastProcessedEnvisionFileFromDatabase = dbFiles?.Where(p => p.Processed && p.FileType == c.EnvisionFileTypeName).OrderByDescending(x => x.FileDate).FirstOrDefault();
                // Ok, so now we have the latest file that has been processed in the database.
                if (null != lastProcessedLakerFileFromDatabase && cs.AppIsInDebugMode)
                {
                    Logger.Value.Info($"The last processed Laker file in the Database was {lastProcessedLakerFileFromDatabase.FileName}. Recording this from method {methodName} on {now}.");
                }
                // Now, let's prepare any necessary folders in the local directory, in preparation for an SFTP operation.
                var lakerMethodSucceeded = new IoHelper().CreateMonthAndYearFolderIfNecessary(FileSource.Laker);
                var envisionMethodSucceeded = new IoHelper().CreateMonthAndYearFolderIfNecessary(FileSource.Envision);
                if (!lakerMethodSucceeded || !envisionMethodSucceeded)
                {
                    if (cs.AppIsInDebugMode)
                    {
                        Logger.Value.Error("Something wrong happened with the creation of the month and year folder. Please read the previous exception message.");
                    }
                    results.Add(LakerAndEnvisionFileProcessResult.MonthYearFolderCouldNotBeCreatedInLocalDirectory);
                    return results;
                }
                // Now, work in the SFTP to download any new files (regardless of whether or not we need them at this point). -- For testing purposes, this should have the option of being skipped.
                var processSftpValue = cs.GetAppSetting(c.ProcessSftpKey)?.ToLower();
                var processSftp = false;
                if (!string.IsNullOrWhiteSpace(processSftpValue))
                {
                    processSftp = Convert.ToBoolean(processSftpValue);
                }
                if (!processSftp)
                {
                    if (cs.AppIsInDebugMode)
                    {
                        Logger.Value.Info("We are NOT doing any SFTP processing because the flag in the App.Config told us not to.");
                    }
                }
                else
                {
                    // Do any necessary SFTP downloading...
                    if (cs.AppIsInDebugMode)
                    {
                        Logger.Value.Info("Ok, beginning SFTP operation(s)...");
                    }
                    var proxyProvider = new SftpProxyProvider();
                    // First download Laker File(s)
                    proxyProvider.ProcessLakerSftpOperation();
                    // Next, download any Envision File(s)
                    proxyProvider.ProcessEnvisionSftpOperation();
                }
                // Now that we've downloaded all SFTP files, we can simply traverse the local directory for the latest file (as we were originally doing)
                var newestLakerFileInLocalDirectory = new IoHelper().BrowseDirectoryToLocateFile(FileSource.Laker);
                var newestEnvisionFileInLocalDirectory = new IoHelper().BrowseDirectoryToLocateFile(FileSource.Envision);
                if (null == newestLakerFileInLocalDirectory)
                {
                    if (cs.AppIsInDebugMode)
                    {
                        Logger.Value.Info("No file was found in the local directory to process. You might want to make sure we're pointing to the right place. " +
                            $"This was recorded in the {methodName} method on {now}.");
                    }
                    results.Add(LakerAndEnvisionFileProcessResult.NoLakerFilesFoundInFileDirectory);
                    return results;
                }
                if (null == newestEnvisionFileInLocalDirectory)
                {
                    if (cs.AppIsInDebugMode)
                    {
                        Logger.Value.Info("No file was found in the local directory to process. You might want to make sure we're pointing to the right place. " +
                                          $"This was recorded in the {methodName} method on {now}.");
                    }
                    results.Add(LakerAndEnvisionFileProcessResult.NoEnvisionFilesFoundInFileDirectory);
                    return results;
                }
                if (cs.AppIsInDebugMode)
                {
                    Logger.Value.Info($"The newest Laker file found in the local directory is {newestLakerFileInLocalDirectory.FullFileName}. This was recorded in the {methodName} method on {now}.");
                    Logger.Value.Info($"The newest Envision file found in the local directory is {newestEnvisionFileInLocalDirectory.FullFileName}. This was recorded in the {methodName} method on {now}.");
                }
                var newLakerFileNeededForProcessing = null == lastProcessedLakerFileFromDatabase || newestLakerFileInLocalDirectory.FileDate > lastProcessedLakerFileFromDatabase.FileDate;
                var newEnvisionFileNeededForProcessing = null == lastProcessedEnvisionFileFromDatabase || newestEnvisionFileInLocalDirectory.FileDate > lastProcessedEnvisionFileFromDatabase.FileDate;
                if (!newLakerFileNeededForProcessing && !newEnvisionFileNeededForProcessing)
                {
                    results.Add(LakerAndEnvisionFileProcessResult.NoLakerOrEnvisionFileProcessingNecessary);
                }
                var lakerResult = await UploadFileToApi(newestLakerFileInLocalDirectory.FullFileName, newestLakerFileInLocalDirectory.FileName, FileSource.Laker);
                var envisionResult = await UploadFileToApi(newestEnvisionFileInLocalDirectory.FullFileName, newestEnvisionFileInLocalDirectory.FileName, FileSource.Envision);
                results.Add(lakerResult);
                results.Add(envisionResult);
                return results;
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                throw;
            }
        }

        private static async Task<LakerAndEnvisionFileProcessResult> UploadFileToApi(string fullFileName, string fileName, FileSource fileSource)
        {
            // Upload and process Laker File on Server.
            var apiClient = new ApiClient();
            var bytes = File.ReadAllBytes(fullFileName);
            var token = await apiClient.GetAuthenticationBearerTokenAsync();
            var newFileName = fileName;
            var succeededUpload = await apiClient.UploadFileToApiAsync(bytes, newFileName, token);
            if (!succeededUpload)
            {
                return fileSource == FileSource.Laker ? LakerAndEnvisionFileProcessResult.LakerFileFailedToUpload : LakerAndEnvisionFileProcessResult.EnvisionFileFailedToUpload;
            }
            var succeededProcessing = await apiClient.ProcessLakerFileToApiAsync(newFileName, token);
            return !succeededProcessing ? fileSource == FileSource.Laker
                    ? LakerAndEnvisionFileProcessResult.LakerFileFailedToProcess
                    : LakerAndEnvisionFileProcessResult.EnvisionFileFailedToProcess
                : fileSource == FileSource.Laker ? LakerAndEnvisionFileProcessResult.LakerFileProcessStartedSuccessfully
                : LakerAndEnvisionFileProcessResult.EnvisionFileProcessStartedSuccessfully;
        }
    }
}