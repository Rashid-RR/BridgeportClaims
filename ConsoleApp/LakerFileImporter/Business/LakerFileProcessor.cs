using NLog;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LakerFileImporter.IO;
using LakerFileImporter.ApiClientCaller;
using LakerFileImporter.DAL.ImportFileProvider;

namespace LakerFileImporter.Business
{
    internal class LakerFileProcessor
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Boolean returns whether or not the process was even necessary (a asel
        /// </summary>
        /// <returns></returns>
        internal async Task<LakerFileProcessResult> UploadAndProcessLakerFileIfNecessary()
        {
            try
            {
                var importFileProvider = new ImportFileProvider();
                var dbFiles = importFileProvider.GetImportFileDtos();
                var lastProcessedFile = dbFiles?.OrderByDescending(x => x.LakerFileDate).FirstOrDefault();
                // Now, find the latest, possible file in the directory, and use 
                // the file found in the database above to see if it's new or not.
                var ioHelper = new IoHelper();
                var newestFileInLocalDirectory = ioHelper.BrowseDirectoryToLocateFile();
                if (null == newestFileInLocalDirectory) return LakerFileProcessResult.NoFilesFoundInFileDirectory; // If no file is found in the directory to process, return no.
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
                    : LakerFileProcessResult.LakerFileProcessedSuccessfully;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}