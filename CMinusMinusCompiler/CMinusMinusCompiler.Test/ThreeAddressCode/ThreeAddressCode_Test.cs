using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void ThreeAddressCode_HappyPath2()
        {
            Bootstrapper.StartThreeAddressCodeDebug(new string[] { @"ThreeAddressCode\Source\HappyPath2.c" });
            TestTools.CompareFileEquality(@"ThreeAddressCode\Expected\HappyPath2.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void ThreeAddressCode_ReturnSoloConstant()
        {
            Bootstrapper.StartThreeAddressCodeDebug(new string[] { @"ThreeAddressCode\Source\ReturnSoloConstant.c" });
            TestTools.CompareFileEquality(@"ThreeAddressCode\Expected\ReturnSoloConstant.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void ThreeAddressCode_InvalidAssignment()
        {
            Bootstrapper.StartThreeAddressCodeDebug(new string[] { @"ThreeAddressCode\Source\InvalidAssignment.c" });
            TestTools.CompareFileEquality(@"ThreeAddressCode\Expected\InvalidAssignment.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void ThreeAddressCode_NoMain()
        {
            Bootstrapper.StartThreeAddressCodeDebug(new string[] { @"ThreeAddressCode\Source\NoMain.c" });
            TestTools.CompareFileEquality(@"ThreeAddressCode\Expected\NoMain.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void ThreeAddressCode_DepthNameCheck()
        {
            Bootstrapper.StartThreeAddressCodeDebug(new string[] { @"ThreeAddressCode\Source\DepthNameCheck.c" });
            TestTools.CompareFileEquality(@"ThreeAddressCode\Expected\DepthNameCheck.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void ThreeAddressCode_ClassCase1()
        {
            Bootstrapper.StartThreeAddressCodeDebug(new string[] { @"ThreeAddressCode\Source\ClassCase1.c" });
            TestTools.CompareFileEquality(@"ThreeAddressCode\Expected\ClassCase1.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void ThreeAddressCode_ClassCase2()
        {
            Bootstrapper.StartThreeAddressCodeDebug(new string[] { @"ThreeAddressCode\Source\ClassCase2.c" });
            TestTools.CompareFileEquality(@"ThreeAddressCode\Expected\ClassCase2.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void ThreeAddressCode_ClassCase3()
        {
            Bootstrapper.StartThreeAddressCodeDebug(new string[] { @"ThreeAddressCode\Source\ClassCase3.c" });
            TestTools.CompareFileEquality(@"ThreeAddressCode\Expected\ClassCase3.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void ThreeAddressCode_ClassCase4()
        {
            Bootstrapper.StartThreeAddressCodeDebug(new string[] { @"ThreeAddressCode\Source\ClassCase4.c" });
            TestTools.CompareFileEquality(@"ThreeAddressCode\Expected\ClassCase4.txt", CommonTools.OutputFilePaths[0]);
        }
    }
}
