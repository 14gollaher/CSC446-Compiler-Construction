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
        public void Parser_InvalidProgram()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.StartParser(new string[] { @"Parser\Source\InvalidProgram.c" });

            TestTools.CompareFileEquality
                (@"Parser\Expected\InvalidProgram.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void Parser_InvalidIdentifierTail()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.StartParser(new string[] { @"Parser\Source\InvalidIdentifierTail.c" });

            TestTools.CompareFileEquality
                (@"Parser\Expected\InvalidIdentifierTail.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void Parser_InvalidType()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.StartParser(new string[] { @"Parser\Source\InvalidType.c" });

            TestTools.CompareFileEquality
                (@"Parser\Expected\InvalidType.txt", CommonTools.OutputFilePath);
        }


        [TestMethod]
        public void Parser_InvalidCompound()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.StartParser(new string[] { @"Parser\Source\InvalidCompound.c" });

            TestTools.CompareFileEquality
                (@"Parser\Expected\InvalidCompound.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void Parser_InvalidRest()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.StartParser(new string[] { @"Parser\Source\InvalidRest.c" });

            TestTools.CompareFileEquality
                (@"Parser\Expected\InvalidRest.txt", CommonTools.OutputFilePath);
        }
    }
}
