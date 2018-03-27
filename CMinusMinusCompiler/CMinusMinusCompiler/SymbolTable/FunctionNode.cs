using System.Collections.Generic;

namespace CMinusMinusCompiler
{
    // Class to hold properties specific to function node
    public class FunctionNode : Node
    {
        // Public properties
        public Token ReturnType { get; set; }
        public List<ParameterNode> Parameters { get; set; } = new List<ParameterNode>();
        public int VariablesSize { get; set; }
    }
}
