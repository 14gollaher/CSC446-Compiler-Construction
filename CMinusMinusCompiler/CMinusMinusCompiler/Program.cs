/*
    Matthew Gollaher
    C-Minus Minus Compiler

    Program target language "C--" code, a subset of C, into Intel assembly code. 
    The compiler features 8 modules:
        Lexical Analyzer (DONE)
        Syntax Analyzer (DONE)
        Symbol Table (DONE)
        Semantic Analysis (IN PROGRESS)
        Intermediate Code Generator (IN PROGRESS)
        Code Optimizer (TODO)
        Code Generator (TODO)


    Questions:
    * Do we need to be type checking like so:
        int a = 5; a(); // invalid as a is not a function
    * Do we need to be making sure assigning to a function is a real function?
        int a = f(); // f is undeclared
*/

namespace CMinusMinusCompiler
{
    public class Program
    {
        // Method calls C-- Bootstrapper
        static void Main(string[] arguments)
        {
            Bootstrapper.Start(arguments);
        }
    }
}