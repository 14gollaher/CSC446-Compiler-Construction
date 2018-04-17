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


    Todo: * What is offset of _bp-x etc if it is a float value?
          * What is offset of _bp-x etc if it is a const float value?
          * Is it okay if I make new _bp-x each time i'm assigning to a constant?
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