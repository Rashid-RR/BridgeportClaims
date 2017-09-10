using System;
using NLog;
using BridgeportClaims.Data.DataProviders.ImportFiles;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Business.LakerFileProcess
{
    public class LakerFileProcessor : ILakerFileProcessor
    {
        private readonly IImportFileProvider _importFileProvider;

        public LakerFileProcessor(IImportFileProvider importFileProvider)
        {
            _importFileProvider = importFileProvider;
        }

        public Tuple<string, string> ProcessOldestLakerFile()
        {
            // First, grab the bytes of the file from the database.
            var tuple = _importFileProvider.GetOldestLakerFileBytes();
            if (null == tuple)
                return new Tuple<string, string>(c.NoLakerFilesToImportToast, null);
            // Which we'll return if this whole process is successful.
            var lakerFileName = tuple.Item1;
            // Take the file bytes, and save them to a temporary path in Windows
            var fullLakerFileTemporaryPath = _importFileProvider.GetLakerFileTemporaryPath(tuple);
            return new Tuple<string, string>(lakerFileName, fullLakerFileTemporaryPath);
        }
    }
}