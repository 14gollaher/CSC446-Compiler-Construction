using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CMinusMinusCompiler.Test
{
    public static class TestTools
    {
        public static void CompareFileEquality(string expectedPath, string outputPath)
        {
            string expected = StandardFileRead(expectedPath);
            string output = StandardFileRead(outputPath);
            Assert.AreEqual(expected, output);
        }

        private static string StandardFileRead(string filePath)
        {
            return Regex.Replace(File.ReadAllText(filePath), "\\s+", " ").TrimEnd();
        }
    }
}
