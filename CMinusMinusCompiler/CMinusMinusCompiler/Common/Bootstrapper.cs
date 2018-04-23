using System;
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
            CommonTools.SetupOutputs(new string[] { GlobalConfiguration.LexicalAnalyzerOutputPath });
            CommonTools.ClearOutputDisplays();

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
            CommonTools.SetupOutputs(new string[] { GlobalConfiguration.LexicalAnalyzerOutputPath });
            CommonTools.ClearOutputDisplays();

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
            CommonTools.SetupOutputs(new string[] { GlobalConfiguration.SymbolTableOutputPath });
            CommonTools.ClearOutputDisplays();
            CommonTools.PromptProgramExit();
        }

        // Initializes and runs Three Address Code module
        public static void StartThreeAddressCode(string[] arguments)
        {
            CommonTools.ThreeAddressCodeRun = true;
            ValidateArguments(arguments);
            CommonTools.SetupOutputs
                (new string[] { GlobalConfiguration.ThreeAddressCodeOutputPath,
                                AppendFileExtension(arguments[0], "tac") });

            CommonTools.ClearOutputDisplays();

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
            CommonTools.ThreeAddressCodeRun = true;
            ValidateArguments(arguments);
            CommonTools.SetupOutputs(new string[] { GlobalConfiguration.ThreeAddressCodeOutputPath,
                                                    AppendFileExtension(arguments[0], "tac") });
            CommonTools.ClearOutputDisplays();

            LexicalAnalyzer lexicalAnalyzer = new LexicalAnalyzer(arguments[0]);
            lexicalAnalyzer.GetNextToken();

            SymbolTable symbolTable = new SymbolTable();
            Parser parser = new Parser(lexicalAnalyzer, symbolTable);

            if (parser.Start())
            {
                CommonTools.SetupOutputs
                    (new string[] { GlobalConfiguration.CodeGeneratorOutputPath,
                                    AppendFileExtension(arguments[0], "asm") });
                CommonTools.ClearOutputDisplays();
                CodeGenerator codeGenerator = new CodeGenerator(AppendFileExtension(arguments[0], "tac"), symbolTable);
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
            CommonTools.ThreeAddressCodeRun = false;
        }

        // Validates that a single comman line argument exists and its path is valid
        private static bool ValidateArguments(string[] arguments)
        {
            if (arguments.Length == 1) return CommonTools.CheckFilePathExists(arguments[0]);

            CommonTools.WriteOutput("ERROR: Usage expected 1 command line argument");
            return false;
        }

        // Gets file name from a given path and appends given extension to paht 
        private static string AppendFileExtension(string path, string extension)
        {
            return $"{Path.GetFileNameWithoutExtension(path)}.{extension}";
        }
    }
}
