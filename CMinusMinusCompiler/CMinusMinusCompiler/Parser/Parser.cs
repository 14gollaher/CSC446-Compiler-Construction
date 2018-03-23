using System.Linq;

namespace CMinusMinusCompiler
{
    /* Recursive descent parser to verify program follows C-- grammar

    Program        -> Type IdentifierToken Rest Program |
                      ConstToken IdentifierToken AssignmentOperatorToken NumberToken SemiColonToken Program |
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
                      ConstToken IdentifierToken AssignmentOperatorToken NumberToken SemiColonToken Declaration |
                      e
    
    IdentifierTail -> , IdentifierToken IdentifierTail

    StatementList  -> e

    Return         -> e
    */
    public class Parser
    {
        // Private properties
        private LexicalAnalyzer LexicalAnaylzer { get; set; }
        private Token[] TypeTokens { get; } = {
            Token.IntToken, Token.FloatToken,
            Token.CharToken 
        };

        // Constructor requires a lexical analyzer instance
        public Parser(LexicalAnalyzer lexicalAnalyzer)
        {
            LexicalAnaylzer = lexicalAnalyzer;
        }

        // Matches expected symbol to current symbol from lexical analyzer
        public void MatchToken(Token expectedSymbol)
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
        //            ConstToken IdentifierToken AssignmentOperatorToken NumberToken SemiColonToken Program |
        //            e
        public void ProcessProgram()
        {
            if (TypeTokens.Contains(LexicalAnaylzer.Token))
            {
                ProcessType();
                MatchToken(Token.IdentifierToken);
                ProcessRest();
                ProcessProgram();
            }
            else if (LexicalAnaylzer.Token == Token.ConstToken)
            {
                MatchToken(Token.ConstToken);
                MatchToken(Token.IdentifierToken);
                MatchToken(Token.AssignmentOperatorToken);
                MatchToken(Token.NumberToken);
                MatchToken(Token.SemiColonToken);
                ProcessProgram();
            }
        }

        // Type -> IntToken | 
        //         FloatToken |
        //         CharToken
        private void ProcessType()
        {
            if (LexicalAnaylzer.Token == Token.IntToken) MatchToken(Token.IntToken);
            else if (LexicalAnaylzer.Token == Token.FloatToken) MatchToken(Token.FloatToken);
            else if (LexicalAnaylzer.Token == Token.CharToken) MatchToken(Token.CharToken);
            else DisplayExpectedTokensError("valid TYPE");
        }

        // Rest -> ( ParameterList ) Compound |
        //         IdentifierTail ; Program
        private void ProcessRest()
        {
            if (LexicalAnaylzer.Token == Token.LeftParenthesisToken)
            {
                MatchToken(Token.LeftParenthesisToken);
                ProcessParameterList();
                MatchToken(Token.RightParenthesisToken);
                ProcessCompound();
            }
            else
            {
                ProcessIdentifierTail();
                MatchToken(Token.SemiColonToken);
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
                MatchToken(Token.IdentifierToken);
                ProcessParameterTail();
            }
        }

        // ParameterTail -> , Type IdentifierToken ParameterTail |
        //                  e
        private void ProcessParameterTail()
        {
            if (LexicalAnaylzer.Token == Token.CommaToken)
            {
                MatchToken(Token.CommaToken);
                ProcessType();
                MatchToken(Token.IdentifierToken);
                ProcessParameterTail();
            }
        }

        // Compound -> { Declaration StatementList Return }
        private void ProcessCompound()
        {
            if (LexicalAnaylzer.Token == Token.LeftBraceToken)
            {
                MatchToken(Token.LeftBraceToken);
                ProcessDeclaration();
                ProcessStatementList();
                ProcessReturn();
                MatchToken(Token.RightBraceToken);
            }
            else
            {
                DisplayExpectedTokensError(Token.LeftBraceToken.ToString());
            }
        }

        // Declaration -> Type IdentifierList |
        //                ConstToken IdentifierToken AssignmentOperatorToken NumberToken SemiColonToken Declaration |
        //                e
        private void ProcessDeclaration()
        {
            if (TypeTokens.Contains(LexicalAnaylzer.Token))
            {
                ProcessType();
                ProcessIdentifierList();
            }
            else if (LexicalAnaylzer.Token == Token.ConstToken)
            {
                MatchToken(Token.ConstToken);
                MatchToken(Token.IdentifierToken);
                MatchToken(Token.AssignmentOperatorToken);
                MatchToken(Token.NumberToken);
                MatchToken(Token.SemiColonToken);
                ProcessDeclaration();
            }
        }

        // IdentifierList -> IdentifierToken IdentifierTail ; Declaration |
        //                   e
        private void ProcessIdentifierList()
        {
            if (LexicalAnaylzer.Token == Token.IdentifierToken)
            {
                MatchToken(Token.IdentifierToken);
                ProcessIdentifierTail();
                MatchToken(Token.SemiColonToken);
                ProcessDeclaration();
            }
        }

        // IdentifierTail -> , IdentifierToken IdentifierTail |
        //                     e
        private void ProcessIdentifierTail()
        {
            if (LexicalAnaylzer.Token == Token.CommaToken)
            {
                MatchToken(Token.CommaToken);
                MatchToken(Token.IdentifierToken);
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
            CommonTools.PromptProgramExit();
        }
    }
}
