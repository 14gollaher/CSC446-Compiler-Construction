using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CMinusMinusCompiler.Test
{
    [TestClass]
    public class CodeGenerator_Test
    {
        [TestMethod]
        public void CodeGenerator_HappyPath()
        {
            Bootstrapper.StartCodeGeneratorDebug(new string[] { @"CodeGenerator\Source\HappyPath.c" });
            TestTools.CompareFileEquality(@"CodeGenerator\Expected\HappyPath.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void CodeGenerator_ConsoleInputOutput()
        {
            Bootstrapper.StartCodeGeneratorDebug(new string[] { @"CodeGenerator\Source\ConsoleInputOutput.c" });
            TestTools.CompareFileEquality(@"CodeGenerator\Expected\ConsoleInputOutput.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void CodeGenerator_Operations()
        {
            Bootstrapper.StartCodeGeneratorDebug(new string[] { @"CodeGenerator\Source\Operations.c" });
            TestTools.CompareFileEquality(@"CodeGenerator\Expected\Operations.txt", CommonTools.OutputFilePaths[0]);
        }
    }
}
