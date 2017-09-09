using System;

namespace BridgeportClaims.Business.LakerFileProcess
{
    public interface ILakerFileProcessor
    {
        Tuple<string, string> ProcessOldestLakerFile();
    }
}