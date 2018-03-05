namespace CMinusMinusCompiler
{
    public class Node
    {
        public Symbol Token;
        public string Lexeme { get; set; }
        public int Depth { get; set; }
        // public EntryType EntryType { get; set; } Don't think we'll need this since OOP
        public VariableNode Variable { get; set; }
        public ConstantNode Constant { get; set; }
        public FunctionNode Function { get; set; }
    }
}
