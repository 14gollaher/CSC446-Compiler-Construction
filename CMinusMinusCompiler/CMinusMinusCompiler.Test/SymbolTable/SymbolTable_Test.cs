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

            symbolTable.InsertNode(new VariableNode { Lexeme = "cats", Depth = 2 });
            symbolTable.InsertNode(new FunctionNode { Lexeme = "cats", Depth = 2 });
            symbolTable.InsertNode(new ConstantNode { Lexeme = "dogs", Depth = 2 });
            symbolTable.InsertNode(new ConstantNode { Lexeme = "horses", Depth = 3 });

            symbolTable.DeleteDepth(3);

            symbolTable.InsertNode(new VariableNode { Lexeme = "cows", Depth = 5 });
            symbolTable.InsertNode(new VariableNode { Lexeme = "cows", Depth = 4 });
            symbolTable.InsertNode(new VariableNode { Lexeme = "cows", Depth = 6 });

            Node nullNode = symbolTable.LookupNode("horses");
            Assert.IsNull(nullNode);

            Node validNode = symbolTable.LookupNode("cats");
            Assert.AreEqual("cats", validNode.Lexeme);
            Assert.AreEqual(2, validNode.Depth);

            validNode = symbolTable.LookupNode("cows");
            Assert.AreEqual("cows", validNode.Lexeme);
            Assert.AreEqual(6, validNode.Depth);

            symbolTable.OutputSymbolTable(2);
            symbolTable.OutputSymbolTable(3);

            TestTools.CompareFileEquality(@"SymbolTable\Expected\HappyPath.txt", CommonTools.OutputFilePaths[0]);
        }
    }
}
