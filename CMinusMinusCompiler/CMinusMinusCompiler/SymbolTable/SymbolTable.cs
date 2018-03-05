using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace CMinusMinusCompiler
{
    public class SymbolTable
    {
        private int TableSize { get; }
            = Int32.Parse(ConfigurationManager.AppSettings["SymbolTableSize"]);
        private LinkedList<Node>[] HashTable { get; }
        private static string OutputFormat { get; } = "{0,-38} {1,-30} {2}";

        public SymbolTable()
        {
            HashTable = new LinkedList<Node>[TableSize];
        }

        public Node LookupNode(string lexeme)
        {
            int location = HashLexeme(lexeme);
            return HashTable[location].Where(item => item.Lexeme == lexeme).FirstOrDefault();
        }

        public void InsertNode(string lexeme, Symbol Token, int depth)
        {
            Node newNode = new Node()
            {
                Lexeme = lexeme,
                Token = Token,
                Depth = depth
            };

            int location = HashLexeme(lexeme);
            if (HashTable[location] == null)
            {
                HashTable[location] = new LinkedList<Node>();
 
            

            HashTable[location].AddFirst(newNode); 
        }

        public void DeleteDepth(int depth)
        {
            foreach (LinkedList<Node> nodeList in HashTable)
            {
                if (nodeList != null) nodeList.RemoveAll(node => node.Depth == depth);
            }
        }

        public void OutputSymbolTable(int depth)
        {
            DisplaySymbolTableHeader();
            foreach (LinkedList<Node> nodeList in HashTable)
            {
                if (nodeList != null) WriteNodes(nodeList.Where(item => item.Depth == depth));
            }
            CommonTools.WriteOutput(Environment.NewLine);
        }

        // Display symbol table header to screen and output file
        public void DisplaySymbolTableHeader()
        {
            string[] headingData = new string[] { "Lexeme", "Token", "Depth" };
            string headerRule = Environment.NewLine + new string('-', 75);
            CommonTools.WriteOutput(string.Format(OutputFormat, headingData) + headerRule);
        }

        private void WriteNodes(IEnumerable<Node> nodeList)
        {
            foreach (Node node in nodeList)
            {
                string[] outputData 
                    = new string[] { node.Lexeme, node.Token.ToString(), node.Depth.ToString() };
                CommonTools.WriteOutput(string.Format(OutputFormat, outputData));
            }
        }

        // Modified PJW hash algorithm from:
        // Hashing and Symbol Tables (summarized from “Compilers – Principles, 
        // Techniques, and Tools”, Aho, Sethi, and Ullman, first edition) 
        // and 
        // https://www.programmingalgorithms.com/algorithm/pjw-hash
        private int HashLexeme(string lexeme)
        {
            //const uint ThreeQuarters = ((uint)(sizeof(uint) * 8) * 3) / 4;
            //const uint OneEighth = (uint)(sizeof(uint) * 8) / 8;
            //const uint HighBits = 0xFFFFFFFF << (int)(sizeof(uint) * 8 - OneEighth);
            //uint hash = 0;
            //uint testHash = 0;

            //foreach (char item in lexeme)
            //{
            //    hash = (hash << (int)OneEighth) + ((byte)item);

            //    if ((testHash = hash & HighBits) != 0)
            //    {
            //        hash = ((hash ^ (testHash >> (int)ThreeQuarters)) & (~HighBits));
            //    }
            //}

            //return (int)hash % TableSize;

            return lexeme.GetHashCode() % 211;
        }
    }

    // Enumerated type to contain possible variable data types
    public enum VariableType { Int, Float, Char }
    // Enumerated type to contain possible entry types
    public enum EntryType { Constant, Variable, Function }
}
