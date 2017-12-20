using NLog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using BridgeportClaims.FileWatcherBusiness.ApiProvider;
using BridgeportClaims.FileWatcherBusiness.DAL;
using BridgeportClaims.FileWatcherBusiness.Dto;
using BridgeportClaims.FileWatcherBusiness.Enums;
using BridgeportClaims.FileWatcherBusiness.Extensions;
using BridgeportClaims.FileWatcherBusiness.IO;
using BridgeportClaims.FileWatcherBusiness.Logging;
using BridgeportClaims.FileWatcherBusiness.URL;
using c = BridgeportClaims.FileWatcherBusiness.StringConstants.Constants;
using cs = BridgeportClaims.FileWatcherBusiness.ConfigService.ConfigService;

namespace BridgeportClaims.FileWatcherBusiness.Providers
{
    [SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable")]
    public class FileWatcherProvider
    {
        private readonly FileSystemWatcher _fileWatcher;
        private static readonly LoggingService LoggingService = LoggingService.Instance;
        private static readonly Logger Logger = LoggingService.Logger;
        private readonly ImageDataProvider _imageDataProvider;
        private readonly string _pathToRemove = cs.GetAppSetting(c.FileLocationKey);
        private readonly string _rootDomain = cs.GetAppSetting(c.ImagesRootDomainNameKey);

        private static string GetFileSize(long length) => IoHelper.GetFileSize(length);

        public FileWatcherProvider()
        {
            _imageDataProvider = new ImageDataProvider();
            _fileWatcher = new FileSystemWatcher(GetFileLocation())
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.LastAccess |
                               NotifyFilters.LastWrite
            };
            _fileWatcher.Created += _fileWatcher_Created;
            _fileWatcher.Deleted += _fileWatcher_Deleted;
            _fileWatcher.Changed += _fileWatcher_Changed;
            _fileWatcher.EnableRaisingEvents = true;
            _fileWatcher.IncludeSubdirectories = true;
        }

        private static void CallSignalRApi(SignalRMethodType type, FileInfo fileInfo, string fileSize, string url)
        {
            var dto = new DocumentDto
            {
                CreationTimeLocal = fileInfo.CreationTime,
                DirectoryName = fileInfo.DirectoryName,
                Extension = fileInfo.Extension,
                FileName = fileInfo.Name,
                FileSize = fileSize,
                FileUrl = url,
                FullFilePath = fileInfo.FullName,
                LastAccessTimeLocal = fileInfo.LastAccessTime,
                LastWriteTimeLocal = fileInfo.LastWriteTime,
                ByteCount = fileInfo.Length
            };
            var apiClient = new ApiCallerProvider();
            var token = apiClient.GetAuthenticationBearerTokenAsync().GetAwaiter().GetResult();
            var succeededApiCall = apiClient.CallSignalRApiMethod(type, token, dto).GetAwaiter().GetResult();
            if (!succeededApiCall)
                throw new Exception("Error, the API call to \"CallSignalRApiMethod\" failed.");
        }

        private static string GetFileLocation()
        {
            try
            {
                if (cs.AppIsInDebugMode)
                {
                    var method = MethodBase.GetCurrentMethod().Name;
                    var now = DateTime.Now.ToString(LoggingService.TimeFormat);
                    LoggingService.LogDebugMessage(method, now);
                }
                var value = cs.GetAppSetting(c.FileLocationKey);
                return value.IsNotNullOrWhiteSpace() ? value : string.Empty;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        private void _fileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (e.FullPath.Right(4) != ".pdf")
                    return;
                if (File.GetAttributes(e.FullPath).HasFlag(FileAttributes.Directory))
                    return; //ignore directories, only process files
                var methodName = MethodBase.GetCurrentMethod().Name;
                var now = DateTime.Now.ToString(LoggingService.TimeFormat);
                var fileInfo = new FileInfo(e.FullPath);
                var fileSize = GetFileSize(fileInfo.Length);
                var url = UrlHelper.GetUrlFromFullFileName(fileInfo.FullName, _rootDomain, _pathToRemove);
                // Database call
                if (cs.AppIsInDebugMode)
                    Logger.Info($"Starting the database call to insert document {fileInfo.Name} within {methodName} method on {now}.");
                var documentId = _imageDataProvider.InsertDocument(fileInfo.Name, fileInfo.Extension, fileSize, fileInfo.CreationTime,
                    fileInfo.LastAccessTime, fileInfo.LastWriteTime, fileInfo.DirectoryName, fileInfo.FullName, url, fileInfo.Length);
                // Api Call
                if (cs.AppIsInDebugMode)
                    Logger.Info($"Starting the API call to add document {fileInfo.Name} within {methodName} method on {now}.");
                CallSignalRApi(SignalRMethodType.Add, fileInfo, fileSize, url);
                if (!cs.AppIsInDebugMode) return;
                LoggingService.LogDebugMessage(methodName, now, $"The DocumentID {documentId} was created.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        private void _fileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (e.FullPath.Right(4) != ".pdf")
                    return;
                if (File.GetAttributes(e.FullPath).HasFlag(FileAttributes.Directory))
                    return; //ignore directories, only process files
                // Ensure that the changed file has at least one changed property worth updating
                var fileInfo = new FileInfo(e.FullPath);
                var needsModification = FileWasModified(fileInfo);
                if (!needsModification)
                    return; // no modification necessary.
                var methodName = MethodBase.GetCurrentMethod().Name;
                var now = DateTime.Now.ToString(LoggingService.TimeFormat);
                var fileSize = GetFileSize(fileInfo.Length);
                var url = UrlHelper.GetUrlFromFullFileName(fileInfo.FullName, _rootDomain, _pathToRemove);
                // Database call
                if (cs.AppIsInDebugMode)
                    Logger.Info($"Starting the database call to insert document {fileInfo.Name} within {methodName} method on {now}.");
                var documentId = _imageDataProvider.GetDocumentIdByDocumentName(fileInfo.Name);
                _imageDataProvider.UpdateDocument(documentId, fileInfo.Name, fileInfo.Extension, fileSize, fileInfo.CreationTime, fileInfo.LastAccessTime, 
                    fileInfo.LastWriteTime, fileInfo.DirectoryName, fileInfo.FullName, url, fileInfo.Length);
                // Api Call
                if (cs.AppIsInDebugMode)
                    Logger.Info($"Starting the API call to add document {fileInfo.Name} within {methodName} method on {now}.");
                CallSignalRApi(SignalRMethodType.Modify, fileInfo, fileSize, url);
                if (!cs.AppIsInDebugMode) return;
                LoggingService.LogDebugMessage(methodName, now, $"The DocumentID {documentId} was updated.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        private bool FileWasModified(FileInfo fileInfo)
        {
            var dbDoc = _imageDataProvider.GetDocumentByFileName(fileInfo.Name);
            var url = UrlHelper.GetUrlFromFullFileName(fileInfo.FullName, _rootDomain, _pathToRemove);
            return dbDoc.FileName != fileInfo.Name ||
                   dbDoc.Extension != fileInfo.Extension ||
                   dbDoc.FileSize != GetFileSize(fileInfo.Length) ||
                   dbDoc.CreationTimeLocal != fileInfo.CreationTime ||
                   dbDoc.LastAccessTimeLocal != fileInfo.LastAccessTime ||
                   dbDoc.LastWriteTimeLocal != fileInfo.LastWriteTime ||
                   dbDoc.DirectoryName != fileInfo.DirectoryName ||
                   dbDoc.FullFilePath != fileInfo.FullName ||
                   dbDoc.FileUrl != url ||
                   dbDoc.ByteCount != fileInfo.Length;
        }

        private void _fileWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (e.FullPath.Right(4) != ".pdf") // This is already, by nature, ignore directories.
                    return;
                var methodName = MethodBase.GetCurrentMethod().Name;
                var now = DateTime.Now.ToString(LoggingService.TimeFormat);
                var fileName = Path.GetFileName(e.FullPath);
                var fileInfo = new FileInfo(e.FullPath);
                var fileSize = GetFileSize(fileInfo.Length);
                var url = UrlHelper.GetUrlFromFullFileName(fileInfo.FullName, _rootDomain, _pathToRemove);
                var documentId = _imageDataProvider.GetDocumentIdByDocumentName(fileName);
                if (documentId != default(int))
                {
                    _imageDataProvider.DeleteDocument(documentId);
                    if (!cs.AppIsInDebugMode) return;
                    LoggingService.LogDebugMessage(methodName, now, $"The DocumentID {documentId} was deleted.");
                }
                else
                {
                    if (cs.AppIsInDebugMode)
                        LoggingService.LogDebugMessage(methodName, now, $"The document with the name {fileName} was not found in the database.");
                }
                // Api Call
                if (cs.AppIsInDebugMode)
                    Logger.Info($"Starting the API call to add document {fileInfo.Name} within {methodName} method on {now}.");
                CallSignalRApi(SignalRMethodType.Delete, fileInfo, fileSize, url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}