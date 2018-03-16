namespace CMinusMinusCompiler
{
    // Class to hold symbol information used by other components of compiler
    public class Node
    {
        public Symbol Token;
        public string Lexeme { get; set; }
        public int Depth { get; set; }
        public EntryType EntryType { get; set; } // Don't think we'll need this since OOP
       
        // Construtor receiving core node information
        public Node(string lexeme, Symbol token, int depth)
        {
            Lexeme = lexeme;
            Token = token;
            Depth = depth;
        }
    }
}
