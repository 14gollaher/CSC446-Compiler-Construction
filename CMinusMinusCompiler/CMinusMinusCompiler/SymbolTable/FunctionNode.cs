using System.Collections.Generic;

namespace CMinusMinusCompiler
{
    public class FunctionNode
    {
        public int LocalSize { get; set; }
        public int ParameterCount { get; set; }
        public VariableType ReturnType { get; set; }
        LinkedList<ParameterNode> Parameters { get; set; }
    }
}
