namespace CMinusMinusCompiler
{
    // Class to hold properties specific to variable node
    public class VariableNode : Node
    {
        public VariableNode(string lexeme, Token token, int depth) 
            : base(lexeme, token, depth)
        {}

        public VariableType Type { get; set; }
        public int Offset { get; set; }
        public int Size { get; set; }
    }
}
