﻿namespace CMinusMinusCompiler
{
    // Class to hold symbol information used by other components of compiler
    public abstract class Node
    {
        // Public propertiies
        public string Lexeme { get; set; }
        public int Depth { get; set; }

        // Get's the string readable class of current node
        public string GetClass()
        {
            if (this is VariableNode) return "Variable";
            else if (this is ConstantNode) return "Constant";
            else return "Function";
        }
    }
}
