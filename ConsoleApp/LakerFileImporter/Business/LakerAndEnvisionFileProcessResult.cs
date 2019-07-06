namespace LakerFileImporter.Business
{
    internal enum LakerAndEnvisionFileProcessResult
    {
        NoLakerFilesFoundInFileDirectory,
        NoEnvisionFilesFoundInFileDirectory,
        MonthYearFolderCouldNotBeCreatedInLocalDirectory,
        NoLakerFileProcessingNecessary,
        NoEnvisionFileProcessingNecessary,
        LakerFileFailedToUpload,
        LakerFileFailedToProcess,
        EnvisionFileFailedToProcess,
        LakerFileProcessStartedSuccessfully,
        EnvisionFileProcessStartedSuccessfully,
        EnvisionFileFailedToUpload
    }
}
