namespace LakerFileImporter.Business
{
    internal enum LakerAndEnvisionFileProcessResult
    {
        NoLakerFilesFoundInFileDirectory,
        NoEnvisionFilesFoundInFileDirectory,
        MonthYearFolderCouldNotBeCreatedInLocalDirectory,
        NoLakerOrEnvisionFileProcessingNecessary,
        LakerFileFailedToUpload,
        LakerFileFailedToProcess,
        EnvisionFileFailedToProcess,
        LakerFileProcessStartedSuccessfully,
        EnvisionFileProcessStartedSuccessfully,
        EnvisionFileFailedToUpload
    }

    internal enum ImportFileType
    {
        LakerImport = 1,
        PaymentImport = 2,
        Other = 3,
        EnvisionImport = 4
    }
}
