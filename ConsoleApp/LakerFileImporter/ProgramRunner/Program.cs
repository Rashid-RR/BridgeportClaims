using System;
using System.Reflection;
using cs = LakerFileImporter.ConfigService.ConfigService;
using System.Threading.Tasks;
using LakerFileImporter.Business;
using LakerFileImporter.Logging;

namespace LakerFileImporter.ProgramRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var loggingService = LoggingService.Instance;
            var logger = loggingService.Logger;
            try
            {
                if (cs.AppIsInDebugMode)
                {
                    var now = DateTime.Now.ToString("G");
                    var methodName = MethodBase.GetCurrentMethod().Name;
                    logger.Info($"Entered the {methodName} method on {now}");
                }
                var driver = new LakerFileProcessor();
                var result = driver.UploadAndProcessLakerFileIfNecessary().GetAwaiter().GetResult();
                if (!cs.AppIsInDebugMode) return;
                switch (result)
                {
                    case LakerAndEnvisionFileProcessResult.NoFilesFoundInFileDirectory:
                        logger.Info("No Laker files where found in the local file directory.");
                        break;
                    case LakerAndEnvisionFileProcessResult.NoLakerFileProcessingNecessary:
                        logger.Info("No Laker file processing is necessary.");
                        break;
                    case LakerAndEnvisionFileProcessResult.LakerFileFailedToUpload:
                        logger.Info("Laker file failed to upload.");
                        break;
                    case LakerAndEnvisionFileProcessResult.LakerFileFailedToProcess:
                        logger.Info("Laker file failed to process.");
                        break;
                    case LakerAndEnvisionFileProcessResult.LakerFileProcessStartedSuccessfully:
                        logger.Info("The Laker file process started successfully.");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (TaskCanceledException ex)
            {
                logger.Error(ex);
                throw;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }
    }
}
