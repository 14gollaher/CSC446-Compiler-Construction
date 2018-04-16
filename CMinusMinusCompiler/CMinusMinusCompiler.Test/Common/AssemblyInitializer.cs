using CMinusMinusCompiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public static class AssemblyInitializer
{
    [AssemblyInitialize]
    public static void Initialize(TestContext context)
    {
        CommonTools.UnitTestExecution = true;
    }
}