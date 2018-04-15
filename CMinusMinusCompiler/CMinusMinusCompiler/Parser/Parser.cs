using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace CMinusMinusCompiler
{
    /* Recursive descent parser to verify program follows C-- grammar

    Program                 -> Type IdentifierToken Rest Program |
                               ConstToken IdentifierToken AssignmentOperatorToken NumberToken SemiColonToken Program |
                               e
                            
    Type                    -> IntToken |
                               FloatToken |
                               CharToken
                            
    Rest                    -> ( ParameterList ) Compound |
                               IdentifierTail ; Program
                            
    ParameterList           -> Type IdentifierToken ParameterTail |
                               e
                            
    ParameterTail           -> , Type IdentifierToken ParameterTail |
                               e
                            
    Compound                -> { Declaration StatementList Return }
                            
    Declaration             -> Type IdentifierList |
                               ConstToken IdentifierToken 
                               Token NumberToken SemiColonToken Declaration |
                               e
                            
    IdentifierTail          -> , IdentifierToken IdentifierTail
                            
    StatementList           -> Statement ; StatementList |
                               e
                             
    Statement               -> AssignmentStatement |
                               InputOutputStatement 
                            
    AssignmentStatement     -> IdentifierToken AssignmentOperatorToken Expression
                            
    InputOutputStatement    -> e
                            
    Expression              -> Relation
                            
    Relation                -> SimpleExpression
                            
    SimpleExpression        -> SignOperation Term MoreTerm
                            
    MoreTerm                -> AdditionOperation Term MoreTerm |
                               e
                            
    Term                    -> Factor MoreFactor
                            
    MoreFactor              -> MultiplicationOperation Factor MoreFactor |
                               e
                            
    Factor                  -> IdentifierToken |
                               NumberToken |
                               LeftParenthesisToken Expression RightParenthesisToken
                            
    AdditionOperation       -> + | 
                               - | 
                               ||

    MultiplicationOperation -> * |
                               / |
                               % |
                               &&

    SignOperation           -> ! |
                               - |
                               e

    Return                  -> e
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

        public bool Start()
        {
            bool programCorrect = ProcessProgram();
            if (LexicalAnaylzer.Token != Token.EndOfFileToken && programCorrect)
            {
                CommonTools.PromptProgramErrorExit(
                    $"ERROR: Line {LexicalAnaylzer.LineNumber} " +
                    $"Unexpected tokens in source file, expected End-of-File Token");
            }

            return programCorrect;
        }

        // Program -> Type IdentifierToken Rest Program |
        //            ConstToken IdentifierToken AssignmentOperatorToken NumberToken SemiColonToken Program |
        //            e
        private bool ProcessProgram()
        {
            if (IsVariableType(LexicalAnaylzer.Token))
            {
                CurrentFunction.ReturnType = LexicalAnaylzer.Token;
                ProcessType();
                CurrentVariable.Lexeme = LexicalAnaylzer.Lexeme;
                CurrentFunction.Lexeme = LexicalAnaylzer.Lexeme;
                if (!MatchToken(Token.IdentifierToken)) return false;
                if (!ProcessRest()) return false;
                if (!ProcessProgram()) return false;
            }
            else if (LexicalAnaylzer.Token == Token.ConstToken)
            {
                LexicalAnaylzer.GetNextToken();
                CurrentConstant.Lexeme = LexicalAnaylzer.Lexeme;
                if (!MatchToken(Token.IdentifierToken)) return false;
                if (!MatchToken(Token.AssignmentOperatorToken)) return false;
                if (!InsertConstantNode()) return false;
                if (!MatchToken(Token.NumberToken)) return false;
                if (!MatchToken(Token.SemiColonToken)) return false;
                if (!ProcessProgram()) return false;
            }
            return true;
        }

        // Type -> IntToken | 
        //         FloatToken |
        //         CharToken
        private bool ProcessType()
        {
            CurrentVariable.Type = LexicalAnaylzer.Token;

            if (LexicalAnaylzer.Token == Token.IntToken)
            {
                LexicalAnaylzer.GetNextToken();
            }
            else if (LexicalAnaylzer.Token == Token.FloatToken)
            {
                LexicalAnaylzer.GetNextToken();
            }
            else if (LexicalAnaylzer.Token == Token.CharToken)
            {
                LexicalAnaylzer.GetNextToken();
            }
            else
            {
                DisplayExpectedTokensError("valid TYPE");
                return false;
            }
            return true;
        }

        // Rest -> ( ParameterList ) Compound |
        //         IdentifierTail ; Program
        private bool ProcessRest()
        {
            if (LexicalAnaylzer.Token == Token.LeftParenthesisToken)
            {
                CurrentFunction.Depth = Depth;
                IncreaseProgramStack();
                LexicalAnaylzer.GetNextToken();
                if (!ProcessParameterList()) return false;
                Offset = 0;
                if (!MatchToken(Token.RightParenthesisToken)) return false;
                if (!ProcessCompound()) return false;
            }
            else
            {
                CurrentVariableType = CurrentVariable.Type;
                if (!InsertVariableNode()) return false;
                if (!ProcessIdentifierTail()) return false;
                if (!MatchToken(Token.SemiColonToken)) return false;
                if (!ProcessProgram()) return false;
            }
            return true;
        }

        // ParameterList -> , Type IdentifierToken ParameterTail |
        //                  e
        private bool ProcessParameterList()
        {
            if (IsVariableType(LexicalAnaylzer.Token))
            {
                ProcessType();
                AddParameterNode();
                CurrentVariable.Lexeme = LexicalAnaylzer.Lexeme;
                if (!InsertVariableNode()) return false;
                if (!MatchToken(Token.IdentifierToken)) return false;
                if (!ProcessParameterTail()) return false;
            }
            return true;
        }

        // ParameterTail -> , Type IdentifierToken ParameterTail |
        //                  e
        private bool ProcessParameterTail()
        {
            if (LexicalAnaylzer.Token == Token.CommaToken)
            {
                LexicalAnaylzer.GetNextToken();
                if (!ProcessType()) return false;
                AddParameterNode();
                CurrentVariable.Lexeme = LexicalAnaylzer.Lexeme;
                if (!InsertVariableNode()) return false;
                if (!MatchToken(Token.IdentifierToken)) return false;
                if (!ProcessParameterTail()) return false;
            }
            return true;
        }

        // Compound -> { Declaration StatementList Return }
        private bool ProcessCompound()
        {
            if (!MatchToken(Token.LeftBraceToken)) return false;
            if (!ProcessDeclaration()) return false;
            if (!ProcessStatementList()) return false;
            if (!ProcessReturn()) return false;
            if (!MatchToken(Token.RightBraceToken)) return false;
            return true;
        }

        // Declaration -> Type IdentifierList |
        //                ConstToken IdentifierToken AssignmentOperatorToken NumberToken SemiColonToken Declaration |
        //                e
        private bool ProcessDeclaration()
        {
            if (IsVariableType(LexicalAnaylzer.Token))
            {
                ProcessType(); 
                if (!ProcessIdentifierList()) return false; 
            }
            else if (LexicalAnaylzer.Token == Token.ConstToken)
            {
                LexicalAnaylzer.GetNextToken();
                CurrentConstant.Lexeme = LexicalAnaylzer.Lexeme;
                if (!MatchToken(Token.IdentifierToken)) return false;
                if (!MatchToken(Token.AssignmentOperatorToken)) return false;
                if (!InsertConstantNode()) return false;
                if (!MatchToken(Token.NumberToken)) return false;
                if (!MatchToken(Token.SemiColonToken)) return false;
                if (!ProcessDeclaration()) return false;
            }
            return true;
        }

        // IdentifierList -> IdentifierToken IdentifierTail ; Declaration |
        //                   e
        private bool ProcessIdentifierList()
        {
            if (LexicalAnaylzer.Token == Token.IdentifierToken)
            {
                CurrentVariable.Lexeme = LexicalAnaylzer.Lexeme;
                CurrentFunction.VariablesSize += CurrentVariable.Size;
                if (!InsertVariableNode()) return false;
                LexicalAnaylzer.GetNextToken();
                if (!ProcessIdentifierTail()) return false;
                if (!MatchToken(Token.SemiColonToken)) return false;
                if (!ProcessDeclaration()) return false;
            }
            return true;
        }

        // IdentifierTail -> , IdentifierToken IdentifierTail |
        //                   e
        private bool ProcessIdentifierTail()
        {
            if (LexicalAnaylzer.Token == Token.CommaToken)
            {
                LexicalAnaylzer.GetNextToken();
                CurrentVariable.Lexeme = LexicalAnaylzer.Lexeme;
                if (!InsertVariableNode()) return false;
                if (!MatchToken(Token.IdentifierToken)) return false;
                if (!ProcessIdentifierTail()) return false;
            }
            return true;
        }

        // StatementList -> Statement ; StatementList |
        //                  e
        private bool ProcessStatementList()
        {
            if (LexicalAnaylzer.Token == Token.IdentifierToken) 
            {
                if (!ProcessStatement()) return false;
                if (!MatchToken(Token.SemiColonToken)) return false;
                if (!ProcessStatementList()) return false;
            }
            return true;
        }

        // Statement -> AssignmentStatement |
        //              InputOutputStatement 
        private bool ProcessStatement()
        {
            if (LexicalAnaylzer.Token == Token.IdentifierToken)
            {
                if (!ProcessAssignmentStatement()) return false;
            }
            else //TODO: This probably won't be an else when IOStat is defined
            {
                if (!ProcessInputOutputStatement()) return false; 
            }
            return true;
        }

        // AssignmentStatement -> IdentifierToken AssignmentOperatorToken Expression
        //                        IdentifierToken AssignmentOperatorToken FunctionCall
        private bool ProcessAssignmentStatement()
        {
            if (!CheckDeclaredVariable()) return false;
            if (!MatchToken(Token.IdentifierToken)) return false;
            if (!MatchToken(Token.AssignmentOperatorToken)) return false;

            if (LexicalAnaylzer.Token == Token.IdentifierToken && LexicalAnaylzer.LookNextCharacter() == '(')
            {
                if (!ProcessFunctionCall()) return false;
            }
            else
            {
                if (!ProcessExpression()) return false;
            }

            //if (LexicalAnaylzer.Token == Token.IdentifierToken)
            //{
            //    LexicalAnalyzer currentLexicalAnalyzer = (LexicalAnalyzer)LexicalAnaylzer.Clone();
            //    if (!ProcessFunctionCall())
            //    {
            //        LexicalAnaylzer = currentLexicalAnalyzer;
            //        if (!ProcessExpression()) return false;
            //    }
            //}
            //else
            //{
            //    if (!ProcessExpression()) return false;
            //}
            return true;
        }

        // FunctionCall -> IdentifierToken LeftParenthesisToken Parameters RightParenthesisToken
        private bool ProcessFunctionCall()
        {
            if (!CheckDeclaredVariable()) return false;
            if (!MatchToken(Token.IdentifierToken)) return false;
            if (!MatchToken(Token.LeftParenthesisToken)) return false;
            if (!ProcessParameters()) return false;
            if (!MatchToken(Token.RightParenthesisToken)) return false;

            return true;
        }

        // InputOutputStatement -> e
        private bool ProcessInputOutputStatement()
        {
            // Blank for now
            return true;
        }

        // Expression -> Relation
        private bool ProcessExpression()
        {
            if (!ProcessRelation()) return false;
            return true;
        }

        // Relation -> SimpleExpression
        private bool ProcessRelation()
        {
            if (!ProcessSimpleExpression()) return false;
            return true;
        }

        // SimpleExpression -> SignOperation Term MoreTerm
        private bool ProcessSimpleExpression()
        {
            if (!ProcessSignOperation()) return false;
            if (!ProcessTerm()) return false;
            if (!ProcessMoreTerm()) return false;
            return true;
        }

        // MoreTerm -> AdditionOperation Term MoreTerm |
        //             e
        private bool ProcessMoreTerm()
        {
            if (LexicalAnaylzer.Token == Token.AdditionOperatorToken)
            {
                if (!ProcessAdditionOperation()) return false;
                if (!ProcessTerm()) return false;
                if (!ProcessMoreTerm()) return false;
            }
            return true;
        }

        // Term -> Factor MoreFactor
        private bool ProcessTerm()
        {
            if (!ProcessFactor()) return false;
            if (!ProcessMoreFactor()) return false;
            return true;
        }

        // MoreFactor -> MultiplicationOperation Factor MoreFactor |
        //               e
        private bool ProcessMoreFactor()
        {
            if (LexicalAnaylzer.Token == Token.MultiplicationOperatorToken)
            {
                if (!ProcessMultiplicationOperation()) return false;
                if (!ProcessFactor()) return false;
                if (!ProcessMoreFactor()) return false;
            }
            return true;
        }

        // Factor -> IdentifierToken |
        //           NumberToken |
        //           LeftParenthesisToken Expression RightParenthesisToken
        private bool ProcessFactor()
        {
            if (LexicalAnaylzer.Token == Token.IdentifierToken)
            {
                if (!CheckDeclaredVariable()) return false;
                if (!MatchToken(Token.IdentifierToken)) return false;
            }
            else if (LexicalAnaylzer.Token == Token.NumberToken)
            {
                if (!MatchToken(Token.NumberToken)) return false;
            }
            else
            {
                if (!MatchToken(Token.LeftParenthesisToken)) return false;
                if (!ProcessExpression()) return false;
                if (!MatchToken(Token.RightParenthesisToken)) return false;
            }
            return true;
        }

        // AdditionOperation -> + | 
        //                      - | 
        //                      ||
        private bool ProcessAdditionOperation()
        {
            if (!MatchToken(Token.AdditionOperatorToken)) return false;
            return true;
        }

        // MultiplicationOperation -> * |
        //                            / |
        //                            % |
        //                            &&
        private bool ProcessMultiplicationOperation()
        {
            if (!MatchToken(Token.MultiplicationOperatorToken)) return false;
            return true;
        }

        // SignOperation -> ! |
        //                  - |
        //                  e
        private bool ProcessSignOperation()
        {
            if (LexicalAnaylzer.Token == Token.NotOperatorToken)
            {
                LexicalAnaylzer.GetNextToken();
            }
            else if (LexicalAnaylzer.Token == Token.AdditionOperatorToken && LexicalAnaylzer.Lexeme == "-")
            {
                LexicalAnaylzer.GetNextToken();
            }
            return true;
        }

        // Return -> ReturnToken Expression SemiColonToken
        private bool ProcessReturn()
        {
            if (!MatchToken(Token.ReturnToken)) return false;
            if (!ProcessExpression()) return false;
            if (!MatchToken(Token.SemiColonToken)) return false;
            if (!InsertFunctionNode()) return false;
            DecreaseProgramStack();
            return true;
        }

        // Parameters -> IdentifierToken ParametersTail |
        //              NumberToken ParametersTail |
        //              e
        private bool ProcessParameters()
        {
            if (LexicalAnaylzer.Token == Token.IdentifierToken)
            {
                LexicalAnaylzer.GetNextToken();
                if (!ProcessParametersTail()) return false;
            }
            else if (LexicalAnaylzer.Token == Token.NumberToken)
            {
                LexicalAnaylzer.GetNextToken();
                if (!ProcessParametersTail()) return false;
            }
            return true;
        }

        // ParametersTail -> CommaToken IdentifierToken ParametersTail |
        //                   CommaToken NumberToken ParametersTail |
        //                   e
        private bool ProcessParametersTail()
        {
            if (LexicalAnaylzer.Token == Token.CommaToken)
            {
                LexicalAnaylzer.GetNextToken();
                if (LexicalAnaylzer.Token == Token.IdentifierToken)
                {
                    LexicalAnaylzer.GetNextToken();
                    if (!ProcessParametersTail()) return false;
                }
  
                if (!MatchToken(Token.NumberToken)) return false;
                if (!ProcessParametersTail()) return false;
            }
            return true;
        }

        // Inserts a variable node into symbol table
        private bool InsertVariableNode()
        {
            if (CurrentVariable.Size == -1) CurrentVariable.Type = CurrentVariableType;
            CurrentVariable.Offset = Offset;
            CurrentVariable.Depth = Depth;
            if (!InsertNode(CurrentVariable)) return false;
            Offset += CurrentVariable.Size;
            CurrentVariable = new VariableNode();
            return true;
        }

        // Inserts a constant node into symbol table
        private bool InsertConstantNode()
        {
            CurrentConstant.SetValues(LexicalAnaylzer.Value, LexicalAnaylzer.ValueReal);
            CurrentConstant.Depth = Depth;
            if (!InsertNode(CurrentConstant)) return false;
            CurrentConstant = new ConstantNode();
            return true;
        }

        // Inserts a function node into symbol table
        private bool InsertFunctionNode()
        {
            if (!InsertNode(CurrentFunction)) return false;
            CurrentFunction = new FunctionNode();
            return true;
        }

        // Inserts a node into symbol table and prints error if insertion failure
        private bool InsertNode(Node node)
        {
            if (!SymbolTable.InsertNode(node))
            {
                CommonTools.PromptProgramErrorExit($"ERROR: Line {LexicalAnaylzer.LineNumber} " +
                    $"Duplicate lexeme \"{node.Lexeme}\" with depth \"{node.Depth}\" exists");
                return false;
            }
            return true;
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

        // Increments global program stack information
        private void IncreaseProgramStack()
        {
            Depth++;
            Offsets.Push(Offset);
            Offset = 0;
        }

        // Decrements global program stack information
        private void DecreaseProgramStack()
        {
            SymbolTable.OutputSymbolTable(Depth);
            SymbolTable.DeleteDepth(Depth);
            Depth--;
            Offset = Offsets.Pop();
        }

        // Checks if an identifier exists as a variable node in symbol table and is in scope
        private bool CheckDeclaredVariable()
        {
            if (SymbolTable.LookupNode(LexicalAnaylzer.Lexeme) == null)
            {
                CommonTools.PromptProgramErrorExit($"ERROR: Line {LexicalAnaylzer.LineNumber} Use of " +
                    $"undeclared variable {LexicalAnaylzer.Lexeme}");
                return false;
            }
            return true;
        }

        // Displays expected tokens error to appropriate displays
        private void DisplayExpectedTokensError(string expectedToken)
        {
            CommonTools.PromptProgramErrorExit($"ERROR: Line {LexicalAnaylzer.LineNumber} Expected token " +
                $"\"{expectedToken}\" - Received token \"{LexicalAnaylzer.Token}\"");
        }

        // Matches expected symbol to current symbol from lexical analyzer
        private bool MatchToken(Token expectedSymbol)
        {
            if (LexicalAnaylzer.Token == expectedSymbol)
            {
                LexicalAnaylzer.GetNextToken();
                return true;
            }
            
            DisplayExpectedTokensError(expectedSymbol.ToString());
            return false;
        }
    }
}