using System;
using System.Threading.Tasks;
using LakerFileImporter.Business;
using LakerFileImporter.Security;
using NLog;

namespace LakerFileImporter.ProgramRunner
{
    public class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {

            try
            {
                var provider = new SensitiveStringsProvider();
                var driver = new LakerFileProcessor();
                var result = driver.UploadAndProcessLakerFileIfNecessary().GetAwaiter().GetResult();
                switch (result)
                {
                    case LakerFileProcessResult.NoFilesFoundInFileDirectory:
                        Logger.Info("No Laker files where found in the local file directory.");
                        break;
                    case LakerFileProcessResult.NoLakerFileProcessingNecessary:
                        Logger.Info("No Laker file processing is necessary.");
                        break;
                    case LakerFileProcessResult.LakerFileFailedToUpload:
                        Logger.Info("Laker file failed to upload.");
                        break;
                    case LakerFileProcessResult.LakerFileFailedToProcess:
                        Logger.Info("Laker file failed to process.");
                        break;
                    case LakerFileProcessResult.LakerFileProcessedSuccessfully:
                        Logger.Info("The Laker file processed successfully.");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                Console.ReadLine();
            }
            catch (TaskCanceledException ex)
            {
                Logger.Error(ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
