using System;
using c = LakerFileImporter.StringConstants.Constants;
using cs = LakerFileImporter.ConfigService.ConfigService;
using System.IO;
using System.Linq;
using LakerFileImporter.Helpers;
using NLog;

namespace LakerFileImporter.IO
{
    internal class IoHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        internal static string GetFullLocalFilePathPlusMonthYearFolderByDate(DateTime date)
        {
            var monthFolderFormat = cs.GetAppSetting(c.MonthFolderFormatKey);
            var monthFolderDirectory = date.ToString(monthFolderFormat);
            var pathWithMonthDirectory = Path.Combine(cs.GetAppSetting(c.LakerFilePathKey), monthFolderDirectory);
            return pathWithMonthDirectory;
        }

        internal bool CreateMonthAndYearFolderIfNecessary()
        {
            try
            {
                // Create a folder
                var pathWithMonthDirectory = GetFullLocalFilePathPlusMonthYearFolderByDate(DateTime.Now);
                if (string.IsNullOrWhiteSpace(pathWithMonthDirectory))
                    throw new Exception(
                        "Something went wrong with the month directory path. It was not populated correctly. It is null or empty.");
                if (Directory.Exists(pathWithMonthDirectory)) return true;
                Directory.CreateDirectory(pathWithMonthDirectory);
                if (cs.AppIsInDebugMode)
                    Logger.Info($"Created new Directory {pathWithMonthDirectory} because it didn't exist.");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
    }

        internal FileDateParsingHelper BrowseDirectoryToLocateFile()
        {
            try
            {
                var directoryInfo = new DirectoryInfo(GetFullLocalFilePathPlusMonthYearFolderByDate(DateTime.Now));
                var files = directoryInfo.GetFiles()
                    .Where(x => !string.IsNullOrWhiteSpace(x.Name) && x.Name.StartsWith("Billing_Claim_File_") && x.Name.EndsWith(".csv"))
                    .OrderByDescending(p => p.CreationTime)
                    .Take(Convert.ToInt32(cs.GetAppSetting(c.FileProcessorTopNumberKey))).ToList();
                // Now traverse the top, however many files to find the latest.
                var newFiles = files.Select(s => new FileDateParsingHelper
                {
                    FileName = s.Name,
                    FullFileName = s.FullName
                }).ToList();
                if (newFiles.Count < 1)
                    return null;
                var newestFullFileName = newFiles.OrderByDescending(x => x.LakerFileDate).FirstOrDefault();
                return newestFullFileName;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
