using System;
using System.Collections.Generic;
using System.Linq;

namespace CMinusMinusCompiler
{
    // Symbol table class hold core node information in a linked implementation
    // of a hash table. The hash table with chaining supports insertion, deletion,
    // and lookup methods to maintain symbol information for other compiler components
    public class SymbolTable
    {
        // Private members
        private LinkedList<Node>[] HashTable { get; }
        public int StringLiteralCount { get; set; }
        private static string OutputFormat { get; } = "{0,-38} {1,-30} {2}";
        private static string OutputDetailedFormat { get; }
            = "{0,-15} {1,-12} {2,-9} {3,-9} {4,-12} {5,-9} {6}";

        // Constructor to create empty symbol table
        public SymbolTable()
        {
            HashTable = new LinkedList<Node>[GlobalConfiguration.TableSize];
        }

        // Inserts a node into the symbol table, requiring node informations
        public bool InsertNode(Node newNode)
        {
            Node node = LookupNode(newNode.Lexeme);

            if (node != null && node.Depth == newNode.Depth)
            {
                if (!CommonTools.ParserDebug) return false;
            }

            int location = HashLexeme(newNode.Lexeme);
            if (HashTable[location] == null) HashTable[location] = new LinkedList<Node>();

            HashTable[location].AddFirst(newNode);

            if (newNode is StringLiteralNode) StringLiteralCount++;

            return true;
        }

        // Deletes entire given scope/depth level from symbol table
        public void DeleteDepth(int depth)
        {
            foreach (LinkedList<Node> nodeList in HashTable)
            {
                if (nodeList != null) nodeList.RemoveAll(node => node.Depth == depth);
            }
        }
        
        // Displays given depth/scope level to output
        public void OutputSymbolTable(int depth)
        {
            if (CommonTools.SemanticAnalysisDebug && !CommonTools.ThreeAddressCodeRun)
            {
                OutputDetailedSymbolTable(depth);
                return;
            }

            if (!CommonTools.ParserDebug && !CommonTools.ThreeAddressCodeRun)
            {
                DisplaySymbolTableHeader(depth);
                WriteNodes(GetSymbolTableDepth(depth));
                CommonTools.WriteOutput(Environment.NewLine);
            }
        }

        // Returns given depth/scope level
        public List<Node> GetSymbolTableDepth(int depth)
        {
            List<Node> nodes = new List<Node>();
            foreach (LinkedList<Node> nodeList in HashTable)
            {
                if (nodeList != null) nodes.AddRange(nodeList.Where(item => item.Depth == depth));
            }
            return nodes;
        }

        // Displays given depth's detailed symbol table information to output
        public void OutputDetailedSymbolTable(int depth)
        {
            DisplayDetailedSymbolTableHeader(depth);
            foreach (LinkedList<Node> nodeList in HashTable)
            {
                if (nodeList != null) WriteDetailedNodes(nodeList.Where(item => item.Depth == depth));
            }
            CommonTools.WriteOutput(Environment.NewLine);
        }

        // Returns first node found (most recent insert) in table matching given lexeme
        public Node LookupNode(string lexeme)
        {
            int location = HashLexeme(lexeme);

            if (HashTable[location] != null)
            {
                return HashTable[location].Where(item => item.Lexeme == lexeme).FirstOrDefault();
            }
            return null;
        }

        // Display symbol table header to screen and output file
        private void DisplaySymbolTableHeader(int depth)
        {
            CommonTools.WriteOutput($"** Symbol Table at Depth {depth} **");
            string[] headingData = new string[] { "Lexeme", "Class", "Depth" };
            string headerRule = Environment.NewLine + new string('-', 75);
            CommonTools.WriteOutput(string.Format(OutputFormat, headingData) + headerRule);
        }

        // Display detailed table header to screen and output file
        private void DisplayDetailedSymbolTableHeader(int depth)
        {
            CommonTools.WriteOutput($"** Symbol Table at Depth {depth} **");
            string[] headingData = new string[] { "Lexeme", "Class", "Depth", "Offset", "Type", "Size", "Value(r)" };
            string headerRule = Environment.NewLine + new string('-', 80);
            CommonTools.WriteOutput(string.Format(OutputDetailedFormat, headingData) + headerRule);
        }

        // Helper function to write given list of nodes to screen
        private void WriteNodes(IEnumerable<Node> nodeList)
        {
            foreach (Node node in nodeList)
            {
                string[] outputData = new string[] { node.Lexeme, node.GetClass(), node.Depth.ToString() };
                CommonTools.WriteOutput(string.Format(OutputFormat, outputData));
            }
        }

        // Helper function to write detailed node information given list of nodes to screen
        private void WriteDetailedNodes(IEnumerable<Node> nodeList)
        {
            foreach (Node node in nodeList)
            {
                if (node is FunctionNode) OutputDetailedFunction(node);
                else if (node is VariableNode) OutputDetailedVariable(node);
                else OutputDetailedConstant(node);
            }
        }

        // Helper function to output data for functions
        private void OutputDetailedConstant(Node node)
        {
            ConstantNode outputNode = (ConstantNode)node;
            var outputValue = outputNode.Value ?? outputNode.ValueReal;
            string[] outputData = new string[] { outputNode.Lexeme, outputNode.GetClass(), outputNode.Depth.ToString(),
                                                 "-", "-", "-", outputValue.ToString()};
            CommonTools.WriteOutput(string.Format(OutputDetailedFormat, outputData));
        }

        // Helper function to output data for functions
        private void OutputDetailedFunction(Node node)
        {
            string[] outputData;
            FunctionNode outputNode = (FunctionNode)node;

            outputData = new string[] { outputNode.Lexeme, outputNode.GetClass(), outputNode.Depth.ToString(),
                                        "-", outputNode.ReturnType.ToString(), outputNode.LocalsSize.ToString(), "-" };
            CommonTools.WriteOutput(string.Format(OutputDetailedFormat, outputData));

            foreach (ParameterNode item in outputNode.Parameters)
            {
                outputData = new string[] { "-", "Parameter", "-", "-", item.Type.ToString(), "-", "-" };
                CommonTools.WriteOutput(string.Format(OutputDetailedFormat, outputData));
            }

        }

        // Helper function to output data for variables
        private void OutputDetailedVariable(Node node)
        {
            VariableNode outputNode = (VariableNode)node;
            string[] outputData = new string[] { outputNode.Lexeme, outputNode.GetClass(), outputNode.Depth.ToString(),
                                                 outputNode.Offset.ToString(), outputNode.Type.ToString(),
                                                 outputNode.Size.ToString(), "-" };

            CommonTools.WriteOutput(string.Format(OutputDetailedFormat, outputData));
        }

        // Function to hash lexeme into a valid integer for symbol table
        private int HashLexeme(string lexeme)
        {   
            return Math.Abs(lexeme.GetHashCode()) % GlobalConfiguration.TableSize;
        }
    }
}