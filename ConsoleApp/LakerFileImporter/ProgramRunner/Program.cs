using System;
using LakerFileImporter.DAL.ImportFileProvider;
using LakerFileImporter.IO;

namespace LakerFileImporter.ProgramRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var importFileProvider = new ImportFileProvider();
            var files = importFileProvider.GetImportFileDtos();

            Console.ReadLine();
        }
    }
}
