using System.Linq;

namespace CMinusMinusCompiler
{
    /* Recursive descent parser to verify program 
    follows the C-- grammar.

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

    StatementList  -> e

    Return         -> e
    */

    public class Parser
    {
        // Private properties
        private LexicalAnalyzer LexicalAnaylzer { get; set; }
        private Symbol[] TypeTokens { get; } = {
            Symbol.IntToken, Symbol.FloatToken,
            Symbol.CharToken //, Symbol.VoidToken
        };

        // Constructor requires a lexical analyzer instance
        public Parser(LexicalAnalyzer lexicalAnalyzer)
        {
            LexicalAnaylzer = lexicalAnalyzer;
        }

        // Matches expected symbol to current symbol from
        // lexical analyzer
        public void MatchToken(Symbol expectedSymbol)
        {
            if (LexicalAnaylzer.Token == expectedSymbol)
            {
                LexicalAnaylzer.GetNextToken();
            }
            else
            {
                CommonTools.WriteOutput(
                    "ERROR: Line " + LexicalAnaylzer.LineNumber 
                    + " Expected token \"" + expectedSymbol 
                    + "\" - Received token \"" + LexicalAnaylzer.Token);
            }
        }
        // Program -> Type IdentifierToken Rest Program |
         //           e
        public void ProcessProgram()
        {
            // Here void token is making this next if skip, fix me
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
            else CommonTools.WriteOutput("TODO make me a better errors");
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
                ProcessCoumpound();
            }
            else
            {
                ProcessIdentifierTail();
                MatchToken(Symbol.SemiColonToken);
                ProcessProgram();
            }
        }

        // ParameterTail  -> , Type IdentifierToken ParameterTail |
        //                     e
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
        private void ProcessCoumpound()
        {
            MatchToken(Symbol.LeftBraceToken);
            ProcessDeclaration();
            ProcessStatementList();
            ProcessReturn();
            MatchToken(Symbol.RightBraceToken);
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

        // ParameterList  -> Type IdentifierToken ParameterTail |
        //                   e
        private void ProcessIdentifierList()
        {
            MatchToken(Symbol.IdentifierToken);
            ProcessIdentifierTail();
            MatchToken(Symbol.SemiColonToken);
            ProcessDeclaration();
        }

        // ParameterTail  -> , Type IdentifierToken ParameterTail |
        //                   e 
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
    }
}
