namespace CMinusMinusCompiler
{
    // Class to hold properties specific to variable node
    public class VariableNode : Node
    {
        // Public Properties
        public Token Type { get; set; }
        public int Offset { get; set; }
        // Getter to translate differing variable types to appropriate size
        public int Size
        {
            get
            {
                switch (Type)
                {
                    case Token.CharToken:
                        return Parser.CharacterSize;
                    case Token.IntToken:
                        return Parser.IntegerSize;
                    case Token.FloatToken:
                        return Parser.FloatSize;
                    default:
                        return -1;
                }
            }
        }
    }
}
