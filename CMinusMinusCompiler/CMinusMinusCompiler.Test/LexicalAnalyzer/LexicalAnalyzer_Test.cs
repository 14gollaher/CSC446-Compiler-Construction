﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CMinusMinusCompiler.Test
{
    [TestClass]
    public class LexicalAnalyzer_Test
    {
        [TestMethod]
        public void LexicalAnalyzer_HappyPath()
        {
            Bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\HappyPath.c" });
            TestTools.CompareFileEquality(@"LexicalAnalyzer\Expected\HappyPath.txt", CommonTools.OutputFilePaths[0]);

        }

        [TestMethod]
        public void LexicalAnalyzer_Identifiers()
        {
            Bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\Identifiers.c" });
            TestTools.CompareFileEquality(@"LexicalAnalyzer\Expected\Identifiers.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void LexicalAnalyzer_Numbers()
        {
            Bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\Numbers.c" });
            TestTools.CompareFileEquality(@"LexicalAnalyzer\Expected\Numbers.txt", CommonTools.OutputFilePaths[0]);
        }


        [TestMethod]
        public void LexicalAnalyzer_Comments()
        {
            Bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\Comments.c" });
            TestTools.CompareFileEquality(@"LexicalAnalyzer\Expected\Comments.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void LexicalAnalyzer_Symbols()
        {
            Bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\Symbols.c" });
            TestTools.CompareFileEquality(@"LexicalAnalyzer\Expected\Symbols.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void LexicalAnalyzer_StringLiterals()
        {
            Bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\StringLiterals.c" });
            TestTools.CompareFileEquality(@"LexicalAnalyzer\Expected\StringLiterals.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void LexicalAnalyzer_ReservedWords()
        {
            Bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\ReservedWords.c" });
            TestTools.CompareFileEquality(@"LexicalAnalyzer\Expected\ReservedWords.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void LexicalAnaylzer_FullWalkthrough()
        {
            Bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\FullWalkthrough.c" });
            TestTools.CompareFileEquality(@"LexicalAnalyzer\Expected\FullWalkthrough.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void LexicalAnaylzer_InstructorCase1()
        {
            Bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\Tokens.c" });
            TestTools.CompareFileEquality(@"LexicalAnalyzer\Expected\Tokens.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void LexicalAnaylzer_InstructorCase2()
        {
            Bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\Tokens2.c" });
            TestTools.CompareFileEquality(@"LexicalAnalyzer\Expected\Tokens2.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void LexicalAnaylzer_InstructorCase3()
        {
            Bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\Tokens3.c" });
            TestTools.CompareFileEquality(@"LexicalAnalyzer\Expected\Tokens3.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void LexicalAnaylzer_InstructorCase4()
        {
            Bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\Tokens4.c" });
            TestTools.CompareFileEquality(@"LexicalAnalyzer\Expected\Tokens4.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void LexicalAnaylzer_InstructorCase5()
        {
            Bootstrapper.StartLexicalAnalyzer(new string[] { @"LexicalAnalyzer\Source\Tokens5.c" });
            TestTools.CompareFileEquality(@"LexicalAnalyzer\Expected\Tokens5.txt", CommonTools.OutputFilePaths[0]);
        }
    }
}
