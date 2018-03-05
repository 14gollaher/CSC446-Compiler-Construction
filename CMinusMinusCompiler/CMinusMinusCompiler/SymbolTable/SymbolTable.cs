using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace CMinusMinusCompiler
{
    public class SymbolTable
    {
        public int TableSize { get; } =
            Int32.Parse(ConfigurationManager.AppSettings["SymbolTableSize"]);

        public LinkedList<Node>[] Nodes = new LinkedList<Node>[211];

        public Node LookupNode(string lexeme)
        {
            //return Nodes.Where(item => item.Lexeme == lexeme).FirstOrDefault();
        }
        public void InsertNode(string lexeme, Symbol Token, int depth)
        {

            Node newNode = new Node()
            {
                Lexeme = lexeme,
                Token = Token,
                Depth = depth
            };

        }
        public void DeleteDepth(int depth)
        {

        }
        public void WriteTable(int depth)
        {

        }
        private int HashNode(string lexeme)
        {
            int location = 0;
            foreach (char item in lexeme)
            {
                location = (location << 24) + (item);
            }
        }
    }

    // Enumerated type to contain possible variable data types
    public enum VariableType
    {
        Int, Float, Char
    }

    public enum EntryType
    {
        Constant, Variable, Function
    }
}
