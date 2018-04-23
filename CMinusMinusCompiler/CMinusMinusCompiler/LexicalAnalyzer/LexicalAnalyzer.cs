using System;
using System.Collections.Generic;
using System.IO;

namespace CMinusMinusCompiler
{
    // Lexical analyzer finds valid tokens in a specified source file. 
    public class LexicalAnalyzer
    {
        // Public Members
        public int LineNumber { get; set; } = 1;
        public int? Value { get; set; }
        public float? ValueReal { get; set; }
        public string Literal { get; set; }
        public Token Token { get; set; }
        public string Lexeme { get; set; }
        public char Character { get; set; } = ' ';

        // Private properties
        private string SourceFileContents { get; set; }
        private Dictionary<string, Token> ReserverdWordTokens { get; } = new Dictionary<string, Token> {
            { "if", Token.IfToken }, { "else", Token.ElseToken }, { "while", Token.WhileToken },
            { "float", Token.FloatToken }, { "int", Token.IntToken }, { "char", Token.CharToken },
            { "break", Token.BreakToken }, { "continue", Token.ContinueToken }, { "void", Token.VoidToken },
            { "const", Token.ConstToken }, { "return", Token.ReturnToken }, { "cin", Token.CharacterInToken },
            { "cout", Token.CharacterOutToken }, { "endl", Token.EndLineToken }
        };
        private Dictionary<char, Token> SingleCharacterSymbols { get; } = new Dictionary<char, Token> {
            { ';', Token.SemiColonToken }, { '.', Token.PeriodToken }, { '(', Token.LeftParenthesisToken },
            { ')', Token.RightParenthesisToken }, { '{', Token.LeftBraceToken }, { '}', Token.RightBraceToken },
            { '[', Token.LeftBracketToken }, { ']', Token.RightBracketToken }, { '+', Token.AdditionOperatorToken },
            { '-', Token.AdditionOperatorToken }, { '*', Token.MultiplicationOperatorToken }, { '/', Token.MultiplicationOperatorToken },
            { '%', Token.MultiplicationOperatorToken }, { ',', Token.CommaToken }, { '<', Token.RelationalOperatorToken },
            { '>', Token.RelationalOperatorToken }, { '=', Token.AssignmentOperatorToken }, { '!', Token.NotOperatorToken },
            { '|', Token.UnknownToken }, { '&', Token.UnknownToken }
        };
        private Dictionary<string, Token> DoubleCharactersSymbols { get; } = new Dictionary<string, Token> {
            { "==", Token.RelationalOperatorToken }, { "!=", Token.RelationalOperatorToken }, { "<=", Token.RelationalOperatorToken },
            { ">=", Token.RelationalOperatorToken }, { "||", Token.AdditionOperatorToken }, { "&&", Token.MultiplicationOperatorToken },
            { "<<", Token.LeftShiftOperatorToken}, { ">>", Token.RightShiftOperatorToken } 
        };
        private static string OutputFormat { get; } = "{0,-38} {1,-30} {2}";

        // Constructor that opens specified source file path to begin parsing
        public LexicalAnalyzer(string sourceFilePath)
        {
            SourceFileContents = File.ReadAllText(sourceFilePath).Replace("\r", string.Empty);
        }

        // Display next token to screen and output file
        public void DisplayCurrentToken()
        {
            if (Token != Token.EndOfFileToken)
            {
                dynamic attribute = Value ?? ValueReal;
                attribute = attribute ?? Literal;

                string[] outputData  = new string[] { Lexeme, Token.ToString(), attribute.ToString() };
                CommonTools.WriteOutput(string.Format(OutputFormat, outputData));
            }
        }

        // Display token header to screen and output file
        public void DisplayTokenHeader()
        {
            string[] headingData = new string[] { "Lexeme", "Token", "Attribute" };
            string headerRule = Environment.NewLine + new string('-', 79);
            CommonTools.WriteOutput(string.Format(OutputFormat, headingData) + headerRule);
        }

        // Get next token from source file contents
        public void GetNextToken()
        {
            ClearLexicalResources();
            LookNextCharacter();
            if (Character == Char.MinValue) Token = Token.EndOfFileToken;
            else ProcessToken();
        }

        // Get the next character from the source file, skipping comments and whitespace
        public char LookNextCharacter()
        {
            while (Character == '/' && PeakNextCharacter() == '*' || Char.IsWhiteSpace(Character))
            {
                if (Character == '/' && PeakNextCharacter() == '*')
                {
                    SkipComment();
                    GetNextCharacter();
                }
                else
                {
                    GetNextCharacter();
                }
            }
            return Character;
        }

        // Check if character is a digit (0-9)
        public bool IsDigitCharacter(char character)
        {
            return Char.IsDigit(character);
        }

        // Check if character is a valid English character (A-Z, a-z)
        public bool IsFirstWordCharacter(char character)
        {
            return (character >= 'A' && character <= 'Z') || (character >= 'a' && character <= 'z');
        }

        // Get the next character from the source file contents
        private void GetNextCharacter()
        {
            if (SourceFileContents.Length > 0)
            {
                Character = SourceFileContents[0];
                SourceFileContents = SourceFileContents.Remove(0, 1);
                if (Character == '\n') LineNumber++;
            }
            else
            {
                Character = Char.MinValue;
            }
        }

        // Get the next character from the source file contents without making character "read"
        private char PeakNextCharacter()
        {
            if (SourceFileContents.Length == 0) return Char.MinValue;

            return SourceFileContents[0];
        }

        // Process token based on next current and next character in the lexeme
        private void ProcessToken()
        {
            UpdateLexemeAndCharacter();

            if (IsFirstWordCharacter(Lexeme[0])) ProcessWordToken();
            else if (IsDigitCharacter(Lexeme[0])) ProcessNumberToken();
            else if (IsForwardSlashCharacter(Lexeme[0])) ProcessForwardSlashToken();
            else if (IsSingleCharacterSymbol(Lexeme[0])) ProcessSymbolToken();
            else if (IsQuotationsSymbol(Lexeme[0])) ProcessStringLiteralToken();
            else Token = Token.UnknownToken;
        }

        // Check if character is forward slash caracter
        private bool IsForwardSlashCharacter(char character)
        {
            return character == '/';
        }

        // Check if character is a valid character after first letter of an identifier
        private bool IsPostWordCharacter(char character)
        {
            return IsFirstWordCharacter(character) || IsDigitCharacter(character) || character == '_';
        }

        // Check if next character is a single character symbol (%, *, ...)
        private bool IsSingleCharacterSymbol(char character)
        {
            return SingleCharacterSymbols.ContainsKey(character);
        }

        // Check if next character is a quotations symbol (")
        private bool IsQuotationsSymbol(char character)
        {
            return character == '"';
        }

        // Proccess a potential word token
        private void ProcessWordToken()
        {
            ProcessRemainingWordToken();

            int MaximumIdentifierLength = GlobalConfiguration.MaximumIdentifierLength;

            if (Lexeme.Length > MaximumIdentifierLength)
            {
                Token = Token.UnknownToken;
            }
            else
            {
                Token = ReserverdWordTokens.ContainsKey(Lexeme)
                    ? ReserverdWordTokens[Lexeme] : Token.IdentifierToken;
            }
        }

        // Process remaining expected word tokens
        private void ProcessRemainingWordToken()
        {
            while (IsPostWordCharacter(Character))
            {
                UpdateLexemeAndCharacter();
            }
        }

        // Process a potential number token
        private void ProcessNumberToken()
        {
            Token = Token.NumberToken;
            ProcessRemainingNumberToken();

            if (Character == '.')
            {
                if (!IsDigitCharacter(PeakNextCharacter()))
                {
                    Value = Convert.ToInt32(Lexeme);
                    return;
                }

                UpdateLexemeAndCharacter();
                ProcessRemainingNumberToken();
                ValueReal = Convert.ToSingle(Lexeme);
            }
            else
            {
                Value = Convert.ToInt32(Lexeme);
            }
        }

        // Process remaining expected number tokens
        private void ProcessRemainingNumberToken()
        {
            while (IsDigitCharacter(Character))
            {
                UpdateLexemeAndCharacter();
            }
        }

        // Process the forward slash token
        private void ProcessForwardSlashToken()
        {
            Token = Token.MultiplicationOperatorToken;
        }

        // Process a comment token
        private void SkipComment()
        {
            while (Character != Char.MinValue)
            {
                GetNextCharacter();
                if (CommentEndSearch())
                {
                    return;
                }
            }
            
            CommonTools.WriteOutput("ERROR: Expected comment end '*/' not found");
        }

        // Search for the end of a comment in the input
        private bool CommentEndSearch()
        {
            if (Character == '*' && PeakNextCharacter() == '/')
            {
                GetNextCharacter();
                return true;
            }

            return false;
        }

        // Process a symbol token using dictionary map members
        private void ProcessSymbolToken()
        {
            if (DoubleCharactersSymbols.ContainsKey(Lexeme + Character))
            {
                UpdateLexemeAndCharacter();
                Token = DoubleCharactersSymbols[Lexeme];
            }
            else
            {
                Token = SingleCharacterSymbols[Lexeme[0]];
            }
        }

        // Process a string literal token
        private void ProcessStringLiteralToken()
        {
            while (!IsQuotationsSymbol(Character))
            {
                if (IsEndOfLineCharacter(Character))
                {
                    Token = Token.UnknownToken;
                    return;
                }

                UpdateLexemeAndCharacter();
            }

            UpdateLexemeAndCharacter();
            Token = Token.StringLiteralToken;
            Literal = Lexeme;
        }

        // Check for determing if character is end of line or file character
        private bool IsEndOfLineCharacter(char character)
        {
            return character == '\n' || character == Char.MinValue;
        }

        // Updates the lexeme with the current character and gets new character
        private void UpdateLexemeAndCharacter()
        {
            Lexeme += Character;
            GetNextCharacter();
        }

        // Clear Lexical Analyzer public variables 
        private void ClearLexicalResources()
        {
            Lexeme = string.Empty;
            Literal = string.Empty;
            Value = null;
            ValueReal = null;
        }
    }

    // Enumerated type to contain all possible token types
    public enum Token
    {
        IfToken, ElseToken, WhileToken, FloatToken, IntToken, CharToken, BreakToken, ContinueToken, ReturnToken,
        VoidToken, CommaToken, SemiColonToken, AssignmentOperatorToken, EndOfFileToken, AdditionOperatorToken,
        MultiplicationOperatorToken, NumberToken, LeftParenthesisToken, RightParenthesisToken, LeftBraceToken,
        RightBraceToken, LeftBracketToken, RightBracketToken, PeriodToken, QuotationsSymbol, ConstToken,
        RelationalOperatorToken, IdentifierToken, StringLiteralToken, UnderscoreToken, NotOperatorToken, EndLineToken,
        LeftShiftOperatorToken, RightShiftOperatorToken, CharacterInToken, CharacterOutToken, UnknownToken,
    }
}