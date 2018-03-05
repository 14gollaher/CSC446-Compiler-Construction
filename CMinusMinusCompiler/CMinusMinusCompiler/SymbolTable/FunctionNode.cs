using System.Collections.Generic;

namespace CMinusMinusCompiler.HashTable
{
    public class FunctionNode
    {
        public int LocalSize { get; set; }
        public int ParameterCount { get; set; }
        public VariableType ReturnType { get; set; }
        //public ParameterNode ParameterNode { get; set; }
        LinkedList<ParameterNode> parameters { get; set; }
    }
}
