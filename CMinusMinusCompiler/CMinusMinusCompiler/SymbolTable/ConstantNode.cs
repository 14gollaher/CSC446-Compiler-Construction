namespace CMinusMinusCompiler
{
    // Class to hold properties specific to constant node
    public class ConstantNode : Node
    {
        public ConstantNode(string lexeme, Token token, int depth, int offset, int? value = null) 
            : base(lexeme, token, depth)
        {
            Offset = offset;
            Value = value;
        }

        public ConstantNode(string lexeme, Token token, int depth, int offset, float? valueReal = null)
            : base(lexeme, token, depth)
        {
            Offset = offset;
            ValueReal = valueReal;
        }
        public int Offset { get; set; }
        public int? Value { get; set; }
        public float? ValueReal { get; set; }
    }
}
