using System;
using System.Configuration;
using System.IO;

namespace CMinusMinusCompiler
{
    // Initializes core modules for C-- Compiler. Gives ability to
    // to run modules independently, and test command line arguments
    public static class Bootstrapper
    {
        // Initialize core C-- Compiler components. Allows for program
        // to run and test modules independently
        public static void Start(string[] arguments)
        {
            //StartLexicalAnalyzer(arguments);
            //StartParser(arguments);
            //StartSymbolTable();
            StartSemanticAnaylsis(arguments);

            CommonTools.PromptProgramExit();
        }

        // Initializes and runs Lexical Analysis module 
        public static void StartLexicalAnalyzer(string[] arguments)
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

            CommonTools.DisplayHeader = lexicalAnalyzer.DisplayTokenHeader;

            lexicalAnalyzer.DisplayTokenHeader();

            while(lexicalAnalyzer.Token != Token.EndOfFileToken)
            {
                lexicalAnalyzer.GetNextToken();
                lexicalAnalyzer.DisplayCurrentToken();
            }
        }

        // Initializes and runs Parser module
        public static void StartParser(string[] arguments)
        {
            CommonTools.CreateOutputDirectory(ConfigurationManager.AppSettings["ParserOutputPath"]);
            CommonTools.ClearDisplays();

            LexicalAnalyzer lexicalAnalyzer;

            if (arguments.Length == 1)
            {
                if (!CommonTools.CheckFilePathExists(arguments[0])) return;

                lexicalAnalyzer = new LexicalAnalyzer(arguments[0]);
            }
            else
            {
                Console.WriteLine("ERROR: Usage expected command line argument");
                return;
            }

            lexicalAnalyzer.GetNextToken();

            SymbolTable symbolTable = new SymbolTable();
            Parser parser = new Parser(lexicalAnalyzer, symbolTable);
            parser.ProcessProgram();

            if (lexicalAnalyzer.Token != Token.EndOfFileToken)
            {
                CommonTools.WriteOutput(
                    $"ERROR: Line {lexicalAnalyzer.LineNumber} " +
                    $"Unexpected tokens in source file, expected End-of-File Token");
            }

            symbolTable.OutputSymbolTable(1);

            CommonTools.WriteOutput($"Completed processing {Path.GetFileName(arguments[0])}");
        }

        // Initializes and runs Symbol Table module
        public static void StartSymbolTable()
        {
            CommonTools.CreateOutputDirectory(ConfigurationManager.AppSettings["SymbolTableOutputPath"]);
            CommonTools.ClearDisplays();
        }

        // Initializes and runs Semantic Analysis module
        public static void StartSemanticAnaylsis(string[] arguments)
        {
            CommonTools.CreateOutputDirectory(ConfigurationManager.AppSettings["SemanticAnalysisOutputPath"]);
            CommonTools.ClearDisplays();

            LexicalAnalyzer lexicalAnalyzer;

            if (arguments.Length == 1)
            {
                if (!CommonTools.CheckFilePathExists(arguments[0])) return;

                lexicalAnalyzer = new LexicalAnalyzer(arguments[0]);
            }
            else
            {
                Console.WriteLine("ERROR: Usage expected command line argument");
                return;
            }

            lexicalAnalyzer.GetNextToken();

            SymbolTable symbolTable = new SymbolTable();
            Parser parser = new Parser(lexicalAnalyzer, symbolTable);
            parser.ProcessProgram();

            if (lexicalAnalyzer.Token != Token.EndOfFileToken)
            {
                CommonTools.WriteOutput(
                    $"ERROR: Line {lexicalAnalyzer.LineNumber} " +
                    $"Unexpected tokens in source file, expected End-of-File Token");
            }

            symbolTable.OutputSymbolTable(1);

            CommonTools.WriteOutput($"Completed processing {Path.GetFileName(arguments[0])}");
        }

        // Initializes and runs Semantic Analysis module
        public static void StartSemanticAnaylsisDebug(string[] arguments)
        {
            CommonTools.SemanticAnalysisDebug = true;
            StartSemanticAnaylsis(arguments);
            CommonTools.SemanticAnalysisDebug = false;
        }

        public static void StartParserDebug(string[] arguments)
        {
            CommonTools.ParserDebug = true;
            StartParser(arguments);
            CommonTools.ParserDebug = false;
        }
    }
}
