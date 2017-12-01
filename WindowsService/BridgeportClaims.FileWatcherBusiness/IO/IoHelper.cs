using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BridgeportClaims.FileWatcherBusiness.Dto;
using BridgeportClaims.FileWatcherBusiness.URL;

namespace BridgeportClaims.FileWatcherBusiness.IO
{
    public static class IoHelper
    {
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

        public static IEnumerable<DocumentDto> TraverseDirectories(string path)
        {
            var files = Directory.EnumerateFiles(path, "*.pdf", SearchOption.AllDirectories).ToList();
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
                    FileUrl = UrlHelper.GetUrlFromFullFileName(f.FullName),
                    FullFilePath = f.FullName,
                    LastAccessTimeLocal = f.LastAccessTime,
                    LastWriteTimeLocal = f.LastWriteTime
                })
                .AsEnumerable();
        }
    }
}