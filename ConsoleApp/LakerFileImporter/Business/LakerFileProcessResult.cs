namespace LakerFileImporter.Business
{
    internal enum LakerFileProcessResult
    {
        NoFilesFoundInFileDirectory,
        NoLakerFileProcessingNecessary,
        LakerFileFailedToUpload,
        LakerFileFailedToProcess,
        LakerFileProcessedSuccessfully
    }
}
