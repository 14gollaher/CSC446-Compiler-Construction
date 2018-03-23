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

            symbolTable.InsertNode("cats", Token.IdentifierToken, 2);
            symbolTable.InsertNode("cats", Token.IfToken, 2);
            symbolTable.InsertNode("dogs", Token.IdentifierToken, 2);

            symbolTable.InsertNode("horses", Token.IdentifierToken, 3);
            symbolTable.DeleteDepth(3);

            symbolTable.InsertNode("cows", Token.CharToken, 5);
            symbolTable.InsertNode("cows", Token.FloatToken, 4);
            symbolTable.InsertNode("cows", Token.FloatToken, 6);

            Node nullNode = symbolTable.LookupNode("horses");
            Assert.IsNull(nullNode);

            Node validNode = symbolTable.LookupNode("cats");
            Assert.AreEqual("cats", validNode.Lexeme);
            Assert.AreEqual(Token.IdentifierToken, validNode.Token);
            Assert.AreEqual(2, validNode.Depth);

            validNode = symbolTable.LookupNode("cows");
            Assert.AreEqual("cows", validNode.Lexeme);
            Assert.AreEqual(Token.FloatToken, validNode.Token);
            Assert.AreEqual(6, validNode.Depth);

            symbolTable.OutputSymbolTable(2);
            symbolTable.OutputSymbolTable(3);

            TestTools.CompareFileEquality
                (@"SymbolTable\Expected\HappyPath.txt", CommonTools.OutputFilePath);
        }
    }
}
