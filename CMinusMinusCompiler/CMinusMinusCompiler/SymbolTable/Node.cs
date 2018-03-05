namespace CMinusMinusCompiler
{
    public class Node
    {
        public Symbol Token;
        public string Lexeme { get; set; }
        public int Depth { get; set; }
        public EntryType EntryType { get; set; }
        //public Node NextNode { get; set; }
    }
}
