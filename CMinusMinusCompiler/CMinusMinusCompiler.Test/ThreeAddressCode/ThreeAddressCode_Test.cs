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

        [TestMethod]
        public void ThreeAddressCode_InstructorCase1()
        {
            Bootstrapper.StartThreeAddressCodeDebug(new string[] { @"ThreeAddressCode\Source\test1.c" });
            TestTools.CompareFileEquality(@"ThreeAddressCode\Expected\test1.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void ThreeAddressCode_InstructorCase2()
        {
            Bootstrapper.StartThreeAddressCodeDebug(new string[] { @"ThreeAddressCode\Source\test2.c" });
            TestTools.CompareFileEquality(@"ThreeAddressCode\Expected\test2.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void ThreeAddressCode_InstructorCase3()
        {
            Bootstrapper.StartThreeAddressCodeDebug(new string[] { @"ThreeAddressCode\Source\test3.c" });
            TestTools.CompareFileEquality(@"ThreeAddressCode\Expected\test3.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void ThreeAddressCode_InstructorCase4()
        {
            Bootstrapper.StartThreeAddressCodeDebug(new string[] { @"ThreeAddressCode\Source\test4.c" });
            TestTools.CompareFileEquality(@"ThreeAddressCode\Expected\test4.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void ThreeAddressCode_InstructorCase5()
        {
            Bootstrapper.StartThreeAddressCodeDebug(new string[] { @"ThreeAddressCode\Source\test5.c" });
            TestTools.CompareFileEquality(@"ThreeAddressCode\Expected\test5.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void ThreeAddressCode_InstructorCase6()
        {
            Bootstrapper.StartThreeAddressCodeDebug(new string[] { @"ThreeAddressCode\Source\test6.c" });
            TestTools.CompareFileEquality(@"ThreeAddressCode\Expected\test6.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void ThreeAddressCode_InstructorCase7()
        {
            Bootstrapper.StartThreeAddressCodeDebug(new string[] { @"ThreeAddressCode\Source\test7.c" });
            TestTools.CompareFileEquality(@"ThreeAddressCode\Expected\test7.txt", CommonTools.OutputFilePaths[0]);
        }

        [TestMethod]
        public void ThreeAddressCode_InstructorCase8()
        {
            Bootstrapper.StartThreeAddressCodeDebug(new string[] { @"ThreeAddressCode\Source\test8.c" });
            TestTools.CompareFileEquality(@"ThreeAddressCode\Expected\test8.txt", CommonTools.OutputFilePaths[0]);
        }
    }
}
