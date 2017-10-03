using System;
using System.Threading.Tasks;
using LakerFileImporter.Business;
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
                var driver = new Driver();
                driver.RunApplicationAsync();
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
