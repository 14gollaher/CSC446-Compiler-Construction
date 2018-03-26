using System.Collections.Generic;

namespace CMinusMinusCompiler
{
    // Class to hold properties specific to function node
    public class FunctionNode : Node
    {
        public FunctionNode(string lexeme, Token token, int depth, int localSize,
            VariableType returnType, LinkedList<ParameterNode> parameters)
            : base(lexeme, token, depth)
        {
            LocalSize = localSize;
            ReturnType = returnType;
            Parameters = Parameters;
        }

        public int LocalSize { get; set; }
        public VariableType ReturnType { get; set; }
        LinkedList<ParameterNode> Parameters { get; set; }
    }
}
