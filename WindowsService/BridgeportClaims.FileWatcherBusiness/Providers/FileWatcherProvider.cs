using NLog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using BridgeportClaims.FileWatcherBusiness.Extensions;
using BridgeportClaims.FileWatcherBusiness.Logging;
using c = BridgeportClaims.FileWatcherBusiness.StringConstants.Constants;
using cm = BridgeportClaims.FileWatcherBusiness.ConfigService.ConfigService;

namespace BridgeportClaims.FileWatcherBusiness.Providers
{
    [SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable")]
    public class FileWatcherProvider
    {
        private readonly FileSystemWatcher _fileWatcher;
        private static readonly Logger Logger = LoggingService.Instance.Logger;

        public FileWatcherProvider()
        {
            _fileWatcher = new FileSystemWatcher(GetFileLocation());
            _fileWatcher.Created += _fileWatcher_Created;
            _fileWatcher.Deleted += _fileWatcher_Deleted;
            _fileWatcher.Changed += _fileWatcher_Changed;
            _fileWatcher.EnableRaisingEvents = true;
            _fileWatcher.IncludeSubdirectories = true;
        }

        private static string GetFileLocation()
        {
            try
            {
                var value = cm.GetAppSetting(c.FileLocationKey);
                return value.IsNotNullOrWhiteSpace() ? value : string.Empty;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        private static void _fileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            

        }

        private static void _fileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            
        }

        private static void _fileWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            
        }
    }
}