using System.Collections.Generic;

namespace CMinusMinusCompiler
{
    // Class to hold properties specific to function node
    public class FunctionNode : Node
    {
        public FunctionNode(string lexeme, Token token, int depth) 
            : base(lexeme, token, depth)
        {}

        public int LocalSize { get; set; }
        public int ParameterCount { get; set; }
        public VariableType ReturnType { get; set; }
        LinkedList<ParameterNode> Parameters { get; set; }
    }
}
