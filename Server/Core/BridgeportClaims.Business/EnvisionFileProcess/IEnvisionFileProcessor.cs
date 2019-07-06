using System;

namespace BridgeportClaims.Business.EnvisionFileProcess
{
    public interface IEnvisionFileProcessor
    {
        Tuple<string, string> ProcessEnvisionFile(int importFileId);
    }
}