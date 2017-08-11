using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.ImportFile
{
    public interface IImportFileProvider
    {
        IList<ImportFileDto> GetImportFileDtos();
    }
}