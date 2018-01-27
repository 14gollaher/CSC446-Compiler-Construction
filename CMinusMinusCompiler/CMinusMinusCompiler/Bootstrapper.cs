using System;
using System.IO;
using static CMinusMinusCompiler.LexicalAnalyzer;

namespace CMinusMinusCompiler
{
    // Initializes core modules for C-- Compiler, and allows for unit
    // testing command line arguments. 
    public class Bootstrapper
    {
        // Initialize core C-- Compiler components.
        public void Start(string[] arguments)
        {
            CommonTools.CreateOutputDirectory();
            StartLexicalAnalyzer(arguments);
            CommonTools.ExitProgram();
        }

        // Initializes and runs Lexical Analysis module
        public void StartLexicalAnalyzer(string[] arguments)
        {
            LexicalAnalyzer lexicalAnalyzer;
            if (arguments.Length == 1)
            {
                lexicalAnalyzer = new LexicalAnalyzer(arguments[0]);
            }
            else
            {
                Console.WriteLine("ERROR: Usage expected command line argument.");
                return;
            }

            Console.Clear();

            File.Delete(CommonTools.OutputFilePath);
            LexicalAnaylzerPrinter.DisplayTokenHeader();

            while(Token != Symbol.EndOfFileToken)
            {
                lexicalAnalyzer.GetNextToken();
                LexicalAnaylzerPrinter.DisplayCurrentToken();
            }
        }
    }
}
