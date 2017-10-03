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
        internal FileDateParsingHelper BrowseDirectoryToLocateFile()
        {
            try
            {
                var directoryInfo = new DirectoryInfo(cs.GetAppSetting(c.LakerFilePathKey));
                var files = directoryInfo.GetFiles().OrderByDescending(p => p.CreationTime)
                    .Take(Convert.ToInt32(cs.GetAppSetting(c.LakerFileTopNumberKey))).ToList();
                // Now traverse the top, however many files to find the latest.
                var newFiles = files.Select(s => new FileDateParsingHelper
                {
                    FileName = s.Name,
                    FullFileName = s.FullName
                }).ToList();
                if (newFiles.Count < 1)
                    return null;
                var newestFullFileName = newFiles.OrderByDescending(x => x.FileNameExtractedDate).FirstOrDefault();
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
