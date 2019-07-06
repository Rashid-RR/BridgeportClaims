using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.ImportFiles
{
    public interface IImportFileProvider
    {
        void LakerImportFileProcedureCall(DataTable dataTable, bool debugOnly = false);
        string GetEnvisionFileTemporaryPath(Tuple<string, byte[]> tuple);
        string GetLakerFileTemporaryPath(Tuple<string, byte[]> tuple);
        DataTable RetrieveDataTableFromLatestLakerFile(string fullFilePathOfLatestLakerFile);
        void DeleteImportFile(int importFileId);
        Tuple<string, byte[]> GetOldestLakerFileBytes();
        Tuple<string, byte[]> GetEnvisionFileBytes(int importFileId);
        IList<ImportFileDto> GetImportFileDtos();
        void MarkFileProcessed(string fileName);
        void SaveFileToDatabase(Stream stream, string fileName, string fileExtension, string fileDescription);
        void EtlLakerFile(string fileName);
    }
}