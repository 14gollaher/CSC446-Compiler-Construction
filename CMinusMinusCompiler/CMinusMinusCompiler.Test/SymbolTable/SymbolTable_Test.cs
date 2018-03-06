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

            symbolTable.InsertNode("horses", Symbol.IdentifierToken, 3);
            symbolTable.DeleteDepth(3);

            symbolTable.InsertNode("cows", Symbol.CharToken, 5);
            symbolTable.InsertNode("cows", Symbol.FloatToken, 4);

            Node nullNode = symbolTable.LookupNode("horses");
            Assert.IsNull(nullNode);

            Node validNode = symbolTable.LookupNode("cats");
            Assert.AreEqual("cats", validNode.Lexeme);
            Assert.AreEqual(Symbol.IdentifierToken, validNode.Token);
            Assert.AreEqual(2, validNode.Depth);

            validNode = symbolTable.LookupNode("cows");
            Assert.AreEqual("cows", validNode.Lexeme);
            Assert.AreEqual(Symbol.FloatToken, validNode.Token);
            Assert.AreEqual(4, validNode.Depth);

            symbolTable.OutputSymbolTable(2);
            symbolTable.OutputSymbolTable(3);

            TestTools.CompareFileEquality
                (@"SymbolTable\Expected\HappyPath.txt", CommonTools.OutputFilePath);
        }
    }
}
