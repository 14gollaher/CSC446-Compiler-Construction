using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CMinusMinusCompiler.Test
{
    [TestClass]
    public class Parser_Test
    {
        [TestMethod]
        public void Parser_HappyPath()
        {
            Bootstrapper.StartParserDebug(new string[] { @"Parser\Source\HappyPath.c" });
            TestTools.CompareFileEquality(@"Parser\Expected\HappyPath.txt", CommonTools.OutputFilePath);
        }
        
        [TestMethod]
        public void Parser_InvalidProgram()
        {
            Bootstrapper.StartParserDebug(new string[] { @"Parser\Source\InvalidProgram.c" });
            TestTools.CompareFileEquality(@"Parser\Expected\InvalidProgram.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void Parser_InvalidIdentifierTail()
        {
            Bootstrapper.StartParserDebug(new string[] { @"Parser\Source\InvalidIdentifierTail.c" });
            TestTools.CompareFileEquality(@"Parser\Expected\InvalidIdentifierTail.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void Parser_InvalidType()
        {
            Bootstrapper.StartParserDebug(new string[] { @"Parser\Source\InvalidType.c" });
            TestTools.CompareFileEquality(@"Parser\Expected\InvalidType.txt", CommonTools.OutputFilePath);
        }


        [TestMethod]
        public void Parser_InvalidCompound()
        {
            Bootstrapper.StartParserDebug(new string[] { @"Parser\Source\InvalidCompound.c" });
            TestTools.CompareFileEquality(@"Parser\Expected\InvalidCompound.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void Parser_InvalidRest()
        {
            Bootstrapper.StartParserDebug(new string[] { @"Parser\Source\InvalidRest.c" });
            TestTools.CompareFileEquality(@"Parser\Expected\InvalidRest.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void Parser_InstructorCase1()
        {
            Bootstrapper.StartParserDebug(new string[] { @"Parser\Source\t31.c" });
            TestTools.CompareFileEquality(@"Parser\Expected\t31.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void Parser_InstructorCase2()
        {
            Bootstrapper.StartParserDebug(new string[] { @"Parser\Source\t32.c" });
            TestTools.CompareFileEquality(@"Parser\Expected\t32.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void Parser_InstructorCase3()
        {
            Bootstrapper.StartParserDebug(new string[] { @"Parser\Source\t33.c" });
            TestTools.CompareFileEquality(@"Parser\Expected\t33.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void Parser_InstructorCase4()
        {
            Bootstrapper.StartParserDebug(new string[] { @"Parser\Source\t34.c" });
            TestTools.CompareFileEquality(@"Parser\Expected\t34.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void Parser_InstructorCase5()
        {
            Bootstrapper.StartParserDebug(new string[] { @"Parser\Source\t35.c" });
            TestTools.CompareFileEquality(@"Parser\Expected\t35.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void Parser_InstructorCase6()
        {
            Bootstrapper.StartParserDebug(new string[] { @"Parser\Source\t36.c" });
            TestTools.CompareFileEquality(@"Parser\Expected\t36.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void Parser_InstructorCase7()
        {
            Bootstrapper.StartParserDebug(new string[] { @"Parser\Source\t37.c" });
            TestTools.CompareFileEquality(@"Parser\Expected\t37.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void Parser_InstructorCase8()
        {
            Bootstrapper.StartParserDebug(new string[] { @"Parser\Source\t38.c" });
            TestTools.CompareFileEquality(@"Parser\Expected\t38.txt", CommonTools.OutputFilePath);
        }
        
    }
}
