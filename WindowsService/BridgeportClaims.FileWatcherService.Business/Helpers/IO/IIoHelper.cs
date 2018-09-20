using System.Collections.Generic;
using BridgeportClaims.Business.Dto;
using BridgeportClaims.Business.Enums;

namespace BridgeportClaims.Business.Helpers.IO
{
    public interface IIoHelper
    {
        IEnumerable<DocumentDto> TraverseDirectories(string path, string rootDomain, FileType fileType);
    }
}