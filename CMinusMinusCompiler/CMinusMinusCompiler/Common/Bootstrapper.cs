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
            //StartThreeAddressCode(arguments);
            StartCodeGenerator(arguments);

        }

        // Initializes and runs Lexical Analysis module 
        public static void StartLexicalAnalyzer(string[] arguments)
        {
            ValidateArguments(arguments);
            CommonTools.SetuputOutputResources(new string[] { ConfigurationManager.AppSettings["LexicalAnalyzerOutputPath"] });
            CommonTools.ClearDisplays();

            LexicalAnalyzer lexicalAnalyzer = new LexicalAnalyzer(arguments[0]);
            CommonTools.DisplayHeader = lexicalAnalyzer.DisplayTokenHeader;
            lexicalAnalyzer.DisplayTokenHeader();

            while(lexicalAnalyzer.Token != Token.EndOfFileToken)
            {
                lexicalAnalyzer.GetNextToken();
                lexicalAnalyzer.DisplayCurrentToken();
            }
            CommonTools.PromptProgramExit();
        }

        // Initializes and runs Parser module
        public static void StartParser(string[] arguments)
        {
            ValidateArguments(arguments);
            CommonTools.SetuputOutputResources(new string[] { ConfigurationManager.AppSettings["ParserOutputPath"] });
            CommonTools.ClearDisplays();

            LexicalAnalyzer lexicalAnalyzer = new LexicalAnalyzer(arguments[0]);
            lexicalAnalyzer.GetNextToken();

            SymbolTable symbolTable = new SymbolTable();
            Parser parser = new Parser(lexicalAnalyzer, symbolTable);

            if (parser.Start()) symbolTable.OutputSymbolTable(1);

            CommonTools.WriteOutput($"Completed processing {Path.GetFileName(arguments[0])}");
            CommonTools.PromptProgramExit();
        }

        // Initializes and runs Symbol Table module
        public static void StartSymbolTable()
        {
            CommonTools.SetuputOutputResources(new string[] {ConfigurationManager.AppSettings["SymbolTableOutputPath"]});
            CommonTools.ClearDisplays();
            CommonTools.PromptProgramExit();
        }

        // Initializes and runs Three Address Code module
        public static void StartThreeAddressCode(string[] arguments)
        {
            ValidateArguments(arguments);
            CommonTools.SetuputOutputResources
                (new string[] { ConfigurationManager.AppSettings["ThreeAddressCodeOutputPath"],
                                AppendFileExtension(arguments[0], "tac") });

            CommonTools.ClearDisplays();
            CommonTools.ThreeAddressCodeRun = true;

            LexicalAnalyzer lexicalAnalyzer = new LexicalAnalyzer(arguments[0]);
            lexicalAnalyzer.GetNextToken();

            SymbolTable symbolTable = new SymbolTable();
            Parser parser = new Parser(lexicalAnalyzer, symbolTable);
            parser.Start();

            CommonTools.PromptProgramExit();
        }

        // Initializes and runs Code Generator module
        public static void StartCodeGenerator(string[] arguments)
        {
            ValidateArguments(arguments);
            CommonTools.SetuputOutputResources(new string[] { AppendFileExtension(arguments[0], "tac") });

            CommonTools.ClearDisplays();
            CommonTools.ThreeAddressCodeRun = true;

            LexicalAnalyzer lexicalAnalyzer = new LexicalAnalyzer(arguments[0]);
            lexicalAnalyzer.GetNextToken();

            SymbolTable symbolTable = new SymbolTable();
            Parser parser = new Parser(lexicalAnalyzer, symbolTable);

            if (parser.Start())
            {
                CommonTools.SetuputOutputResources
                    (new string[] { ConfigurationManager.AppSettings["CodeGeneratorOutputPath"],
                                    AppendFileExtension(arguments[0], "asm") });
                CodeGenerator codeGenerator = new CodeGenerator(AppendFileExtension(arguments[0], "asm"), symbolTable);
                codeGenerator.Start();
            }

            CommonTools.PromptProgramExit();
        }

        // Initializes and runs Semantic Analysis module with debug flag set
        public static void StartSemanticAnaylsisDebug(string[] arguments)
        {
            CommonTools.SemanticAnalysisDebug = true;
            StartParser(arguments);
            CommonTools.SemanticAnalysisDebug = false;
        }

        // Initializes and runs Parser module with debug flag set
        public static void StartParserDebug(string[] arguments)
        {
            CommonTools.ParserDebug = true;
            StartParser(arguments);
            CommonTools.ParserDebug = false;
        }

        // Initializes and runs Three Address Code module with debug flag set
        public static void StartThreeAddressCodeDebug(string[] arguments)
        {
            StartThreeAddressCode(arguments);
            CommonTools.ThreeAddressCodeRun = false;
        }

        // Initializes and runs Code Generator module with debug flag set
        public static void StartCodeGeneratorDebug(string[] arguments)
        {
            StartCodeGenerator(arguments);
            CommonTools.CodeGeneratorRun = false;
        }

        private static bool ValidateArguments(string[] arguments)
        {
            if (arguments.Length == 1)
            {
                return CommonTools.CheckFilePathExists(arguments[0]);
            }
            else
            {
                Console.WriteLine("ERROR: Usage expected 1 command line argument");
                return false;
            }
        }

        private static string AppendFileExtension(string path, string extension)
        {
            return $"{Path.GetFileNameWithoutExtension(path)}.{extension}";
        }
    }
}
