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
            File.AppendAllText(OutputFilePath, output + Environment.NewLine);
        }

        public static void ExitProgram()
        {
            if (!IsUnitTestExecution)
            {
                Console.Write("Press any key to exit...");
                Console.ReadKey();
            }
        }

        public static bool IsUnitTestExecution { get; set; }
    }
}
