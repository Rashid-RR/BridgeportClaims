using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using BridgeportClaims.Business.ApiProvider;
using BridgeportClaims.Business.DAL;
using BridgeportClaims.Business.Dto;
using BridgeportClaims.Business.Enums;
using BridgeportClaims.Business.Extensions;
using BridgeportClaims.Business.IO;
using BridgeportClaims.Business.Logging;
using BridgeportClaims.Business.URL;
using NLog;
using c = BridgeportClaims.Business.StringConstants.Constants;
using cs = BridgeportClaims.Business.ConfigService.ConfigService;


namespace BridgeportClaims.Business.Providers
{
    [SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable")]
    public abstract class FileWatcherProvider
    {
        private readonly FileSystemWatcher _fileWatcher;
        private static readonly LoggingService LoggingService = LoggingService.Instance;
        private static readonly Logger Logger = LoggingService.Logger;
        private readonly DocumentDataProvider _documentDataProvider;
        private readonly string _pathToRemove;
        private readonly string _rootDomain;
        private FileType InternalFileType { get; }

        private static string GetFileSize(long length) => IoHelper.GetFileSize(length);

        protected FileWatcherProvider(FileType fileType)
        {
            InternalFileType = fileType;
            _pathToRemove = cs.GetAppSetting(fileType == FileType.Images ? c.ImagesFileLocationKey :
                fileType == FileType.Invoices ? c.InvoicesFileLocationKey :
                throw new Exception($"Error, could not find a valid file type for arguement {nameof(fileType)}"));
            _rootDomain = cs.GetAppSetting(fileType == FileType.Images ? c.ImagesRootDomainNameKey :
                fileType == FileType.Invoices ? c.InvoicesRootDomainNameKey :
                throw new Exception($"Error, could not find a valid file type for arguement {nameof(fileType)}"));
            _documentDataProvider = new DocumentDataProvider();
            _fileWatcher = new FileSystemWatcher(GetFileLocation(fileType))
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.LastAccess |
                               NotifyFilters.LastWrite | NotifyFilters.Size
            };
            _fileWatcher.Created += _fileWatcher_Created;
            _fileWatcher.Deleted += _fileWatcher_Deleted;
            _fileWatcher.Changed += _fileWatcher_Changed;
            _fileWatcher.Renamed += _fileWatcher_Renamed;
            _fileWatcher.Error += _fileWatcher_Error;
            _fileWatcher.EnableRaisingEvents = true;
            _fileWatcher.IncludeSubdirectories = true;
        }

        private bool FileWasModified(FileInfo fileInfo, string oldFileName = null)
        {
            var dbDoc = _documentDataProvider.GetDocumentByFileName(oldFileName ?? fileInfo.FullName, (byte) InternalFileType);
            if (null == dbDoc)
                return true;
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

        private static void CallSignalRApi(SignalRMethodType type, FileInfo fileInfo, string fileSize, string url, int documentId, FileType fileType)
        {
            if (null == fileInfo && type != SignalRMethodType.Delete)
                throw new ArgumentNullException(nameof(fileInfo));
            DocumentDto dto = null;
            if (type != SignalRMethodType.Delete)
            {
                dto = new DocumentDto
                {
                    DocumentId = documentId,
                    CreationTimeLocal = fileInfo.CreationTime,
                    DirectoryName = fileInfo.DirectoryName,
                    Extension = fileInfo.Extension,
                    FileName = fileInfo.Name,
                    FileSize = fileSize,
                    FileUrl = url,
                    FullFilePath = fileInfo.FullName,
                    LastAccessTimeLocal = fileInfo.LastAccessTime,
                    LastWriteTimeLocal = fileInfo.LastWriteTime,
                    ByteCount = fileInfo.Length,
                    FileTypeId = (byte)fileType
                };
            }
            var apiClient = new ApiCallerProvider();
            var token = apiClient.GetAuthenticationBearerTokenAsync().GetAwaiter().GetResult();
            if (token.IsNullOrWhiteSpace())
                throw new Exception("Error, could not obtain a valid bearer token. Please ensure that the correct username and password are being used.");
            var succeededApiCall = apiClient.CallSignalRApiMethod(type, token, dto, documentId).GetAwaiter().GetResult();
            if (!succeededApiCall)
                throw new Exception("Error, the API call to \"CallSignalRApiMethod\" failed.");
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
                var documentId = _documentDataProvider.InsertDocument(fileInfo.Name, fileInfo.Extension, fileSize, fileInfo.CreationTime,
                    fileInfo.LastAccessTime, fileInfo.LastWriteTime, fileInfo.DirectoryName, fileInfo.FullName, url, fileInfo.Length, (byte) InternalFileType);
                // Api Call
                if (cs.AppIsInDebugMode)
                    Logger.Info($"Starting the API call to add document {fileInfo.Name} within {methodName} method on {now}.");
                CallSignalRApi(SignalRMethodType.Add, fileInfo, fileSize, url, documentId, InternalFileType);
                if (!cs.AppIsInDebugMode)
                {
                    return;
                }
                LoggingService.LogDebugMessage(methodName, now, $"The DocumentID {documentId} was created.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        //  This method is called when the FileSystemWatcher detects an error.
        private static void _fileWatcher_Error(object source, ErrorEventArgs e)
        {
            try
            {
                var ex = e?.GetException();
                if (null != ex)
                {
                    if (ex.GetType() == typeof(InternalBufferOverflowException))
                    {
                        //  This can happen if Windows is reporting many file system events quickly 
                        //  and internal buffer of the  FileSystemWatcher is not large enough to handle this
                        //  rate of events. The InternalBufferOverflowException error informs the application
                        //  that some of the file system events are being lost.
                        Logger.Fatal(ex,
                            $"The file system watcher experienced an internal buffer overflow: {ex.Message}");
                    }

                    Logger.Error(ex, $"The FileSystemWatcher has detected an error: {ex.Message}");
                }
                else
                {
                    Logger.Error(
                        $"An error was detected in the File System Watcher, but an Exception could not be retrieved from the {nameof(ErrorEventArgs)} class.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        private void _fileWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            try
            {
                if (e.FullPath.Right(4) != ".pdf")
                    return;
                if (File.GetAttributes(e.FullPath).HasFlag(FileAttributes.Directory))
                    return; //ignore directories, only process files
                // Ensure that the changed file has at least one changed property worth updating
                var fileInfo = new FileInfo(e.FullPath);
                var needsModification = FileWasModified(fileInfo, e.OldFullPath);
                if (!needsModification)
                    return; // no modification necessary.
                var methodName = MethodBase.GetCurrentMethod().Name;
                var now = DateTime.Now.ToString(LoggingService.TimeFormat);
                var fileSize = GetFileSize(fileInfo.Length);
                var url = UrlHelper.GetUrlFromFullFileName(fileInfo.FullName, _rootDomain, _pathToRemove);
                // Database call
                if (cs.AppIsInDebugMode)
                    Logger.Info($"Starting the database call to update document {fileInfo.Name} within {methodName} method on {now}.");
                var documentId = _documentDataProvider.GetDocumentIdByDocumentName(e.OldFullPath, (byte)InternalFileType);
                _documentDataProvider.UpdateDocument(documentId, fileInfo.Name, fileInfo.Extension, fileSize, fileInfo.CreationTime, fileInfo.LastAccessTime,
                    fileInfo.LastWriteTime, fileInfo.DirectoryName, fileInfo.FullName, url, fileInfo.Length, (byte)InternalFileType);
                // Api Call
                if (cs.AppIsInDebugMode)
                    Logger.Info($"Starting the API call to update document {fileInfo.Name} within {methodName} method on {now}.");
                CallSignalRApi(SignalRMethodType.Modify, fileInfo, fileSize, url, documentId, InternalFileType);
                if (!cs.AppIsInDebugMode)
                {
                    return;
                }
                LoggingService.LogDebugMessage(methodName, now, $"The DocumentID {documentId} was renamed.");

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
                    Logger.Info($"Starting the database call to update document {fileInfo.Name} within {methodName} method on {now}.");
                var documentId = _documentDataProvider.GetDocumentIdByDocumentName(fileInfo.FullName, (byte) InternalFileType);
                _documentDataProvider.UpdateDocument(documentId, fileInfo.Name, fileInfo.Extension, fileSize, fileInfo.CreationTime, fileInfo.LastAccessTime,
                    fileInfo.LastWriteTime, fileInfo.DirectoryName, fileInfo.FullName, url, fileInfo.Length, (byte) InternalFileType);
                // Api Call
                if (cs.AppIsInDebugMode)
                    Logger.Info($"Starting the API call to update document {fileInfo.Name} within {methodName} method on {now}.");
                CallSignalRApi(SignalRMethodType.Modify, fileInfo, fileSize, url, documentId, InternalFileType);
                if (!cs.AppIsInDebugMode)
                {
                    return;
                }
                LoggingService.LogDebugMessage(methodName, now, $"The DocumentID {documentId} was updated.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        private static string GetFileLocation(FileType fileType)
        {
            try
            {
                if (cs.AppIsInDebugMode)
                {
                    var method = MethodBase.GetCurrentMethod().Name;
                    var now = DateTime.Now.ToString(LoggingService.TimeFormat);
                    LoggingService.LogDebugMessage(method, now);
                }
                var value = cs.GetAppSetting(fileType == FileType.Images ? c.ImagesFileLocationKey :
                    fileType == FileType.Invoices ? c.InvoicesFileLocationKey :
                    throw new Exception($"Error, could not find a valid type for arguement {nameof(fileType)}"));
                return value.IsNotNullOrWhiteSpace() ? value : string.Empty;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
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
                var documentId = _documentDataProvider.GetDocumentIdByDocumentName(e.FullPath, (byte) InternalFileType);
                if (documentId != default(int))
                {
                    _documentDataProvider.DeleteDocument(documentId);
                    if (!cs.AppIsInDebugMode)
                    {
                        return;
                    }
                    LoggingService.LogDebugMessage(methodName, now, $"The DocumentID {documentId} was deleted.");
                }
                else
                {
                    if (cs.AppIsInDebugMode)
                    {
                        LoggingService.LogDebugMessage(methodName, now,
                            $"The document with the full file path: {e.FullPath} was not found in the database.");
                    }
                }
                // Api Call
                if (cs.AppIsInDebugMode)
                {
                    Logger.Info(
                        $"Starting the API call to add document {fileName} within {methodName} method on {now}.");
                }
                CallSignalRApi(SignalRMethodType.Delete, null, null, null, documentId, InternalFileType);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}