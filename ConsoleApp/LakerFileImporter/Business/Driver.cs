using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LakerFileImporter.ApiClientCaller;
using LakerFileImporter.DAL.ImportFileProvider;
using LakerFileImporter.IO;
using NLog;

namespace LakerFileImporter.Business
{
    internal class Driver
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        internal async Task<bool> RunApplicationAsync()
        {
            try
            {
                var importFileProvider = new ImportFileProvider();
                var dbFiles = importFileProvider.GetImportFileDtos();
                var lastProcessedFile = dbFiles?.Where(x => x.Processed)
                    .OrderByDescending(x => x.FileNameExtractedDate).FirstOrDefault();
                // Now, find the latest, possible file in the directory, and use 
                // the file found in the database above to see if it's new or not.
                var ioHelper = new IoHelper();
                var newestFileInDirectory = ioHelper.BrowseDirectoryToLocateFile();
                if (null == newestFileInDirectory) return false;
                if (null != lastProcessedFile && newestFileInDirectory.FileNameExtractedDate <=
                    lastProcessedFile.FileNameExtractedDate) return false;
                // Upload and process Laker File on Server.
                var apiClient = new ApiClient();
                var bytes = File.ReadAllBytes(newestFileInDirectory.FullFileName);
                var token = await apiClient.GetAuthenticationBearerTokenAsync();
                return await apiClient.UploadFileToApiAsync(bytes, newestFileInDirectory.FileName, token);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}