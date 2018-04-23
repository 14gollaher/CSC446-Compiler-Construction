using System;

namespace CMinusMinusCompiler
{
    // Class to hold properties specific to parameter node

    public class ParameterNode
    {
        public Token Type { get; set; }
        public int Size { get
            {
                if (Type == Token.IntToken) return GlobalConfiguration.IntegerSize;
                else if (Type == Token.FloatToken) return GlobalConfiguration.FloatSize;
                else if (Type == Token.CharToken) return GlobalConfiguration.CharacterSize;
                else return -1;
            }
        }
    }
}