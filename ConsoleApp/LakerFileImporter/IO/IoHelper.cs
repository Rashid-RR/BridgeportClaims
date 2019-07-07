using System;
using System.Collections.Generic;
using c = LakerFileImporter.StringConstants.Constants;
using cs = LakerFileImporter.ConfigService.ConfigService;
using System.IO;
using System.Linq;
using LakerFileImporter.Enums;
using LakerFileImporter.Helpers;
using NLog;

namespace LakerFileImporter.IO
{
    internal class IoHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        internal static string GetFullLocalFilePathPlusMonthYearFolderByDate(DateTime date, FileSource fileSource)
        {
            var monthFolderFormat = cs.GetAppSetting(c.MonthFolderFormatKey);
            var monthFolderDirectory = date.ToString(monthFolderFormat);
            var filePath = fileSource == FileSource.Envision ? cs.GetAppSetting(c.EnvisionFilePathKey) : cs.GetAppSetting(c.LakerFilePathKey);
            var pathWithMonthDirectory = Path.Combine(filePath, monthFolderDirectory);
            return pathWithMonthDirectory;
        }

        internal static bool CreateMonthAndYearFolderIfNecessary(FileSource fileSource)
        {
            try
            {
                // Create a folder
                var pathWithMonthDirectory = GetFullLocalFilePathPlusMonthYearFolderByDate(DateTime.Now, fileSource);
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

        internal static ImportFileModel BrowseDirectoryToLocateFile()
        {
            try
            {
                var directoryInfo = new DirectoryInfo(GetFullLocalFilePathPlusMonthYearFolderByDate(DateTime.Now, FileSource.Laker));
                var files = directoryInfo.GetFiles()
                    .Where(x => !string.IsNullOrWhiteSpace(x.Name) && x.Name.StartsWith("Billing_Claim_File_") && x.Name.EndsWith(".csv"))
                    .OrderByDescending(p => p.CreationTime)
                    .Take(Convert.ToInt32(cs.GetAppSetting(c.FileProcessorTopNumberKey))).ToList();
                // Now traverse the top, however many files to find the latest.
                var newFiles = files.Select(s => new ImportFileModel
                {
                    FileName = s.Name,
                    FullFileName = s.FullName
                }).ToList();
                if (newFiles.Count < 1)
                {
                    return null;
                }
                var newestFullFileName = newFiles.OrderByDescending(x => x.FileDate).FirstOrDefault();
                return newestFullFileName;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        internal static IList<ImportFileModel> BrowseDirectoryForTenEnvisionFiles()
        {
            try
            {
                var take = Convert.ToInt32(cs.GetAppSetting(c.EnvisionFileProcessingCountKey));
                var directoryInfo = new DirectoryInfo(GetFullLocalFilePathPlusMonthYearFolderByDate(DateTime.Now, FileSource.Envision));
                var files = directoryInfo.GetFiles().Where(x =>
                    !string.IsNullOrWhiteSpace(x.Name) && x.Name.StartsWith("ENVexport_BPC_") &&
                    x.Name.EndsWith(".csv"));
                var newFiles = files.Select(x => new ImportFileModel
                {
                    FileName = x.Name,
                    FullFileName = x.FullName
                }).ToList();
                if (newFiles.Count < 1)
                {
                    return null;
                }
                // Hard-coding the date (per Adam) never to pull any files prior to 6/22/2019.
                var firstDate = new DateTime(2019, 6, 22);
                var lastFiles = newFiles.Where(x => x.FileDate >= firstDate)
                    .OrderByDescending(x => x.FileDate).Take(take).ToList();
                return lastFiles;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
