using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using BridgeportClaims.Common.Enums;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.ImportFiles
{
    public interface IImportFileProvider
    {
        void ImportEnvisionFile(DataTable dt);
        void ImportDataTableIntoDatabase(DataTable dataTable, bool debugOnly = false);
        string GetEnvisionFileTemporaryPath(Tuple<string, byte[]> tuple);
        string GetLakerFileTemporaryPath(Tuple<string, byte[]> tuple);
        DataTable RetrieveDataTableFromFullFilePath(string fullFilePath, FileSource fileSource);
        void DeleteImportFile(int importFileId);
        Tuple<string, byte[]> GetOldestLakerFileBytes();
        Tuple<string, byte[]> GetEnvisionFileBytes(int importFileId);
        IList<ImportFileDto> GetImportFiles();
        void MarkFileProcessed(string fileName);
        void SaveFileToDatabase(Stream stream, string fileName, string fileExtension, string fileDescription);
        void EtlLakerFile(string fileName);
    }
}