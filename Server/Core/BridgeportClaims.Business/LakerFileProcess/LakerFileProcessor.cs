using System;
using BridgeportClaims.Common.Constants;
using BridgeportClaims.Data.DataProviders.ImportFiles;

namespace BridgeportClaims.Business.LakerFileProcess
{
    public class LakerFileProcessor : ILakerFileProcessor
    {
        private readonly Lazy<IImportFileProvider> _importFileProvider;

        public LakerFileProcessor(Lazy<IImportFileProvider> importFileProvider)
        {
            _importFileProvider = importFileProvider;
        }

        public Tuple<string, string> ProcessOldestLakerFile()
        {
            // First, grab the bytes of the file from the database.
            var tuple = _importFileProvider.Value.GetOldestLakerFileBytes();
            if (null == tuple)
            {
                return new Tuple<string, string>(StringConstants.NoLakerFilesToImportToast, null);
            }
            // Which we'll return if this whole process is successful.
            var lakerFileName = tuple.Item1;
            // Take the file bytes, and save them to a temporary path in Windows
            var fullLakerFileTemporaryPath = _importFileProvider.Value.GetLakerFileTemporaryPath(tuple);
            return new Tuple<string, string>(lakerFileName, fullLakerFileTemporaryPath);
        }
    }
}