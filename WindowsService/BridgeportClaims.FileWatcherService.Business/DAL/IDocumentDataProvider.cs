using System;
using System.Data;
using BridgeportClaims.Business.Dto;
using BridgeportClaims.Business.Enums;

namespace BridgeportClaims.Business.DAL
{
    public interface IDocumentDataProvider
    {
        DocumentDto GetDocumentByFileName(string fullFileName, byte fileTypeId);
        void DeleteDocument(int documentId);
        int GetDocumentIdByDocumentName(string fullFileName, byte fileTypeId);

        void UpdateDocument(int documentId, string fileName, string extension, string fileSize, DateTime creationTime, DateTime lastAccessTime,
            DateTime lastWriteTime, string directoryName, string fullFilePath, string fileUrl, DateTime? documentDate, long byteCount, byte fileTypeId);

        int InsertDocument(string fileName, string extension, string fileSize, DateTime creationTime, DateTime lastAccessTime,
            DateTime lastWriteTime, string directoryName, string fullFilePath, string fileUrl, DateTime? documentDate,
            long byteCount, byte fileTypeId);

        void MergeDocuments(DataTable dt, FileType fileType);
    }
}