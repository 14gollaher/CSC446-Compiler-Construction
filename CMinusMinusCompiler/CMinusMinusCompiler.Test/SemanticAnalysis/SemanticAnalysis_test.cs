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
        public void SemanticAnalysis_HappyPath2()
        {
            Bootstrapper.StartSemanticAnaylsisDebug(new string[] { @"SemanticAnalysis\Source\HappyPath2.c" });
            TestTools.CompareFileEquality(@"SemanticAnalysis\Expected\HappyPath2.txt", CommonTools.OutputFilePath);
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

        [TestMethod]
        public void SemanticAnalysis_UndeclaredVariableLeft()
        {
            Bootstrapper.StartSemanticAnaylsisDebug(new string[] { @"SemanticAnalysis\Source\UndeclaredVariableLeft.c" });
            TestTools.CompareFileEquality(@"SemanticAnalysis\Expected\UndeclaredVariableLeft.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void SemanticAnalysis_UndeclaredVariableRight()
        {
            Bootstrapper.StartSemanticAnaylsisDebug(new string[] { @"SemanticAnalysis\Source\UndeclaredVariableRight.c" });
            TestTools.CompareFileEquality(@"SemanticAnalysis\Expected\UndeclaredVariableRight.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void SemanticAnalysis_InvalidDeclaration()
        {
            Bootstrapper.StartSemanticAnaylsisDebug(new string[] { @"SemanticAnalysis\Source\InvalidDeclaration.c" });
            TestTools.CompareFileEquality(@"SemanticAnalysis\Expected\InvalidDeclaration.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void SemanticAnalysis_InstructorCase1()
        {
            Bootstrapper.StartSemanticAnaylsisDebug(new string[] { @"SemanticAnalysis\Source\t51.c" });
            TestTools.CompareFileEquality(@"SemanticAnalysis\Expected\t51.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void SemanticAnalysis_InstructorCase2()
        {
            Bootstrapper.StartSemanticAnaylsisDebug(new string[] { @"SemanticAnalysis\Source\t52.c" });
            TestTools.CompareFileEquality(@"SemanticAnalysis\Expected\t52.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void SemanticAnalysis_InstructorCase3()
        {
            Bootstrapper.StartSemanticAnaylsisDebug(new string[] { @"SemanticAnalysis\Source\t53.c" });
            TestTools.CompareFileEquality(@"SemanticAnalysis\Expected\t53.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void SemanticAnalysis_InstructorCase4()
        {
            Bootstrapper.StartSemanticAnaylsisDebug(new string[] { @"SemanticAnalysis\Source\t54.c" });
            TestTools.CompareFileEquality(@"SemanticAnalysis\Expected\t54.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void SemanticAnalysis_InstructorCase5()
        {
            Bootstrapper.StartSemanticAnaylsisDebug(new string[] { @"SemanticAnalysis\Source\t55.c" });
            TestTools.CompareFileEquality(@"SemanticAnalysis\Expected\t55.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void SemanticAnalysis_InstructorCase6()
        {
            Bootstrapper.StartSemanticAnaylsisDebug(new string[] { @"SemanticAnalysis\Source\t56.c" });
            TestTools.CompareFileEquality(@"SemanticAnalysis\Expected\t56.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void SemanticAnalysis_InstructorCase7()
        {
            Bootstrapper.StartSemanticAnaylsisDebug(new string[] { @"SemanticAnalysis\Source\t57.c" });
            TestTools.CompareFileEquality(@"SemanticAnalysis\Expected\t57.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void SemanticAnalysis_InstructorCase8()
        {
            Bootstrapper.StartSemanticAnaylsisDebug(new string[] { @"SemanticAnalysis\Source\t58.c" });
            TestTools.CompareFileEquality(@"SemanticAnalysis\Expected\t58.txt", CommonTools.OutputFilePath);
        }
    }
}
