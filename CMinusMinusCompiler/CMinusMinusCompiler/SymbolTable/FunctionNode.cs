using System.Collections.Generic;

namespace CMinusMinusCompiler
{
    // Class to hold properties specific to function node
    public class FunctionNode : Node
    {
        public int LocalSize { get; set; }
        public Token ReturnType { get; set; }
        LinkedList<ParameterNode> Parameters { get; set; }
    }
}
