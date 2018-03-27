﻿/*
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
   * When should we print to the screen? Are we using a single command line arg still?
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