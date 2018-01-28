using System;
using System.Configuration;

namespace CMinusMinusCompiler
{
    // Initializes core modules for C-- Compiler. Gives ability to
    // to run modules independently, and test command line arguments
    public class Bootstrapper
    {
        // Initialize core C-- Compiler components.
        public void Start(string[] arguments)
        {
            CommonTools.CreateOutputDirectory(ConfigurationManager.AppSettings["LexicalAnalyzerOutputPath"]);
            
            // Change below line as needed 
            StartLexicalAnalyzer(arguments); 

            CommonTools.PromptProgramExit();
        }

        // Initializes and runs Lexical Analysis module. Allows for program
        // to run and test modules separately
        public void StartLexicalAnalyzer(string[] arguments)
        {
            LexicalAnalyzer lexicalAnalyzer;
            if (arguments.Length == 1)
            {
                if (!CommonTools.CheckFilePathExists(arguments[0])) return;

                lexicalAnalyzer = new LexicalAnalyzer(arguments[0]);
                CommonTools.LexicalAnalyzerInstance = lexicalAnalyzer;
            }
            else
            {
                Console.WriteLine("ERROR: Usage expected command line argument.");
                return;
            }

            CommonTools.ClearDisplays();
            lexicalAnalyzer.DisplayTokenHeader();

            while(lexicalAnalyzer.Token != LexicalAnalyzer.TokenType.EndOfFileToken)
            {
                lexicalAnalyzer.GetNextToken();
                lexicalAnalyzer.DisplayCurrentToken();
            }
        }
    }
}
