namespace CMinusMinusCompiler
{
    // Class to hold properties specific to constant node
    public class ConstantNode : Node
    {
        public ConstantNode(string lexeme, Symbol token, int depth) 
            : base(lexeme, token, depth)
        {}
        public VariableType Type { get; set; }
        public int Offset { get; set; }
        public int? Value { get; set; }
        public int? ValueR { get; set; }
    }
}
