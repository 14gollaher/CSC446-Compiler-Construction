namespace CMinusMinusCompiler
{
    // Class to hold properties specific to constant node
    public class ConstantNode : Node
    {
        // Public properties
        public int? Value { get; set; }
        public float? ValueReal { get; set; }

        // Function to set the appropriate stored value
        public void SetValues(int? value, float? valueReal)
        {
            Value = value;
            ValueReal = valueReal;
        }
    }
}
