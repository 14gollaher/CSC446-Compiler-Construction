using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CMinusMinusCompiler.Test
{
    [TestClass]
    public class SemanticAnalysis_Test
    {
        [TestMethod]
        public void SemanticAnalysis_HappyPath()
        {
            Bootstrapper.StartSemanticAnaylsisDebug(new string[] { @"SemanticAnalysis\Source\HappyPath.c" });
            TestTools.CompareFileEquality(@"SemanticAnalysis\Expected\HappyPath.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void SemanticAnalysis_Variables()
        {
            Bootstrapper.StartSemanticAnaylsisDebug(new string[] { @"SemanticAnalysis\Source\Variables.c" });
            TestTools.CompareFileEquality(@"SemanticAnalysis\Expected\Variables.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void SemanticAnalysis_Constants()
        {
            Bootstrapper.StartSemanticAnaylsisDebug(new string[] { @"SemanticAnalysis\Source\Constants.c" });
            TestTools.CompareFileEquality(@"SemanticAnalysis\Expected\Constants.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void SemanticAnalysis_Functions()
        {
            Bootstrapper.StartSemanticAnaylsisDebug(new string[] { @"SemanticAnalysis\Source\Functions.c" });
            TestTools.CompareFileEquality(@"SemanticAnalysis\Expected\Functions.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void SemanticAnalysis_Duplicates()
        {
            Bootstrapper.StartSemanticAnaylsisDebug(new string[] { @"SemanticAnalysis\Source\Duplicates.c" });
            TestTools.CompareFileEquality(@"SemanticAnalysis\Expected\Duplicates.txt", CommonTools.OutputFilePath);
        }
    }
}
