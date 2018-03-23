﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace CMinusMinusCompiler
{
    // Symbol table class hold core node information in a linked implementation
    // of a hash table. The hash table with chaining supports insertion, deletion,
    // and lookup methods to maintain symbol information for other compiler components
    public class SymbolTable
    {
        // Private members
        private int TableSize { get; }
            = Int32.Parse(ConfigurationManager.AppSettings["SymbolTableSize"]);
        private LinkedList<Node>[] HashTable { get; }
        private static string OutputFormat { get; } = "{0,-38} {1,-30} {2}";

        // Constructor to create empty symbol table
        public SymbolTable()
        {
            HashTable = new LinkedList<Node>[TableSize];
        }

        // Returns first node found (most recent insert)
        // in symbol table matching given lexeme
        public Node LookupNode(string lexeme)
        {
            int location = HashLexeme(lexeme);
            if (HashTable[location] != null)
            {
                return HashTable[location].Where(item => item.Lexeme == lexeme).FirstOrDefault();
            }
            return null;
        }

        // Inserts a node into the symbol table, requiring node informations
        public void InsertNode(string lexeme, Token token, int depth)
        {
            Node node = LookupNode(lexeme);

            if (node != null && node.Depth == depth)
            {
                CommonTools.WriteOutput(
                    $"ERROR: Duplicate lexeme \"{lexeme}\" with depth \"{depth}\" exists");
                return;
            }

            int location = HashLexeme(lexeme);
            if (HashTable[location] == null) HashTable[location] = new LinkedList<Node>();

            // Just doing base class of node since we have no way to tell which type of node
            // to insert at this point
            node = new Node(lexeme, token, depth);
            HashTable[location].AddFirst(node);
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
            DisplaySymbolTableHeader();
            foreach (LinkedList<Node> nodeList in HashTable)
            {
                if (nodeList != null) WriteNodes(nodeList.Where(item => item.Depth == depth));
            }
            CommonTools.WriteOutput(Environment.NewLine);
        }

        // Display symbol table header to screen and output file
        private void DisplaySymbolTableHeader()
        {
            string[] headingData = new string[] { "Lexeme", "Token", "Depth" };
            string headerRule = Environment.NewLine + new string('-', 75);
            CommonTools.WriteOutput(string.Format(OutputFormat, headingData) + headerRule);
        }

        // Helper function to write given list of nodes to screen
        private void WriteNodes(IEnumerable<Node> nodeList)
        {
            foreach (Node node in nodeList)
            {
                string[] outputData
                    = new string[] { node.Lexeme, node.Token.ToString(), node.Depth.ToString() };
                CommonTools.WriteOutput(string.Format(OutputFormat, outputData));
            }
        }

        // Function to hash lexeme into a valid integer for symbol table
        private int HashLexeme(string lexeme)
        {   
            return Math.Abs(lexeme.GetHashCode()) % TableSize;
        }
    }
    // Enumerated type to contain possible variable data types
    public enum VariableType { Int, Float, Char }

    // Enumerated type to contain possible entry types
    public enum EntryType { Constant, Variable, Function } // Shouldn't need this since we have different class types
}
