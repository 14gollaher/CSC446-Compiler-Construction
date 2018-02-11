using System;
using System.Configuration;
using System.IO;

namespace CMinusMinusCompiler
{
    // Initializes core modules for C-- Compiler. Gives ability to
    // to run modules independently, and test command line arguments
    public class Bootstrapper
    {
        // Initialize core C-- Compiler components.
        public void Start(string[] arguments)
        {

            // StartLexicalAnalyzer(arguments); 
            StartParser(arguments);

            CommonTools.PromptProgramExit();
        }

        // Initializes and runs Lexical Analysis module. Allows for program
        // to run and test modules separately
        public void StartLexicalAnalyzer(string[] arguments)
        {
            CommonTools.CreateOutputDirectory(ConfigurationManager.AppSettings["LexicalAnalyzerOutputPath"]);
            CommonTools.ClearDisplays();

            LexicalAnalyzer lexicalAnalyzer;
            if (arguments.Length == 1)
            {
                if (!CommonTools.CheckFilePathExists(arguments[0])) return;

                lexicalAnalyzer = new LexicalAnalyzer(arguments[0]);
            }
            else
            {
                Console.WriteLine("ERROR: Usage expected command line argument.");
                return;
            }

            CommonTools.LexicalAnalyzerInstance = lexicalAnalyzer;

            lexicalAnalyzer.DisplayTokenHeader();

            while(lexicalAnalyzer.Token != Symbol.EndOfFileToken)
            {
                lexicalAnalyzer.GetNextToken();
                lexicalAnalyzer.DisplayCurrentToken();
            }
        }

        public void StartParser(string[] arguments)
        {
            CommonTools.CreateOutputDirectory(ConfigurationManager.AppSettings["ParserOutputPath"]);
            CommonTools.ClearDisplays();

            LexicalAnalyzer lexicalAnalyzer;
            Parser parser;

            if (arguments.Length == 1)
            {
                if (!CommonTools.CheckFilePathExists(arguments[0])) return;

                lexicalAnalyzer = new LexicalAnalyzer(arguments[0]);
            }
            else
            {
                Console.WriteLine("ERROR: Usage expected command line argument.");
                return;
            }

            CommonTools.LexicalAnalyzerInstance = lexicalAnalyzer;

            lexicalAnalyzer.GetNextToken();

            parser = new Parser(lexicalAnalyzer);
            parser.ProcessProgram();

            if (lexicalAnalyzer.Token != Symbol.EndOfFileToken)
            {
                CommonTools.WriteOutput("ERROR: Unexpected tokens after end-of-file symbol.");
            }

            CommonTools.WriteOutput("Completed processing " + Path.GetFileName(arguments[0]));
        }
    }
}
