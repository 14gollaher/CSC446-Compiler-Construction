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
    }
}
