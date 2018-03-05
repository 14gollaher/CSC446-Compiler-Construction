using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CMinusMinusCompiler.Test
{
    [TestClass]
    public class SymbolTable_Test
    {
        [TestMethod]
        public void SymbolTable_HappyPath()
        {
            Bootstrapper.StartSymbolTable();

            SymbolTable symbolTable = new SymbolTable();

            symbolTable.InsertNode("cats", Symbol.IdentifierToken, 2);
            symbolTable.InsertNode("cats", Symbol.IfToken, 2);
            symbolTable.InsertNode("dogs", Symbol.IdentifierToken, 2);
            symbolTable.InsertNode("cats", Symbol.IdentifierToken, 3);
            symbolTable.DeleteDepth(3);
            symbolTable.OutputSymbolTable(2);
            symbolTable.OutputSymbolTable(3);

            TestTools.CompareFileEquality
                (@"SymbolTable\Expected\HappyPath.txt", CommonTools.OutputFilePath);
        }
    }
}
