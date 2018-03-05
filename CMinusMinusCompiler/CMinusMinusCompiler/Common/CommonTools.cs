using System;
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
        public static Action DisplayHeader { get; set; }

        // Writes the output to the screen and output file
        public static void WriteOutput(string output)
        {
            UpdateOutputPager(DisplayHeader);
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
                Console.Write("\nPress any key to exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        // Create an output directory for specified path
        public static void CreateOutputDirectory(string outputFilePath)
        {
            OutputFilePath = outputFilePath;
            Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));
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

        // Clear the file and screen displays
        public static void ClearDisplays()
        {
            Console.Clear();
            File.Delete(OutputFilePath);
        }

        // Manage the output to ensure only 20 results on the page at a time
        // and accept function to display header information
        private static void UpdateOutputPager(Action displayHeader)
        {
            if (DisplayLineCount == 20 && !IsUnitTestExecution)
            {
                DisplayLineCount = 0;
                OutputDisplayPause();

                displayHeader?.Invoke();
            }
        }
    }
}
