using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text.RegularExpressions;

namespace CMinusMinusCompiler.Test
{
    [TestClass]
    public class LexicalAnalysis_Test
    {
        [TestMethod]
        public void LexicalAnalysis_HappyPath()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(new string[] { @"LexicalAnalysis\Source\HappyPath.c" });

            string output = File.ReadAllText(CommonTools.OutputFilePath);
            string expected = File.ReadAllText(@"LexicalAnalysis\Expected\HappyPath.txt");
            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void LexicalAnalysis_Identifiers()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(new string[] { @"LexicalAnalysis\Source\Identifiers.c" });

            string expected = Regex.Replace(File.ReadAllText(@"LexicalAnalysis\Expected\Identifiers.txt"), "\\s+", " ");
            string output = Regex.Replace(File.ReadAllText(CommonTools.OutputFilePath), "\\s+", " ");

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void LexicalAnalysis_Numbers()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(new string[] { @"LexicalAnalysis\Source\Numbers.c" });

            string output = File.ReadAllText(CommonTools.OutputFilePath);
            string expected = File.ReadAllText(@"LexicalAnalysis\Expected\Numbers.txt");
            Assert.AreEqual(expected, output);
        }


        [TestMethod]
        public void LexicalAnalysis_Comments()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(new string[] { @"LexicalAnalysis\Source\Comments.c" });

            string output = File.ReadAllText(CommonTools.OutputFilePath);
            string expected = File.ReadAllText(@"LexicalAnalysis\Expected\Comments.txt");
            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void LexicalAnalysis_SingleCharacterSymbols()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(new string[] { @"LexicalAnalysis\Source\SingleCharacterSymbols.c" });

            string output = File.ReadAllText(CommonTools.OutputFilePath);
            string expected = File.ReadAllText(@"LexicalAnalysis\Expected\SingleCharacterSymbols.txt");
            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void LexicalAnalysis_List1()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(new string[] { @"LexicalAnalysis\Source\List1.c" });

            string output = File.ReadAllText(CommonTools.OutputFilePath);
            string expected = File.ReadAllText(@"LexicalAnalysis\Expected\List1.txt");
            Assert.AreEqual(expected, output);
        }
    }
}
