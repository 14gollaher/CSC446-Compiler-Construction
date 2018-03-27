using System;

namespace CMinusMinusCompiler
{
    // Class to hold symbol information used by other components of compiler
    public abstract class Node
    {
        public string Lexeme { get; set; }
        public int Depth { get; set; }

        public string GetClass()
        {
            if (this is VariableNode) return "Variable";
            else if (this is ConstantNode) return "Constant";
            else return "Function";
        }
    }
}
