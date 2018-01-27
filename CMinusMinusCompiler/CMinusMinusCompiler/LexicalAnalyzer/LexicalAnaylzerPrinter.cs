using System;
using static CMinusMinusCompiler.LexicalAnalyzer;

namespace CMinusMinusCompiler
{
    public static class LexicalAnaylzerPrinter
    {
        private static string OutputFormat { get; } = "{0,-38} {1,-30} {2}";

        // Display next token to screen and output file
        public static void DisplayCurrentToken()
        {
            if (Token != Symbol.CommentToken && Token != Symbol.EndOfFileToken)
            {
                dynamic attribute = Value ?? ValueReal;
                attribute = attribute ?? Literal;

                string[] outputData = new string[] { Lexeme, Token.ToString(), attribute.ToString() };
                CommonTools.WriteOutput(string.Format(OutputFormat, outputData));
            }
        }

        // Display token header to screen and output file
        public static void DisplayTokenHeader()
        {
            string[] headingData = new string[] { "Lexeme", "Token", "Attribute" };
            string headerRule = Environment.NewLine + new string('-', 79);
            CommonTools.WriteOutput(string.Format(OutputFormat, headingData) + headerRule);
        }


    }
}
