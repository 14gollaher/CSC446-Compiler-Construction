namespace CMinusMinusCompiler
{
    // Class to hold properties specific to variable node
    public class VariableNode
    {
        public VariableType Type { get; set; }
        public int Offset { get; set; }
        public int Size { get; set; }
    }
}
