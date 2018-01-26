using System;
using System.Configuration;
using System.IO;

namespace CMinusMinusCompiler
{
    public static class CommonTools
    {
        public static int DisplayLineCount { get; set; }

        public static string OutputFilePath
            = ConfigurationManager.AppSettings["LexicalAnalyzerOutputPath"];

        public static void WriteOutput(string output)
        {
            Console.WriteLine(output);
            File.AppendAllText(OutputFilePath, output + Environment.NewLine);
            DisplayLineCount++;
        }

        public static void ExitProgram()
        {
            if (!IsUnitTestExecution)
            {
                Console.Write("Press any key to exit...");
                Console.ReadKey();
            }
        }

        public static void CreateOutputDirectory()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(OutputFilePath));
        }

        public static void OutputDisplayPause()
        {
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        public static bool IsUnitTestExecution { get; set; }
    }
}
