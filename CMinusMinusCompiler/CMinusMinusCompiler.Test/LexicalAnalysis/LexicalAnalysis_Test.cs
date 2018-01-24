using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace CMinusMinusCompiler.Test
{
    [TestClass]
    public class LexicalAnalysis_Test
    {
        [TestMethod]
        public void LexicalAnalysis_HappyPath()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalysis\Source\HappyPath.c" });

            string output = File.ReadAllText(CommonTools.OutputFilePath);
            string expected = File.ReadAllText(@"LexicalAnalysis\Expected\HappyPath.txt");
            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void LexicalAnalysis_Identifiers()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalysis\Source\Identifiers.c" });

            string output = File.ReadAllText(CommonTools.OutputFilePath);
            string expected = File.ReadAllText(@"LexicalAnalysis\Expected\Identifiers.txt");
            Assert.AreEqual(expected, output);
        }
    }
}
