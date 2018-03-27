using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CMinusMinusCompiler.Test
{
    [TestClass]
    public class SemanticAnalysis_Test
    {
        [TestMethod]
        public void SemanticAnalysis_HappyPath()
        {

            Bootstrapper.StartSemanticAnaylsis(new string[] { @"SemanticAnalysis\Source\HappyPath.c" });
        }

        [TestMethod]
        public void SemanticAnalysis_Constants()
        {
            Bootstrapper.StartSemanticAnaylsis(new string[] { @"SemanticAnalysis\Source\Constants.c" });
            TestTools.CompareFileEquality(@"SemanticAnalysis\Expected\Constants.txt", CommonTools.OutputFilePath);
        }
    }
}
