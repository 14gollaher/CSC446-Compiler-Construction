using System.IO;

namespace CMinusMinusCompiler
{
    public class CodeGenerator
    {
        // Private properties
        SymbolTable lexicalAnalyzer = new SymbolTable();
        private string SourceFileContents { get; set; }

        public CodeGenerator(string intermediateCodeFilePath, SymbolTable symbolTable)
        {
            SourceFileContents = File.ReadAllText(intermediateCodeFilePath).Replace("\r", string.Empty);
        }

        public void Start()
        {
            int x = 5;
        }

        private void OutputThreeAddressCode(string output)
        {
            CommonTools.WriteOutput(output);
        }
    }
}