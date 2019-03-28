using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace AzureDatabaseBackupExportImport
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var scriptFile = @"C:\Development\PowerShell\ExportSqlServerBacpac.ps1";
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
    }
}
