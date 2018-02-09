using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CMinusMinusCompiler.Test
{
    [TestClass]
    public class LexicalAnalyzer_Test
    {
        [TestMethod]
        public void LexicalAnalyzer_HappyPath()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\HappyPath.c" });

            TestTools.CompareFileEquality
                (@"LexicalAnalyzer\Expected\HappyPath.txt", CommonTools.OutputFilePath);

        }

        [TestMethod]
        public void LexicalAnalyzer_Identifiers()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\Identifiers.c" });

            TestTools.CompareFileEquality
                (@"LexicalAnalyzer\Expected\Identifiers.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void LexicalAnalyzer_Numbers()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\Numbers.c" });

            TestTools.CompareFileEquality
                (@"LexicalAnalyzer\Expected\Numbers.txt", CommonTools.OutputFilePath);
        }


        [TestMethod]
        public void LexicalAnalyzer_Comments()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\Comments.c" });

            TestTools.CompareFileEquality
                (@"LexicalAnalyzer\Expected\Comments.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void LexicalAnalyzer_Symbols()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\Symbols.c" });

            TestTools.CompareFileEquality
                (@"LexicalAnalyzer\Expected\Symbols.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void LexicalAnalyzer_StringLiterals()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\StringLiterals.c" });

            TestTools.CompareFileEquality
                (@"LexicalAnalyzer\Expected\StringLiterals.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void LexicalAnalyzer_ReservedWords()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\ReservedWords.c" });

            TestTools.CompareFileEquality
                (@"LexicalAnalyzer\Expected\ReservedWords.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void LexicalAnaylzer_FullWalkthrough()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\FullWalkthrough.c" });

            TestTools.CompareFileEquality
                (@"LexicalAnalyzer\Expected\FullWalkthrough.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void LexicalAnaylzer_InstructorTestFile1()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(new string[] { @"LexicalAnalyzer\Source\Tokens.c" });

            TestTools.CompareFileEquality
                (@"LexicalAnalyzer\Expected\Tokens.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void LexicalAnaylzer_InstructorTestFile2()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(new string[] { @"LexicalAnalyzer\Source\Tokens2.c" });

            TestTools.CompareFileEquality
                (@"LexicalAnalyzer\Expected\Tokens2.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void LexicalAnaylzer_InstructorTestFile3()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(new string[] { @"LexicalAnalyzer\Source\Tokens3.c" });

            TestTools.CompareFileEquality
                (@"LexicalAnalyzer\Expected\Tokens3.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void LexicalAnaylzer_InstructorTestFile4()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(new string[] { @"LexicalAnalyzer\Source\Tokens4.c" });

            TestTools.CompareFileEquality
                (@"LexicalAnalyzer\Expected\Tokens4.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void LexicalAnaylzer_InstructorTestFile5()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(new string[] { @"LexicalAnalyzer\Source\Tokens5.c" });

            TestTools.CompareFileEquality
                (@"LexicalAnalyzer\Expected\Tokens5.txt", CommonTools.OutputFilePath);
        }
    }
}
