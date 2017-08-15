using System.Collections.Generic;
using System.IO;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.ImportFile
{
    public interface IImportFileProvider
    {
        void DeleteImportFile(int importFileId);
        IList<ImportFileDto> GetImportFileDtos();
        void MarkFileProcessed(string fileName);
        void SaveFileToDatabase(Stream stream, string fileName, string fileExtension, 
            string fileDescription);
    }
}