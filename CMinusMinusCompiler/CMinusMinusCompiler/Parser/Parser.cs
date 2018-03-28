using System;
using System.Collections.Generic;
using System.Configuration;

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
        // Public properties
        public static int CharacterSize { get; } = Int32.Parse(ConfigurationManager.AppSettings["CharacterSize"]);
        public static int IntegerSize { get; } = Int32.Parse(ConfigurationManager.AppSettings["IntegerSize"]);
        public static int FloatSize { get; } = Int32.Parse(ConfigurationManager.AppSettings["FloatSize"]);

        // Private properties
        private LexicalAnalyzer LexicalAnaylzer { get; set; }
        private SymbolTable SymbolTable { get; set; }
        private int Depth { get; set; } = 1;
        private int Offset { get; set; }
        private Stack<int> Offsets = new Stack<int>();
        private FunctionNode CurrentFunction { get; set; } = new FunctionNode();
        private ConstantNode CurrentConstant { get; set; } = new ConstantNode();
        private VariableNode CurrentVariable { get; set; } = new VariableNode();
        private Token CurrentVariableType { get; set; }

        // Parameterized constructor requires a lexical analyzer and symbol table
        public Parser(LexicalAnalyzer lexicalAnalyzer, SymbolTable symbolTable)
        {
            LexicalAnaylzer = lexicalAnalyzer;
            SymbolTable = symbolTable;
        }

        // Program -> Type IdentifierToken Rest Program |
        //            ConstToken IdentifierToken AssignmentOperatorToken NumberToken SemiColonToken Program |
        //            e
        public void ProcessProgram()
        {
            if (IsVariableType(LexicalAnaylzer.Token))
            {
                CurrentFunction.ReturnType = LexicalAnaylzer.Token;
                ProcessType();
                CurrentVariable.Lexeme = LexicalAnaylzer.Lexeme;
                CurrentFunction.Lexeme = LexicalAnaylzer.Lexeme;
                MatchToken(Token.IdentifierToken);
                ProcessRest();
                ProcessProgram();
            }
            else if (LexicalAnaylzer.Token == Token.ConstToken)
            {
                MatchToken(Token.ConstToken);
                CurrentConstant.Lexeme = LexicalAnaylzer.Lexeme;
                MatchToken(Token.IdentifierToken);
                MatchToken(Token.AssignmentOperatorToken);
                InsertConstantNode();
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
            CurrentVariable.Type = LexicalAnaylzer.Token;

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
                CurrentFunction.Depth = Depth;
                IncreaseProgramStack();
                MatchToken(Token.LeftParenthesisToken);
                ProcessParameterList();
                Offset = 0;
                MatchToken(Token.RightParenthesisToken);
                ProcessCompound();
            }
            else
            {
                CurrentVariableType = CurrentVariable.Type;
                InsertVariableNode();
                ProcessIdentifierTail();
                MatchToken(Token.SemiColonToken);
                ProcessProgram();
            }
        }

        // ParameterList -> , Type IdentifierToken ParameterTail |
        //                  e
        private void ProcessParameterList()
        {
            if (IsVariableType(LexicalAnaylzer.Token))
            {
                ProcessType();
                AddParameterNode();
                CurrentVariable.Lexeme = LexicalAnaylzer.Lexeme;
                InsertVariableNode();
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
                AddParameterNode();
                CurrentVariable.Lexeme = LexicalAnaylzer.Lexeme;
                InsertVariableNode();
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
            if (IsVariableType(LexicalAnaylzer.Token))
            {
                ProcessType();
                ProcessIdentifierList();
            }
            else if (LexicalAnaylzer.Token == Token.ConstToken)
            {
                MatchToken(Token.ConstToken);
                CurrentConstant.Lexeme = LexicalAnaylzer.Lexeme;
                MatchToken(Token.IdentifierToken);
                MatchToken(Token.AssignmentOperatorToken);
                InsertConstantNode();
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
                CurrentVariable.Lexeme = LexicalAnaylzer.Lexeme;
                CurrentFunction.VariablesSize += CurrentVariable.Size;
                InsertVariableNode();
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
                CurrentVariable.Lexeme = LexicalAnaylzer.Lexeme;
                InsertVariableNode();
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
            // No grammar rules for now
            InsertFunctionNode();
            DecreaseProgramStack();
        }

        // Inserts a variable node into symbol table
        private void InsertVariableNode()
        {
            if (CurrentVariable.Size == -1) CurrentVariable.Type = CurrentVariableType;
            CurrentVariable.Offset = Offset;
            CurrentVariable.Depth = Depth;
            SymbolTable.InsertNode(CurrentVariable);
            Offset += CurrentVariable.Size;
            CurrentVariable = new VariableNode();
        }

        // Inserts a constant node into symbol table
        private void InsertConstantNode()
        {
            CurrentConstant.SetValues(LexicalAnaylzer.Value, LexicalAnaylzer.ValueReal);
            CurrentConstant.Depth = Depth;
            SymbolTable.InsertNode(CurrentConstant);
            CurrentConstant = new ConstantNode();
        }

        // Inserts a function node into symbol table
        private void InsertFunctionNode()
        {
            SymbolTable.InsertNode(CurrentFunction);
            CurrentFunction = new FunctionNode();
        }

        // Adds a parameter node into current function node
        private void AddParameterNode()
        {
            ParameterNode parameter = new ParameterNode()
            {
                Type = CurrentVariable.Type
            };
            CurrentFunction.VariablesSize += CurrentVariable.Size;
            CurrentFunction.Parameters.Add(parameter);
        }

        // Checks that token is a valid variable type
        private bool IsVariableType(Token token)
        {
            if (token == Token.IntToken || token == Token.FloatToken || token == Token.CharToken)
            {
                return true;
            }
            return false;
        }

        private void IncreaseProgramStack()
        {
            Depth++;
            Offsets.Push(Offset);
            Offset = 0;
        }

        private void DecreaseProgramStack()
        {
            SymbolTable.OutputSymbolTable(Depth);
            SymbolTable.DeleteDepth(Depth);
            Depth--;
            Offset = Offsets.Pop();
        }

        // Displays expected tokens error to appropriate displays
        private void DisplayExpectedTokensError(string expectedToken)
        {
            CommonTools.WriteOutput( $"ERROR: Line {LexicalAnaylzer.LineNumber} Expected token " +
                $"\"{expectedToken}\" -  Received token \"{LexicalAnaylzer.Token}\"");
            CommonTools.PromptProgramExit();
        }

        // Matches expected symbol to current symbol from lexical analyzer
        private void MatchToken(Token expectedSymbol)
        {
            if (LexicalAnaylzer.Token == expectedSymbol) LexicalAnaylzer.GetNextToken();
            else DisplayExpectedTokensError(expectedSymbol.ToString());

        }
    }
}