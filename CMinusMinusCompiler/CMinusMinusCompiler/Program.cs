/*
    Matthew Gollaher
    C-Minus Minus Compiler

    Program target language "C--" code, a subset of C, 
    into Intel assembly code. The compiler features 8 
    modules:
        Lexical Analyzer (DONE)
        Syntax Analyzer (DONE)
        Symbol Table (DONE)
        Semantic Analyzer (TODO)
        Intermediate Code Generator (TODO)
        Code Optimizer (TODO)
        Code Generator (TODO)


   QUESTIONS:
   * Shouldn't const have a type involved? or are we supposed to just look a the value and determine it ourselves?
   * The return type of a function is always void currently, true or false?
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