using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using BridgeportClaims.Business.Dto;
using BridgeportClaims.Business.Enums;
using BridgeportClaims.Business.Logging;
using BridgeportClaims.Business.URL;
using NLog;
using c = BridgeportClaims.Business.StringConstants.Constants;
using cs = BridgeportClaims.Business.ConfigService.ConfigService;

namespace BridgeportClaims.Business.Helpers.IO
{
    public class IoHelper : IIoHelper
    {
        private readonly Lazy<ILogger> _logger;

        public IoHelper(Lazy<ILogger> logger)
        {
            _logger = logger;
        }

        internal static string GetFileSize(double byteCount)
        {
            var size = "0 Bytes";
            if (byteCount >= 1073741824.0)
                size = $"{byteCount / 1073741824.0:##.##}" + " GB";
            else if (byteCount >= 1048576.0)
                size = $"{byteCount / 1048576.0:##.##}" + " MB";
            else if (byteCount >= 1024.0)
                size = $"{byteCount / 1024.0:##.##}" + " KB";
            else if (byteCount > 0 && byteCount < 1024.0)
                size = byteCount.ToString(CultureInfo.InvariantCulture) + " Bytes";
            return size;
        }

        public IEnumerable<DocumentDto> TraverseDirectories(string path, string rootDomain, FileType fileType)
        {
            try
            {
                var files = Directory.EnumerateFiles(path, c.PdfPattern, SearchOption.AllDirectories)
                    .Where(x => !x.Contains($@"\{c.Prints}\")).ToList();
                var pathToRemove = cs.GetFileLocationByFileType(fileType);
                if (!files.Any())
                    return null;
                return files.Select(file => new FileInfo(file))
                    .Select(f => new DocumentDto
                    {
                        CreationTimeLocal = f.CreationTime,
                        DirectoryName = f.DirectoryName,
                        Extension = f.Extension,
                        FileName = f.Name,
                        FileSize = GetFileSize(f.Length),
                        FileUrl = UrlHelper.GetUrlFromFullFileName(f.FullName, rootDomain, pathToRemove),
                        FullFilePath = f.FullName,
                        LastAccessTimeLocal = f.LastAccessTime,
                        LastWriteTimeLocal = f.LastWriteTime,
                        ByteCount = f.Length,
                        FileTypeId = (byte) fileType
                    })
                    .AsEnumerable();
            }
            catch (Exception ex)
            {
                var method = MethodBase.GetCurrentMethod().Name;
                var now = DateTime.Now.ToString(LoggingService.TimeFormat);
                if (cs.AppIsInDebugMode)
                    _logger.Value.Debug($"Debugging inside of the {method} method on {now}.");
                _logger.Value.Error(ex);
                return null;
            }
        }
    }
}