namespace LakerFileImporter.Business
{
    internal enum LakerFileProcessResult
    {
        NoFilesFoundInFileDirectory,
        MonthYearFolderCouldNotBeCreatedInLocalDirectory,
        NoLakerFileProcessingNecessary,
        LakerFileFailedToUpload,
        LakerFileFailedToProcess,
        LakerFileProcessStartedSuccessfully
    }
}
