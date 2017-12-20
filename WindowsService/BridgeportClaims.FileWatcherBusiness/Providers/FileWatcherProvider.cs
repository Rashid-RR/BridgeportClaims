﻿using NLog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
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
                var f = new FileInfo(e.FullPath);
                var fileSize = IoHelper.GetFileSize(f.Length);
                var pathToRemove = cs.GetAppSetting(c.FileLocationKey);
                var rootDomain = cs.GetAppSetting(c.ImagesRootDomainNameKey);
                var url = UrlHelper.GetUrlFromFullFileName(f.FullName, rootDomain, pathToRemove);
                // Database call
                var documentId = _imageDataProvider.InsertDocument(f.Name, f.Extension, fileSize, f.CreationTime,
                    f.LastAccessTime, f.LastWriteTime, f.DirectoryName, f.FullName, url, f.Length);
                // Api Call
                var dto = new DocumentDto
                {
                    CreationTimeLocal = f.CreationTime,
                    DirectoryName = f.DirectoryName,
                    Extension = f.Extension,
                    FileName = f.Name,
                    FileSize = fileSize,
                    FileUrl = url,
                    FullFilePath = f.FullName,
                    LastAccessTimeLocal = f.LastAccessTime,
                    LastWriteTimeLocal = f.LastWriteTime,
                    ByteCount = f.Length
                };
                var apiClient = new ApiCallerProvider();
                var token = apiClient.GetAuthenticationBearerTokenAsync().GetAwaiter().GetResult();
                var succeededApiCall = apiClient.CallSignalRApiMethod(SignalRMethodType.Add, token, dto).GetAwaiter().GetResult();
                if (!succeededApiCall)
                    throw new Exception("Error, the API call to \"CallSignalRApiMethod\" failed.");
                if (!cs.AppIsInDebugMode) return;
                var methodName = MethodBase.GetCurrentMethod().Name;
                var now = DateTime.Now.ToString(LoggingService.TimeFormat);
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
                var f = new FileInfo(e.FullPath);
                var fileSize = IoHelper.GetFileSize(f.Length);
                var pathToRemove = cs.GetAppSetting(c.FileLocationKey);
                var rootDomain = cs.GetAppSetting(c.ImagesRootDomainNameKey);
                var url = UrlHelper.GetUrlFromFullFileName(f.FullName, rootDomain, pathToRemove);
                var documentId = _imageDataProvider.GetDocumentIdByDocumentName(f.Name);
                _imageDataProvider.UpdateDocument(documentId, f.Name, f.Extension, fileSize, f.CreationTime, f.LastAccessTime, 
                    f.LastWriteTime, f.DirectoryName, f.FullName, url, f.Length);
                if (!cs.AppIsInDebugMode) return;
                var methodName = MethodBase.GetCurrentMethod().Name;
                var now = DateTime.Now.ToString(LoggingService.TimeFormat);
                LoggingService.LogDebugMessage(methodName, now, $"The DocumentID {documentId} was updated.");
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
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}