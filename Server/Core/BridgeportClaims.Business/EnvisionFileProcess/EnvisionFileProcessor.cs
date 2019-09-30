using System;
using s = BridgeportClaims.Common.Constants.StringConstants;
using BridgeportClaims.Data.DataProviders.ImportFiles;

namespace BridgeportClaims.Business.EnvisionFileProcess
{
    public class EnvisionFileProcessor : IEnvisionFileProcessor
    {
        private readonly Lazy<IImportFileProvider> _importFileProvider;

        public EnvisionFileProcessor(Lazy<IImportFileProvider> importFileProvider)
        {
            _importFileProvider = importFileProvider;
        }

        public Tuple<string, string> ProcessEnvisionFile(int importFileId = -1)
        {
            // First, grab the bytes of the file from the database.
            var tuple = importFileId != -1
                ? _importFileProvider.Value.GetEnvisionFileBytes(importFileId)
                : _importFileProvider.Value.GetOldestEnvisionFileBytes();
            if (null == tuple)
            {
                return new Tuple<string, string>(s.NoEnvisionFilesFound, null);
            }
            // Which we'll return if this whole process is successful.
            var envisionFileName = tuple.Item1;
            // Take the file bytes, and save them to a temporary path in Windows
            var fullEnvisionFileTemporaryPath = _importFileProvider.Value.GetEnvisionFileTemporaryPath(tuple);
            return new Tuple<string, string>(envisionFileName, fullEnvisionFileTemporaryPath);
        }
    }
}