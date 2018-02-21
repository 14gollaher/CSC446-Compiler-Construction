using System.Linq;

namespace CMinusMinusCompiler
{
    /* Recursive descent parser to verify program follows C-- grammar

    Program        -> Type IdentifierToken Rest Program |
                      e

    Type           -> IntToken |
                      FloatToken |
                      CharToken

    Rest           -> ( ParameterList ) Compound |
                      IdentifierTail ; Program

    ParameterList  -> Type IdentifierToken ParameterTail |
                      e

    ParameterTail  -> , Type IdentifierToken ParameterTail |
                      e

    Compound       -> { Declaration StatementList Return }

    Declaration    -> Type IdentifierList |
                      e

    IdentifierList -> IdentifierToken IdentifierTail ; Declaration
    
    IdentifierTail -> , IdentifierToken IdentifierTail

    StatementList  -> e

    Return         -> e
    */
    public class Parser
    {
        // Private properties
        private LexicalAnalyzer LexicalAnaylzer { get; set; }
        private Symbol[] TypeTokens { get; } = {
            Symbol.IntToken, Symbol.FloatToken,
            Symbol.CharToken 
        };

        // Constructor requires a lexical analyzer instance
        public Parser(LexicalAnalyzer lexicalAnalyzer)
        {
            LexicalAnaylzer = lexicalAnalyzer;
        }

        // Matches expected symbol to current symbol from lexical analyzer
        public void MatchToken(Symbol expectedSymbol)
        {
            if (LexicalAnaylzer.Token == expectedSymbol)
            {
                LexicalAnaylzer.GetNextToken();
            }
            else
            {
                DisplayExpectedTokensError(expectedSymbol.ToString());
            }
        }

        // Program -> Type IdentifierToken Rest Program |
        //            e
        public void ProcessProgram()
        {
            if (TypeTokens.Contains(LexicalAnaylzer.Token))
            {
                ProcessType();
                MatchToken(Symbol.IdentifierToken);
                ProcessRest();
                ProcessProgram();
            }
        }

        // Type -> IntToken | 
        //         FloatToken |
        //         CharToken
        private void ProcessType()
        {
            if (LexicalAnaylzer.Token == Symbol.IntToken) MatchToken(Symbol.IntToken);
            else if (LexicalAnaylzer.Token == Symbol.FloatToken) MatchToken(Symbol.FloatToken);
            else if (LexicalAnaylzer.Token == Symbol.CharToken) MatchToken(Symbol.CharToken);
            else DisplayExpectedTokensError("valid TYPE");
        }

        // Rest -> ( ParameterList ) Compound |
        //         IdentifierTail ; Program
        private void ProcessRest()
        {
            if (LexicalAnaylzer.Token == Symbol.LeftParenthesisToken)
            {
                MatchToken(Symbol.LeftParenthesisToken);
                ProcessParameterList();
                MatchToken(Symbol.RightParenthesisToken);
                ProcessCompound();
            }
            else
            {
                ProcessIdentifierTail();
                MatchToken(Symbol.SemiColonToken);
                ProcessProgram();
            }
        }

        // ParameterList -> , Type IdentifierToken ParameterTail |
        //                  e
        private void ProcessParameterList()
        {
            if (TypeTokens.Contains(LexicalAnaylzer.Token))
            {
                ProcessType();
                MatchToken(Symbol.IdentifierToken);
                ProcessParameterTail();
            }
        }

        // ParameterTail -> , Type IdentifierToken ParameterTail |
        //                  e
        private void ProcessParameterTail()
        {
            if (LexicalAnaylzer.Token == Symbol.CommaToken)
            {
                MatchToken(Symbol.CommaToken);
                ProcessType();
                MatchToken(Symbol.IdentifierToken);
                ProcessParameterTail();
            }
        }

        // Compound -> { Declaration StatementList Return }
        private void ProcessCompound()
        {
            if (LexicalAnaylzer.Token == Symbol.LeftBraceToken)
            {
                MatchToken(Symbol.LeftBraceToken);
                ProcessDeclaration();
                ProcessStatementList();
                ProcessReturn();
                MatchToken(Symbol.RightBraceToken);
            }
            else
            {
                DisplayExpectedTokensError(Symbol.LeftBraceToken.ToString());
            }
        }

        // Declaration -> Type IdentifierList |
        //                e
        private void ProcessDeclaration()
        {
            if (TypeTokens.Contains(LexicalAnaylzer.Token))
            {
                ProcessType();
                ProcessIdentifierList();
            }
        }

        // IdentifierList -> IdentifierToken IdentifierTail ; Declaration |
        //                   e
        private void ProcessIdentifierList()
        {
            if (LexicalAnaylzer.Token == Symbol.IdentifierToken)
            {
                MatchToken(Symbol.IdentifierToken);
                ProcessIdentifierTail();
                MatchToken(Symbol.SemiColonToken);
                ProcessDeclaration();
            }
        }

        // IdentifierTail -> , IdentifierToken IdentifierTail |
        //                     e
        private void ProcessIdentifierTail()
        {
            if (LexicalAnaylzer.Token == Symbol.CommaToken)
            {
                MatchToken(Symbol.CommaToken);
                MatchToken(Symbol.IdentifierToken);
                ProcessIdentifierTail();
            }
        }

        // StatementList -> e
        private void ProcessStatementList()
        {
            // Blank for now
        }

        // Return -> e
        private void ProcessReturn()
        {
            // Blank for now
        }

        // Displays expected tokens error to appropriate displays
        private void DisplayExpectedTokensError(string expectedToken)
        {
            CommonTools.WriteOutput(
                $"ERROR: Line {LexicalAnaylzer.LineNumber} " +
                $"Expected token \"{expectedToken}\" " +
                $"- Received token \"{LexicalAnaylzer.Token}\"");
        }
    }
}
