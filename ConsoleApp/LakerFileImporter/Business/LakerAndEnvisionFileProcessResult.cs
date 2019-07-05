namespace LakerFileImporter.Business
{
    internal enum LakerAndEnvisionFileProcessResult
    {
        NoFilesFoundInFileDirectory,
        MonthYearFolderCouldNotBeCreatedInLocalDirectory,
        NoLakerFileProcessingNecessary,
        LakerFileFailedToUpload,
        LakerFileFailedToProcess,
        LakerFileProcessStartedSuccessfully
    }

    internal enum ImportFileType
    {
        LakerImport = 1,
        PaymentImport = 2,
        Other = 3,
        EnvisionImport = 4
    }
}
