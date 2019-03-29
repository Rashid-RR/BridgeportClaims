using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace AzureDatabaseBackupExportImport
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var scriptFile = @"C:\Development\PowerShell\ImportSqlServerBacpac.ps1";
                var runSpaceConfiguration = RunspaceConfiguration.Create();
                var runSpace = RunspaceFactory.CreateRunspace(runSpaceConfiguration);
                runSpace.Open();
                using (var scriptInvoker = new RunspaceInvoke(runSpace))
                {
                    scriptInvoker.Invoke("Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass");
                    var pipeline = runSpace.CreatePipeline();
                    //Here's how you add a new script with arguments
                    var myCommand = new Command(scriptFile);
                    /*var testParam = new CommandParameter("key", "value");
                    myCommand.Parameters.Add(testParam);*/
                    pipeline.Commands.Add(myCommand);
                    // Execute PowerShell script
                    var results = pipeline.Invoke();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }
    }
}