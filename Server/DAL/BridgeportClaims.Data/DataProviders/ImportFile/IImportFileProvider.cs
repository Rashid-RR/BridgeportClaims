using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.ImportFile
{
    public interface IImportFileProvider
    {
        Task DeleteImportFile(int importFileId);
        IList<ImportFileDto> GetImportFileDtos();
        void MarkFileProcessed(string fileName);
        void SaveFileToDatabase(Stream stream, string fileName, string fileExtension, 
            string fileDescription);
    }
}