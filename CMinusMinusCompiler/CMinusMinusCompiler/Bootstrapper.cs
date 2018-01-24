using System;
using System.IO;

//TODO: File opening issues?
//TODO: Initial creation of output files
//TODO: Figure out where Hamer will drop the input files
//TODO: Nexted comments?
//TODO: Change startup to say "expected usage: " 


namespace CMinusMinusCompiler
{
    // Initializes core modules for C-- Compiler, and allows for unit
    // testing command line arguments. 
    public class Bootstrapper
    {
        // Initialize core C-- Compiler components.
        public void Start(string[] arguments)
        {
            CreateOutputDirectory();
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
                Console.Write("Enter a file path: ");
                string sourceFilePath = Console.ReadLine();
                lexicalAnalyzer = new LexicalAnalyzer(sourceFilePath);
            }

            lexicalAnalyzer.DisplayTokenHeader();

            while(lexicalAnalyzer.Token != LexicalAnalyzer.Symbol.EndOfFileToken)
            {
                lexicalAnalyzer.GetNextToken();
                lexicalAnalyzer.DisplayCurrentToken();
            }
        }

        private void CreateOutputDirectory()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(CommonTools.OutputFilePath));
        }
    }
}
