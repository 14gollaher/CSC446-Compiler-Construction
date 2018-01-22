using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace CMinusMinusCompiler.Test
{
    [TestClass]
    public class LexicalAnalysis_Test
    {
        [TestMethod]
        public void HappyPath()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(null);

            string output = File.ReadAllText(CommonTools.OutputFilePath);
            string expected = File.ReadAllText(@"LexicalAnalysis\LexicalAnalysisTest_HappyPath.txt");

            Assert.AreEqual(expected, output);
        }
    }
}
