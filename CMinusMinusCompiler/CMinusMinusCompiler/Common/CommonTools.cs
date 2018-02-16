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
        public static LexicalAnalyzer LexicalAnalyzerInstance { get; set; }
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
                Console.Write("Press any key to exit...");
                if (!IsUnitTestExecution) Console.ReadKey();
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

    // Enumerated type to contain all possible token types
    public enum Symbol {
        IfToken, ElseToken, WhileToken, FloatToken, IntToken, CharToken,
        BreakToken, ContinueToken, VoidToken, CommaToken, SemiColonToken,
        AssignmentOperatorToken, EndOfFileToken, AdditionOperatorToken,
        MultiplicationOperatorToken, LeftParenthesisToken, RightParenthesisToken,
        LeftBraceToken, RightBraceToken, LeftBracketToken, RightBracketToken,
        PeriodToken, QuotationsSymbol, RelationalOperatorToken, IdentifierToken,
        NumberToken, StringLiteralToken, UnderscoreToken, UnknownToken
    }
}
