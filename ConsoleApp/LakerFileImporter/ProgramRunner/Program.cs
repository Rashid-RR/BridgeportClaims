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
                var results = driver.UploadAndProcessLakerFileIfNecessaryAsync().GetAwaiter().GetResult();
                if (!cs.AppIsInDebugMode)
                {
                    return;
                }
                foreach (var result in results)
                {
                    switch (result)
                    {
                        case LakerAndEnvisionFileProcessResult.NoLakerFilesFoundInFileDirectory:
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
                        case LakerAndEnvisionFileProcessResult.NoEnvisionFilesFoundInFileDirectory:
                            logger.Info("No Envision files where found in the local file directory.");
                            break;
                        case LakerAndEnvisionFileProcessResult.MonthYearFolderCouldNotBeCreatedInLocalDirectory:
                            logger.Info("The month and year folder for either file type could not be created successfully.");
                            break;
                        case LakerAndEnvisionFileProcessResult.EnvisionFileFailedToUpload:
                            logger.Info("Envision file failed to upload.");
                            break;
                        case LakerAndEnvisionFileProcessResult.EnvisionFileFailedToProcess:
                            logger.Info("Envision file failed to process.");
                            break;
                        case LakerAndEnvisionFileProcessResult.EnvisionFileProcessStartedSuccessfully:
                            logger.Info("The Envision file process started successfully.");
                            break;
                        case LakerAndEnvisionFileProcessResult.NoEnvisionFileProcessingNecessary:
                            logger.Info("No Envision file processing is necessary.");
                            break;
                        case LakerAndEnvisionFileProcessResult.LakerFileUploadedSuccessfully:
                            logger.Info("Laker file uploaded successfully.");
                            break;
                        case LakerAndEnvisionFileProcessResult.EnvisionFileUploadedSuccessfully:
                            logger.Info("Envision file uploaded successfully.");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
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
