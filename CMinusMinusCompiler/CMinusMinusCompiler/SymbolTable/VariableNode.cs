namespace CMinusMinusCompiler
{
    public class VariableNode : Node
    {
        public VariableType Type { get; set; }
        public int Offset { get; set; }
        public int Size { get; set; }
    }
}
