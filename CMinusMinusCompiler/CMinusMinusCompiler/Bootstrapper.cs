using System;
using System.IO;

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
            CommonTools.PromptProgramExit();
        }

        // Initializes and runs Lexical Analysis module
        public void StartLexicalAnalyzer(string[] arguments)
        {
            LexicalAnalyzer lexicalAnalyzer;
            if (arguments.Length == 1)
            {
                lexicalAnalyzer = new LexicalAnalyzer(arguments[0]);
                CommonTools.LexicalAnalyzerInstance = lexicalAnalyzer;
            }
            else
            {
                Console.WriteLine("ERROR: Usage expected command line argument.");
                return;
            }

            Console.Clear();

            File.Delete(CommonTools.OutputFilePath);
            lexicalAnalyzer.DisplayTokenHeader();

            while(lexicalAnalyzer.Token != LexicalAnalyzer.TokenType.EndOfFileToken)
            {
                lexicalAnalyzer.GetNextToken();
                lexicalAnalyzer.DisplayCurrentToken();
            }
        }
    }
}
