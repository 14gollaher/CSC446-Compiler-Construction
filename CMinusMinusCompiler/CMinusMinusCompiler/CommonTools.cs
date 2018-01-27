using System;
using System.Configuration;
using System.IO;

namespace CMinusMinusCompiler
{
    // Contains reusable components and tools
    public static class CommonTools
    {
        // Public members
        public static int DisplayLineCount { get; set; }
        public static bool IsUnitTestExecution { get; set; }
        public static string OutputFilePath { get; set; }
            = ConfigurationManager.AppSettings["LexicalAnalyzerOutputPath"];
        public static LexicalAnalyzer LexicalAnalyzerInstance { get; set;}

        // Writes the output to the screen and output file
        public static void WriteOutput(string output)
        {
            UpdateOutputPager();
            Console.WriteLine(output);
            File.AppendAllText(OutputFilePath, output + Environment.NewLine);
            DisplayLineCount++;
        }
        
        // Checks that an input 
        public static bool CheckFilePathExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("ERROR: Could not open specified source file.");
                PromptProgramExit();
                return false;
            }
            return true;
        }

        // Prompt user to for final key before program termination
        public static void PromptProgramExit()
        {
            if (!IsUnitTestExecution)
            {
                Console.Write("Press any key to exit...");
                if (!IsUnitTestExecution) Console.ReadKey();
            }
        }

        // Create an output directory for the 
        public static void CreateOutputDirectory()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(OutputFilePath));
        }

        // Pause output prompting user for key to continue
        public static void OutputDisplayPause()
        {
            Console.Write("Press any key to see more output...");
            if (!IsUnitTestExecution)
            {
                Console.ReadKey();
                Console.Clear();
            }
        }

        // Manage the output to ensure only 20 results on the page at a time
        private static void UpdateOutputPager()
        {
            if (DisplayLineCount == 20 && !IsUnitTestExecution)
            {
                DisplayLineCount = 0;
                OutputDisplayPause();
                LexicalAnalyzerInstance.DisplayTokenHeader(); // Will be commented in later versions
            }
        }
    }
}
