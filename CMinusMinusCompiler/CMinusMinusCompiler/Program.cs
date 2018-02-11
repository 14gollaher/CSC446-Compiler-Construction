/*
    Matthew Gollaher
    C-Minus Minus Compiler

    Program target language "C--" code, a subset of C, 
    into Intel assembly code. The compiler features 8 
    modules:
        Lexical Analyzer (DONE)
        Syntax Analyzer (DONE)
        Semantic Analyzer (TODO)
        Intermediate Code Generator (TODO)
        Code Optimizer (TODO)
        Code Generator (TODO)
        Symbol Table (TODO)
*/

/* Questions: 

Do we need to support more "stuff"?
    - Types(double, long/short float, etc)
        - And if not, why is void not a type? 
    - Assignment
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