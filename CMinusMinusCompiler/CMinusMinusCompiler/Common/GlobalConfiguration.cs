using System;
using System.Configuration;

namespace CMinusMinusCompiler
{
    public class GlobalConfiguration
    {
        public static int CharacterSize { get; } = Int32.Parse(ConfigurationManager.AppSettings["CharacterSize"]);
        public static int IntegerSize { get; } = Int32.Parse(ConfigurationManager.AppSettings["IntegerSize"]);
        public static int FloatSize { get; } = Int32.Parse(ConfigurationManager.AppSettings["FloatSize"]);
        public static int BaseDepth { get; } = Int32.Parse(ConfigurationManager.AppSettings["BaseDepth"]);
        public static int BaseLocalOffset { get; } = Int32.Parse(ConfigurationManager.AppSettings["BaseLocalOffset"]);
        public static int BaseParameterOffset { get; } = Int32.Parse(ConfigurationManager.AppSettings["BaseParameterOffset"]);
        public static int TableSize { get; } = Int32.Parse(ConfigurationManager.AppSettings["SymbolTableSize"]);
        public static int MaximumIdentifierLength { get; } = Int32.Parse(ConfigurationManager.AppSettings["MaximumIdentifierLength"]);
        public static string LexicalAnalyzerOutputPath { get; } = ConfigurationManager.AppSettings["LexicalAnalyzerOutputPath"];
        public static string SymbolTableOutputPath { get; } = ConfigurationManager.AppSettings["SymbolTableOutputPath"];
        public static string ParserOutputPath { get; } = ConfigurationManager.AppSettings["ParserOutputPath"];
        public static string ThreeAddressCodeOutputPath { get; } = ConfigurationManager.AppSettings["ThreeAddressCodeOutputPath"];
        public static string CodeGeneratorOutputPath { get; } = ConfigurationManager.AppSettings["CodeGeneratorOutputPath"];
    }
}
