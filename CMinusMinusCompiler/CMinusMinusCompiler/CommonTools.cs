using System;
using System.Configuration;
using System.IO;

namespace CMinusMinusCompiler
{
    public static class CommonTools
    {
        public static string OutputFilePath = ConfigurationManager.AppSettings["OutputFilePath"];

        public static void WriteOutput(string output)
        {
            Console.WriteLine(output);
            File.WriteAllText(OutputFilePath, output);
        }
    }
}
