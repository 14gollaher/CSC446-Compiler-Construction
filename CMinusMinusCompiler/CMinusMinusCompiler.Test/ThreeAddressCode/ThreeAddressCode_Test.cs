﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CMinusMinusCompiler.Test
{
    [TestClass]
    public class ThreeAddressCode_Test
    {

        [TestMethod]
        public void ThreeAddressCode_HappyPath()
        {
            Bootstrapper.StartThreeAddressCodeDebug(new string[] { @"ThreeAddressCode\Source\HappyPath.c" });
            TestTools.CompareFileEquality(@"ThreeAddressCode\Expected\HappyPath.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void ThreeAddressCode_ClassCase1()
        {
            Bootstrapper.StartThreeAddressCodeDebug(new string[] { @"ThreeAddressCode\Source\ClassCase1.c" });
            TestTools.CompareFileEquality(@"ThreeAddressCode\Expected\ClassCase1.txt", CommonTools.OutputFilePaths[0]);
        }
    }
}
