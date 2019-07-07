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
using LakerFileImporter.DAL.ImportFileProvider.Dtos;
using LakerFileImporter.Enums;
using LakerFileImporter.Helpers;
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
        internal async Task<List<LakerAndEnvisionFileProcessResult>> UploadAndProcessLakerFileIfNecessaryAsync()
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
                var lastUnprocessedLakerFileFromDatabase = dbFiles?.Where(p => !p.Processed && p.FileType == c.LakerFileTypeName).OrderByDescending(x => x.FileDate).FirstOrDefault();
                var envisionFilesFromDatabase = dbFiles?.Where(p => p.FileType == c.EnvisionFileTypeName).OrderByDescending(x => x.FileDate).ToList();
                // Ok, so now we have the latest file that has been processed in the database.
                if (null != lastProcessedLakerFileFromDatabase && cs.AppIsInDebugMode)
                {
                    Logger.Value.Info($"The last processed Laker file in the Database was {lastProcessedLakerFileFromDatabase.FileName}. Recording this from method {methodName} on {now}.");
                }
                // Now, let's prepare any necessary folders in the local directory, in preparation for an SFTP operation.
                var lakerMethodSucceeded = IoHelper.CreateMonthAndYearFolderIfNecessary(FileSource.Laker);
                var envisionMethodSucceeded = IoHelper.CreateMonthAndYearFolderIfNecessary(FileSource.Envision);
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
                var newestLakerFileInLocalDirectory = IoHelper.BrowseDirectoryToLocateFile();
                var envisionFiles = IoHelper.BrowseDirectoryForTenEnvisionFiles();
                // Process Envision Files.
                await UploadEnvisionFilesAsync(envisionFiles, envisionFilesFromDatabase);
                // TODO: Process Envision files.

                // Move on to Laker File.
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
                if (cs.AppIsInDebugMode)
                {
                    Logger.Value.Info($"The newest Laker file found in the local directory is {newestLakerFileInLocalDirectory.FullFileName}. This was recorded in the {methodName} method on {now}.");
                }
                var newLakerFileNeededForProcessing = null == lastProcessedLakerFileFromDatabase || newestLakerFileInLocalDirectory.FileDate > lastProcessedLakerFileFromDatabase.FileDate;
                if (!newLakerFileNeededForProcessing)
                {
                    results.Add(LakerAndEnvisionFileProcessResult.NoLakerFileProcessingNecessary);
                    return results;
                }
                var isLatestLakerFileAlreadyUploaded = null != lastUnprocessedLakerFileFromDatabase && newestLakerFileInLocalDirectory.FileDate == lastUnprocessedLakerFileFromDatabase.FileDate;
                if (!isLatestLakerFileAlreadyUploaded)
                {
                    var lakerResult = await UploadFileToApiAsync(newestLakerFileInLocalDirectory.FullFileName,
                        newestLakerFileInLocalDirectory.FileName, FileSource.Laker).ConfigureAwait(false);
                    results.Add(lakerResult);
                    return results;
                }
                // Else, if the Laker file is already there, just not processed, then process it.
                var result = await ProcessFileToApiAsync(FileSource.Laker).ConfigureAwait(false);
                results.Add(result);
                return results;
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                throw;
            }
        }

        private static async Task UploadEnvisionFilesAsync(IEnumerable<ImportFileModel> files,
            IList<ImportFileDto> unprocessedEnvisionFilesFromDatabase)
        {
            try
            {
                var emptyEnvisionFileByteSize = Convert.ToInt32(cs.GetAppSetting(c.EmptyEnvisionFileByteSizeKey));
                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file.FullFileName);
                    // If the file is empty, ignore it
                    if (fileInfo.Length <= emptyEnvisionFileByteSize)
                    {
                        continue;
                    }
                    // If the file is already uploaded, ignore it.
                    if (unprocessedEnvisionFilesFromDatabase.Any(x =>
                        string.Equals(x.FileName, file.FileName, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        continue;
                    }
                    await UploadFileToApiAsync(file.FullFileName, file.FileName, FileSource.Envision).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                throw;
            }
        }

        private static async Task<LakerAndEnvisionFileProcessResult> UploadFileToApiAsync(string fullFileName, string fileName, FileSource fileSource)
        {
            // Upload and process Laker File on Server.
            var apiClient = new ApiClient();
            var bytes = File.ReadAllBytes(fullFileName);
            var token = await apiClient.GetAuthenticationBearerTokenAsync().ConfigureAwait(false);
            var newFileName = fileName;
            var succeededUpload = await apiClient.UploadFileToApiAsync(bytes, newFileName, token).ConfigureAwait(false);
            if (!succeededUpload)
            {
                return fileSource == FileSource.Laker ? LakerAndEnvisionFileProcessResult.LakerFileFailedToUpload : LakerAndEnvisionFileProcessResult.EnvisionFileFailedToUpload;
            }
            return fileSource == FileSource.Laker ? LakerAndEnvisionFileProcessResult.LakerFileUploadedSuccessfully : LakerAndEnvisionFileProcessResult.EnvisionFileUploadedSuccessfully;
        }

        private static async Task<LakerAndEnvisionFileProcessResult> ProcessFileToApiAsync(FileSource fileSource)
        {
            var apiClient = new ApiClient();
            var token = await apiClient.GetAuthenticationBearerTokenAsync().ConfigureAwait(false);
            var succeededProcessing = await apiClient.ProcessImportFileToApiAsync(token, fileSource).ConfigureAwait(false);
            if (fileSource == FileSource.Laker)
            {
                return succeededProcessing ? LakerAndEnvisionFileProcessResult.LakerFileProcessStartedSuccessfully : LakerAndEnvisionFileProcessResult.LakerFileFailedToProcess;
            }
            return succeededProcessing ? LakerAndEnvisionFileProcessResult.EnvisionFileProcessStartedSuccessfully : LakerAndEnvisionFileProcessResult.EnvisionFileFailedToProcess;
        }
    }
}