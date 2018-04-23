using System;
using System.Collections.Generic;

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
                               ConstToken IdentifierToken AssignmentOperatorToken NumberToken SemiColonToken Declaration |
                               e

    IdentifierList          -> IdentifierToken IdentifierTail ; Declaration |
                               e
                            
    IdentifierTail          -> , IdentifierToken IdentifierTail
                            
    StatementList           -> Statement ; StatementList |
                               e
                             
    Statement               -> AssignmentStatement |
                               InputOutputStatement 
                            
    AssignmentStatement     -> IdentifierToken AssignmentOperatorToken Expression |
                               IdentifierToken AssignmentOperatorToken FunctionCall
                            
    InputOutputStatement    -> InputStatement | OutputStatement

    InputStatement          -> CharacterInToken RightShiftOperatorToken IdentifierToken InputEnd

    InputEnd                -> RightShiftOperatorToken IdentifierToken InputEnd | 
                               e
    
    OutputStatement         -> CharacterOutToken LeftShiftOperatorToken OutputOptions OutputEnd

    OutputOptions           -> IdentifierToken | StringLiteralToken | EndLineToken

    OutputEnd               -> LeftShiftOperatorToken OutputOptions OutputEnd |
                               e
                            
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
    
    FunctionCall            -> IdentifierToken LeftParenthesisToken Parameters RightParenthesisToken

    Parameters              -> IdentifierToken ParametersTail |
                               NumberToken ParametersTail |
                               e
    
    ParametersTail          -> , IdentiferToken ParametersTail |
                               , NumberToken ParametersTail |
                               e
    
    Return                  -> ReturnToken Expression SemiColonToken

    */
    public class Parser
    {
        // Private properties
        private LexicalAnalyzer LexicalAnaylzer { get; set; }
        private SymbolTable SymbolTable { get; set; }
        private FunctionNode CurrentFunction { get; set; } = new FunctionNode();
        private ConstantNode CurrentConstant { get; set; } = new ConstantNode();
        private VariableNode CurrentVariable { get; set; } = new VariableNode();
        private int Depth { get; set; } = GlobalConfiguration.BaseDepth;
        private int ParameterOffset { get; set; }
        private Stack<int> ParameterOffsets { get; set; } = new Stack<int>();
        private int LocalOffset { get; set; } = GlobalConfiguration.BaseLocalOffset;
        private Stack<int> LocalOffsets { get; set; } = new Stack<int>();
        private Token CurrentVariableType { get; set; }
        private Stack<string> ParameterStack { get; set; } = new Stack<string>();
        private string CurrentFunctionCall { get; set; }
        private string SignOperation { get; set; }
        private List<string> SignOperations { get; } = new List<string>() { "-", "!" };
        private string ThreeAddressCodeOutput { get; set; } = string.Empty;

        // Parameterized constructor requires a lexical analyzer and symbol table
        public Parser(LexicalAnalyzer lexicalAnalyzer, SymbolTable symbolTable)
        {
            LexicalAnaylzer = lexicalAnalyzer;
            SymbolTable = symbolTable;
        }

        public bool Start()
        {
            bool programCorrect = ProcessProgram() && CheckMainDeclared();

            if (LexicalAnaylzer.Token != Token.EndOfFileToken && programCorrect)
            {
                CommonTools.PromptProgramErrorExit(
                    $"ERROR: Line {LexicalAnaylzer.LineNumber} " +
                    $"Unexpected tokens in source file, expected End-of-File Token");
            }

            if ((CommonTools.ThreeAddressCodeRun) && programCorrect)
            {
                CommonTools.WriteOutput(ThreeAddressCodeOutput);
            }
            return programCorrect;
        }

        private bool CheckMainDeclared()
        {
            if (!(SymbolTable.LookupNode("main") is FunctionNode))
            {
                CommonTools.WriteOutput("ERROR: No required function \"main\" exists");
                return false;
            }

            OutputThreeAddressCode("START PROC main");
            return true;
        }

        // Program -> Type IdentifierToken Rest Program |
        //            ConstToken IdentifierToken AssignmentOperatorToken NumberToken SemiColonToken Program |
        //            e
        private bool ProcessProgram()
        {
            if (IsVariableType(LexicalAnaylzer.Token))
            {
                CurrentFunction.ReturnType = LexicalAnaylzer.Token;
                if (!ProcessType()) return false;
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
                OutputThreeAddressCode($"PROC {CurrentFunction.Lexeme}");
                CurrentFunction.Depth = Depth;
                IncreaseProgramStack();
                LexicalAnaylzer.GetNextToken();
                if (!ProcessParameterList()) return false;
                LocalOffset = GlobalConfiguration.BaseLocalOffset;
                if (!MatchToken(Token.RightParenthesisToken)) return false;
                if (!ProcessCompound()) return false;
            }
            else
            {
                CurrentVariableType = CurrentVariable.Type;
                if (!InsertVariableNode(true)) return false;
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
                if (!ProcessType()) return false;
                AddParameterNode();
                CurrentVariable.Lexeme = LexicalAnaylzer.Lexeme;
                if (!InsertVariableNode(false)) return false;
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
                if (!InsertVariableNode(false)) return false;
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
                CurrentFunction.LocalsSize += CurrentVariable.Size;
                CurrentVariableType = CurrentVariable.Type;
                if (!InsertVariableNode(true)) return false;
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
                if (!InsertVariableNode(true)) return false;
                if (!MatchToken(Token.IdentifierToken)) return false;
                if (!ProcessIdentifierTail()) return false;
            }
            return true;
        }

        // StatementList -> Statement ; StatementList |
        //                  e
        private bool ProcessStatementList()
        {
            if (LexicalAnaylzer.Token == Token.IdentifierToken 
                || LexicalAnaylzer.Token == Token.CharacterInToken
                || LexicalAnaylzer.Token == Token.CharacterOutToken)
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
            else
            {
                if (!ProcessInputOutputStatement()) return false;
            }
            return true;
        }

        // InputOutputStatement -> InputStatement | OutputStatement
        private bool ProcessInputOutputStatement()
        {
            if (LexicalAnaylzer.Token == Token.CharacterInToken)
            {
                if (!ProcessInputStatement()) return false;
            }
            else
            {
                if (!ProcessOutputStatement()) return false;
            }
            return true;
        }

        // InputStatement -> CharacterInToken RightShiftOperatorToken IdentifierToken InputEnd
        private bool ProcessInputStatement()
        {
            if (!MatchToken(Token.CharacterInToken)) return false;
            if (!MatchToken(Token.RightShiftOperatorToken)) return false;

            VariableNode node = (VariableNode)SymbolTable.LookupNode(LexicalAnaylzer.Lexeme);
            if (node.Type == Token.CharToken) OutputThreeAddressCode($"\t_READCHAR {GetThreeAddressCodeName(LexicalAnaylzer.Lexeme)}");
            else if (node.Type == Token.IntToken) OutputThreeAddressCode($"\t_READINT {GetThreeAddressCodeName(LexicalAnaylzer.Lexeme)}");
            else OutputThreeAddressCode($"\t_readFloat {GetThreeAddressCodeName(LexicalAnaylzer.Lexeme)}");

            if (!MatchToken(Token.IdentifierToken)) return false;
            if (!ProcessInputEnd()) return false;
            return true;
        }

        // InputEnd -> RightShiftOperatorToken IdentifierToken InputEnd | 
        //             e
        private bool ProcessInputEnd()
        {
            if (LexicalAnaylzer.Token == Token.RightShiftOperatorToken)
            {
                LexicalAnaylzer.GetNextToken();
                if (!MatchToken(Token.RightShiftOperatorToken)) return false;
                if (!MatchToken(Token.IdentifierToken)) return false;
                if (!ProcessInputEnd()) return false;
            }
            return true;
        }

        // OutputStatement -> CharacterOutToken LeftShiftOperatorToken OutputOptions OutputEnd
        private bool ProcessOutputStatement()
        {
            if (!MatchToken(Token.CharacterOutToken)) return false;
            if (!MatchToken(Token.LeftShiftOperatorToken)) return false;
            if (!ProcessOutputOptions()) return false;
            if (!ProcessOutputEnd()) return false;
            return true;
        }

        // OutputOptions -> IdentifierToken | StringLiteralToken | EndLineToken
        private bool ProcessOutputOptions()
        {
            if (LexicalAnaylzer.Token == Token.IdentifierToken)
            {
                VariableNode node = (VariableNode) SymbolTable.LookupNode(LexicalAnaylzer.Lexeme);
                if (node.Type == Token.CharToken) OutputThreeAddressCode($"\t_WRITECHAR {GetThreeAddressCodeName(LexicalAnaylzer.Lexeme)}");
                else if (node.Type == Token.IntToken) OutputThreeAddressCode($"\t_WRITEINT {GetThreeAddressCodeName(LexicalAnaylzer.Lexeme)}");
                else OutputThreeAddressCode($"\t_writeFloat {GetThreeAddressCodeName(LexicalAnaylzer.Lexeme)}");
                LexicalAnaylzer.GetNextToken();
            }
            else if (LexicalAnaylzer.Token == Token.StringLiteralToken)
            {
                StringLiteralNode node = new StringLiteralNode() {
                    Lexeme = $"_S{SymbolTable.StringLiteralCount}",
                    Literal = LexicalAnaylzer.Lexeme
                };
                SymbolTable.InsertNode(node);
                OutputThreeAddressCode($"\t_WRITESTRINGLITERAL {node.Lexeme}");
                LexicalAnaylzer.GetNextToken();
            }
            else
            {
                OutputThreeAddressCode($"\t_WRITENEWLINE");
                if (!MatchToken(Token.EndLineToken)) return false;
            }
            return true;
        }


        // OutputEnd -> LeftShiftOperatorToken OutputOptions OutputEnd |
        //              e
        private bool ProcessOutputEnd()
        {
            if (LexicalAnaylzer.Token == Token.LeftShiftOperatorToken)
            {
                LexicalAnaylzer.GetNextToken();
                if (!ProcessOutputOptions()) return false;
                if (!ProcessOutputEnd()) return false;
            }
            return true;
        }

        // AssignmentStatement -> IdentifierToken AssignmentOperatorToken Expression
        //                        IdentifierToken AssignmentOperatorToken FunctionCall
        private bool ProcessAssignmentStatement()
        {
            if (!CheckDeclaredVariable()) return false;
            string lexeme = LexicalAnaylzer.Lexeme;

            if (!MatchToken(Token.IdentifierToken)) return false;
            if (!MatchToken(Token.AssignmentOperatorToken)) return false;

            if (LexicalAnaylzer.Token == Token.IdentifierToken && LexicalAnaylzer.LookNextCharacter() == '(')
            { 
                CurrentFunctionCall = LexicalAnaylzer.Lexeme;
                if (!ProcessFunctionCall()) return false;
                if (!CheckValidVariableAssignment(lexeme)) return false;
                OutputThreeAddressCode($"\t_CALL {CurrentFunctionCall}");
                OutputThreeAddressCode($"\t{GetThreeAddressCodeName(lexeme)} = _AX");
            }
            else
            {
                Node node = new Node();
                if (!ProcessExpression(ref node)) return false;
                if (!CheckValidVariableAssignment(lexeme)) return false;
                OutputThreeAddressCode($"\t{GetThreeAddressCodeName(lexeme)} = {GetThreeAddressCodeName(node.Lexeme)}");
            }
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
            PrintThreeAddressCodeParameterStack();
            return true;
        }

        // Expression -> Relation
        private bool ProcessExpression(ref Node expressionNode)
        {
            if (!ProcessRelation(ref expressionNode)) return false;
            return true;
        }

        // Relation -> SimpleExpression
        private bool ProcessRelation(ref Node relationNode)
        {
            if (!ProcessSimpleExpression(ref relationNode)) return false;
            return true;
        }

        // SimpleExpression -> SignOperation Term MoreTerm
        private bool ProcessSimpleExpression(ref Node simpleExpressionNode)
        {
            Node termNode = new Node();
            if (!ProcessSignOperation()) return false;
            if (!ProcessTerm(ref termNode)) return false;
            if (!ProcessMoreTerm(ref termNode)) return false;
            simpleExpressionNode = termNode;
            return true;
        }

        // Term -> Factor MoreFactor
        private bool ProcessTerm(ref Node termNode)
        {
            Node factorNode = new Node();
            if (!ProcessFactor(ref factorNode)) return false;
            if (!ProcessMoreFactor(ref factorNode)) return false;
            termNode = factorNode;
            return true;
        }

        // MoreTerm -> AdditionOperation Term MoreTerm |
        //             e
        private bool ProcessMoreTerm(ref Node moreTermNode)
        {
            if (LexicalAnaylzer.Token == Token.AdditionOperatorToken)
            {
                string temporaryVariableName = GetTemporaryVariableName(Token.IntToken); //TODO: Don't hardcode me to IntToken
                string threeAddressCodeOutput = $"\t{temporaryVariableName} = {GetThreeAddressCodeName(moreTermNode.Lexeme)} {LexicalAnaylzer.Lexeme}";

                if (!ProcessAdditionOperation()) return false;

                Node newMoreTermMode = new Node();
                if (!ProcessTerm(ref newMoreTermMode)) return false;
                OutputThreeAddressCode($"{threeAddressCodeOutput} {GetThreeAddressCodeName(newMoreTermMode.Lexeme)}");

                moreTermNode.Lexeme = temporaryVariableName;
                if (!ProcessMoreTerm(ref moreTermNode)) return false;
            }
            return true;
        }

        // Factor -> IdentifierToken |
        //           NumberToken |
        //           LeftParenthesisToken Expression RightParenthesisToken
        private bool ProcessFactor(ref Node factorNode)
        {
            if (LexicalAnaylzer.Token == Token.IdentifierToken)
            {
                if (!CheckDeclaredVariable()) return false;

                Node lookupfactorNode = SymbolTable.LookupNode(LexicalAnaylzer.Lexeme);
                if (lookupfactorNode is ConstantNode)
                {
                    factorNode.Lexeme = GetThreeAddressCodeName(lookupfactorNode.Lexeme);
                }
                else
                {
                    factorNode.Lexeme = SignOperation + lookupfactorNode.Lexeme;
                }
                SignOperation = string.Empty;
                LexicalAnaylzer.GetNextToken();
            }
            else if (LexicalAnaylzer.Token == Token.NumberToken)
            {
                
                factorNode.Lexeme = GetThreeAddressCodeName(SignOperation + LexicalAnaylzer.Lexeme);
                SignOperation = string.Empty;
                LexicalAnaylzer.GetNextToken();
            }
            else
            {
                if (!MatchToken(Token.LeftParenthesisToken)) return false;
                if (!ProcessExpression(ref factorNode)) return false;
                if (!MatchToken(Token.RightParenthesisToken)) return false;
            }

            return true;
        }

        // MoreFactor -> MultiplicationOperation Factor MoreFactor |
        //               e
        private bool ProcessMoreFactor(ref Node factorNode)
        {
            if (LexicalAnaylzer.Token == Token.MultiplicationOperatorToken)
            {
                string temporaryVariableName = GetTemporaryVariableName(Token.IntToken); //TODO: Don't hardcode me to IntToken
                string threeAddressCodeOutput = $"\t{temporaryVariableName} = {GetThreeAddressCodeName(factorNode.Lexeme)} {LexicalAnaylzer.Lexeme}";

                if (!ProcessMultiplicationOperation()) return false;

                Node moreFactorNode = new Node();
                if (!ProcessFactor(ref moreFactorNode)) return false;
                OutputThreeAddressCode($"{threeAddressCodeOutput} {GetThreeAddressCodeName(moreFactorNode.Lexeme)}");

                factorNode.Lexeme = temporaryVariableName;
                if (!ProcessMoreFactor(ref moreFactorNode)) return false;
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
                SignOperation = LexicalAnaylzer.Lexeme;
                LexicalAnaylzer.GetNextToken();
            }
            else if (LexicalAnaylzer.Token == Token.AdditionOperatorToken && LexicalAnaylzer.Lexeme == "-")
            {
                SignOperation = LexicalAnaylzer.Lexeme;
                LexicalAnaylzer.GetNextToken();
            }
            return true;
        }

        // Return -> ReturnToken Expression SemiColonToken
        private bool ProcessReturn()
        {
            if (!MatchToken(Token.ReturnToken)) return false;

            Node expressionNode = new Node();
            if (!ProcessExpression(ref expressionNode)) return false; 
            OutputThreeAddressCode($"\t_AX = {GetThreeAddressCodeName(expressionNode.Lexeme)}");

            if (!MatchToken(Token.SemiColonToken)) return false;
            DecreaseProgramStack();
            if (!InsertFunctionNode()) return false;
            return true;
        }

        // Parameters -> IdentifierToken ParametersTail |
        //               NumberToken ParametersTail |
        //               e
        private bool ProcessParameters()
        {
            if (LexicalAnaylzer.Token == Token.IdentifierToken)
            {
                if (!CheckDeclaredVariable()) return false;
                ParameterStack.Push(LexicalAnaylzer.Lexeme);
                LexicalAnaylzer.GetNextToken();
                if (!ProcessParametersTail()) return false;
            }
            else if (LexicalAnaylzer.Token == Token.NumberToken)
            {
                ParameterStack.Push(LexicalAnaylzer.Lexeme);
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

            if (LexicalAnaylzer.Token == Token.CommaToken && LexicalAnaylzer.IsFirstWordCharacter(LexicalAnaylzer.LookNextCharacter()))
            { // TODO: This isn't good - will work for this grammar but shouldn't be checking just next character like so
                LexicalAnaylzer.GetNextToken();
                if (!CheckDeclaredVariable()) return false;
                ParameterStack.Push(LexicalAnaylzer.Lexeme);
                if (!MatchToken(Token.IdentifierToken)) return false;
                if (!ProcessParametersTail()) return false;
            }
            else if (LexicalAnaylzer.Token == Token.CommaToken && LexicalAnaylzer.IsDigitCharacter(LexicalAnaylzer.LookNextCharacter()))
            { // TODO: This isn't good - will work for this grammar but shouldn't be checking just next character like so
                LexicalAnaylzer.GetNextToken();
                ParameterStack.Push(LexicalAnaylzer.Lexeme);
                if (!MatchToken(Token.NumberToken)) return false;
                if (!ProcessParametersTail()) return false;
            }
            return true;
        }

        // Inserts a variable node into symbol table
        private bool InsertVariableNode(bool localVariable)
        {
            if (CurrentVariable.Size == -1) CurrentVariable.Type = CurrentVariableType;
            if (localVariable)
            {
                CurrentVariable.Offset = LocalOffset;
                LocalOffset -= CurrentVariable.Size;
            }
            else
            {
                CurrentVariable.Offset = ParameterOffset;
                ParameterOffset += CurrentVariable.Size;
            }

            CurrentVariable.Depth = Depth;
            if (!InsertNode(CurrentVariable)) return false;
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
            LocalOffsets.Push(LocalOffset);
            ParameterOffsets.Push(ParameterOffset);
            LocalOffset = GlobalConfiguration.BaseLocalOffset;
            ParameterOffset = GlobalConfiguration.BaseParameterOffset;
        }

        // Decrements global program stack information
        private void DecreaseProgramStack()
        {
            OutputThreeAddressCode($"ENDP {CurrentFunction.Lexeme}{Environment.NewLine}");
            SymbolTable.OutputSymbolTable(Depth);
            SymbolTable.DeleteDepth(Depth);
            Depth--;

            CurrentFunction.LocalsSize = Math.Abs(LocalOffset);
            CurrentFunction.ParametersSize = Math.Abs(ParameterOffset) - GlobalConfiguration.BaseParameterOffset;

            LocalOffset = LocalOffsets.Pop();
            ParameterOffset = ParameterOffsets.Pop();
        }

        // Prints to three address code the contents of the parameter stack, and resets stack
        private void PrintThreeAddressCodeParameterStack()
        {
            foreach (string parameter in ParameterStack)
            {
                OutputThreeAddressCode($"\t_PUSH {GetThreeAddressCodeName(parameter)}");
            }
            ParameterStack = new Stack<string>();
        }

        // Returns the three address code variable name of the given lexeme
        private string GetThreeAddressCodeName(string lexeme)
        {
            if (decimal.TryParse(lexeme, out decimal n))
            {
                string temporaryVariable;
                if (int.TryParse(lexeme, out int x)) temporaryVariable = GetTemporaryVariableName(Token.IntToken);
                else temporaryVariable = GetTemporaryVariableName(Token.FloatToken);

                OutputThreeAddressCode($"\t{temporaryVariable} = {lexeme}");
                return temporaryVariable;
            }

            string signOperation = string.Empty;
            if (SignOperations.Contains(lexeme[0].ToString()))
            {
                signOperation = lexeme[0].ToString();
                lexeme = lexeme.Remove(0, 1);
            }

            Node node = SymbolTable.LookupNode(lexeme);
            if (node is VariableNode)
            {
                if (node.Depth == GlobalConfiguration.BaseDepth) return lexeme;
                else return ($"{signOperation}_BP" + ((VariableNode)node).Offset.ToString("+0;-#"));
            }
            else if (node is ConstantNode)
            {
                string temporaryVariable;
                if ((((ConstantNode)node).Value != null)) temporaryVariable = GetTemporaryVariableName(Token.IntToken);
                else temporaryVariable = GetTemporaryVariableName(Token.FloatToken);

                string outputvalue = (((ConstantNode)node).Value ?? ((ConstantNode)node).ValueReal).ToString();
                OutputThreeAddressCode($"\t{temporaryVariable} = {outputvalue}");
                return signOperation + temporaryVariable;
            }
            else if (node is FunctionNode)
            {
                CommonTools.PromptProgramErrorExit($"ERROR: Line {LexicalAnaylzer.LineNumber} Invalid use function {node.Lexeme}");
                return string.Empty;
            }
            else
            {
                return lexeme;
            }

        }

        // Returns a new temporary variable, created by looking at current variable offset
        private string GetTemporaryVariableName(Token type)
        {
            int localOffset;
            if (type == Token.CharToken) localOffset = GlobalConfiguration.CharacterSize;
            else if (type == Token.IntToken) localOffset = GlobalConfiguration.IntegerSize;
            else localOffset = GlobalConfiguration.FloatSize;

            int oldOffset = LocalOffset;
            LocalOffset -= localOffset;


            return ($"_BP{oldOffset.ToString("+0;-#")}");
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

        private bool CheckValidVariableAssignment(string lexeme)
        {
            if (!(SymbolTable.LookupNode(lexeme) is VariableNode))
            {
                CommonTools.PromptProgramErrorExit($"ERROR: Line {LexicalAnaylzer.LineNumber} Invalid " +
                    $"assignment to {lexeme}");
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

        // Outputs to screens when running three address code modules
        private void OutputThreeAddressCode(string output)
        {
            if (CommonTools.ThreeAddressCodeRun) ThreeAddressCodeOutput += output + Environment.NewLine;
        }
    }
}