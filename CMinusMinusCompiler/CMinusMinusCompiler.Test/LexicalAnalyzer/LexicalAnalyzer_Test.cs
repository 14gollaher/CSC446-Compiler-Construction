﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text.RegularExpressions;

namespace CMinusMinusCompiler.Test
{
    [TestClass]
    public class LexicalAnalyzer_Test
    {
        [TestMethod]
        public void LexicalAnalyzer_HappyPath()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(new string[] { @"LexicalAnalyzer\Source\HappyPath.c" });

            TestTools.CompareFileEquality
                (@"LexicalAnalyzer\Expected\HappyPath.txt", CommonTools.OutputFilePath);

        }

        [TestMethod]
        public void LexicalAnalyzer_Identifiers()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(new string[] { @"LexicalAnalyzer\Source\Identifiers.c" });

            TestTools.CompareFileEquality
                (@"LexicalAnalyzer\Expected\Identifiers.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void LexicalAnalyzer_Numbers()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(new string[] { @"LexicalAnalyzer\Source\Numbers.c" });

            TestTools.CompareFileEquality
                (@"LexicalAnalyzer\Expected\Numbers.txt", CommonTools.OutputFilePath);
        }


        [TestMethod]
        public void LexicalAnalyzer_Comments()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(new string[] { @"LexicalAnalyzer\Source\Comments.c" });

            TestTools.CompareFileEquality
                (@"LexicalAnalyzer\Expected\Comments.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void LexicalAnalyzer_Symbols()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(new string[] { @"LexicalAnalyzer\Source\Symbols.c" });

            TestTools.CompareFileEquality
                (@"LexicalAnalyzer\Expected\Symbols.txt", CommonTools.OutputFilePath);
        }

        [TestMethod]
        public void LexicalAnalyzer_StringLiterals()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(new string[] { @"LexicalAnalyzer\Source\StringLiterals.c" });

            TestTools.CompareFileEquality
                (@"LexicalAnalyzer\Expected\StringLiterals.txt", CommonTools.OutputFilePath);
        }
    }
}
