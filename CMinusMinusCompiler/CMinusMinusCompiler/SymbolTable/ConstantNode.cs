namespace CMinusMinusCompiler
{
    // Class to hold properties specific to constant node
    public class ConstantNode : Node
    {
        public void SetValues(int? value, float? valueReal)
        {
            Value = value;
            ValueReal = valueReal;
        }

        public int? Value { get; set; }
        public float? ValueReal { get; set; }
    }
}
