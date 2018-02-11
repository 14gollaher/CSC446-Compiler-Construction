using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CMinusMinusCompiler.Test
{
    [TestClass]
    public class Parser_Test
    {
        [TestMethod]
        public void Parser_HappyPath()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.StartParser(new string[] { @"Parser\Source\HappyPath.c" });

            TestTools.CompareFileEquality
                (@"Parser\Expected\HappyPath.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void Parser_UnsupportedOperations()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.StartParser(new string[] { @"Parser\Source\UnsupportedOperations.c" });

            TestTools.CompareFileEquality
                (@"Parser\Expected\UnsupportedOperations.txt", CommonTools.OutputFilePath);
        }
    }
}
