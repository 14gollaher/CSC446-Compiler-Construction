/*
    Matthew Gollaher
    C-Minus Minus Compiler

    Program target language "C--" code, a subset of C, 
    into Intel assembly code. The compiler features 8 
    modules:
        Lexical Analyzer (DONE)
        Syntax Analyzer (TODO)
        Semantic Analyzer (TODO)
        Intermediate Code Generator (TODO)
        Code Optimizer (TODO)
        Code Generator (TODO)
        Symbol Table (TODO)
*/

namespace CMinusMinusCompiler
{
    public class Program
    {
        // Method calls C-- Bootstrapper
        static void Main(string[] arguments)
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Start(arguments);
        }
    }
}
