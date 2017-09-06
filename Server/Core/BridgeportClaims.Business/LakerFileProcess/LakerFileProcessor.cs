using BridgeportClaims.Data.DataProviders.ImportFiles;

namespace BridgeportClaims.Business.LakerFileProcess
{
    public class LakerFileProcessor : ILakerFileProcessor
    {
        private readonly IImportFileProvider _importFileProvider;

        public LakerFileProcessor(IImportFileProvider importFileProvider)
        {
            _importFileProvider = importFileProvider;
        }

        public string ProcessOldestLakerFile()
        {
            // First, grab the bytes of the file from the database.
            var tuple = _importFileProvider.GetOldestLakerFileBytes();
            // Which we'll return if this whole process is successful.
            var lakerFileName = tuple.Item1;
            // Take the file bytes, and save them to a temporary path in Windows
            var fullLakerFileTemporaryPath = _importFileProvider.GetLakerFileTemporaryPath(tuple);
            // Take a third-party CSV reader, and turn that temporarily saved laker file into a Data Table.
            var dataTable = _importFileProvider.RetreiveDataTableFromLatestLakerFile(fullLakerFileTemporaryPath);
            // Import the new file, into the new Staged Laker File that will be imported into the database
             _importFileProvider.LakerImportFileProcedureCall(dataTable, false);
            // Finally, use the newly imported file, to Upsert the database.

            return lakerFileName;
        }
    }
}