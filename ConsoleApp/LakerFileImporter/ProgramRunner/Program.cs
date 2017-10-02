using System;
using LakerFileImporter.IO;

namespace LakerFileImporter.ProgramRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var str = new IoHelper().GetMe();
            Console.WriteLine(str);
            Console.ReadLine();
        }
    }
}
