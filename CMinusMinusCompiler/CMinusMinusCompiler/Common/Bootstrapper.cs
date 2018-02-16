using System;
using System.Configuration;
using System.IO;

namespace CMinusMinusCompiler
{
    // Initializes core modules for C-- Compiler. Gives ability to
    // to run modules independently, and test command line arguments
    public class Bootstrapper
    {
        // Initialize core C-- Compiler components. Allows for program
        // to run and test modules independently
        public void Start(string[] arguments)
        {
            //StartLexicalAnalyzer(arguments);
            StartParser(arguments);

            CommonTools.PromptProgramExit();
        }

        // Initializes and runs Lexical Analysis module 
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
                Console.WriteLine("ERROR: Usage expects single command line argument.");
                return;
            }

            CommonTools.LexicalAnalyzerInstance = lexicalAnalyzer;
            CommonTools.DisplayHeader = lexicalAnalyzer.DisplayTokenHeader;

            lexicalAnalyzer.DisplayTokenHeader();

            while(lexicalAnalyzer.Token != Symbol.EndOfFileToken)
            {
                lexicalAnalyzer.GetNextToken();
                lexicalAnalyzer.DisplayCurrentToken();
            }
        }

        // Initializes and runs Parser module
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
                CommonTools.WriteOutput(
                    "ERROR: Line " 
                    + lexicalAnalyzer.LineNumber 
                    + " Unexpected tokens in source file, expected End-of-File Token");
            }

            CommonTools.WriteOutput("Completed processing " + Path.GetFileName(arguments[0]));
        }
    }
}
