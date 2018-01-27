using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace CMinusMinusCompiler
{
    // Lexical analyzer finds valid tokens in a specified source file. 
    public class LexicalAnalyzer
    {
        // Public Members
        public char Character { get; set; } = ' ';
        public int LineNumber { get; set; } = 1;
        public int? Value { get; set; }
        public float? ValueReal { get; set; }
        public string Literal { get; set; }
        public TokenType Token { get; set; }
        public string Lexeme { get; set; }

        // Private Members
        private string SourceFileContents { get; set; }
        private Dictionary<string, TokenType> ReserverdWordTokens { get; } = new Dictionary<string, TokenType>
        {
            { "if", TokenType.IfToken }, { "else", TokenType.ElseToken }, { "while", TokenType.WhileToken },
            { "float", TokenType.FloatToken }, { "int", TokenType.IntToken }, { "char", TokenType.CharToken },
            { "break", TokenType.BreakToken }, { "continue", TokenType.ContinueToken }, { "void", TokenType.VoidToken }
        };
        private Dictionary<char, TokenType> SingleCharacterSymbols { get; } = new Dictionary<char, TokenType>
        {
            { ';', TokenType.SemiColonToken }, { '.', TokenType.PeriodToken },
            { '(', TokenType.LeftParenthesisToken }, { ')', TokenType.RightParenthesisToken },
            { '{', TokenType.LeftBraceToken }, { '}', TokenType.RightBraceToken },
            { '[', TokenType.LeftBracketToken }, { ']', TokenType.RightBracketToken },
            { '+', TokenType.AdditionOperatorToken }, { '-', TokenType.AdditionOperatorToken },
            { '*', TokenType.MultiplicationOperatorToken }, { '/', TokenType.MultiplicationOperatorToken },
            { '%', TokenType.MultiplicationOperatorToken }, { ',', TokenType.CommaToken },
            { '<', TokenType.RelationalOperatorToken }, { '>', TokenType.RelationalOperatorToken },
            { '=', TokenType.AssignmentOperatorToken }, { '!', TokenType.UnknownToken },
            { '|', TokenType.UnknownToken }, { '&', TokenType.UnknownToken }
        };
        private Dictionary<string, TokenType> DoubleCharactersSymbols { get; } = new Dictionary<string, TokenType>
        {
            { "==", TokenType.RelationalOperatorToken }, { "!=", TokenType.RelationalOperatorToken },
            { "<=", TokenType.RelationalOperatorToken }, { ">=", TokenType.RelationalOperatorToken },
            { "||", TokenType.AdditionOperatorToken }, { "&&", TokenType.MultiplicationOperatorToken }
        };
        private static string OutputFormat { get; } = "{0,-38} {1,-30} {2}";

        // Constructor that opens specified source file path to begin parsing
        public LexicalAnalyzer(string sourceFilePath)
        {
            // Read source file contents from specified source file path
            try
            {
                SourceFileContents = File.ReadAllText(sourceFilePath).Replace("\r", string.Empty);
            }
            catch (Exception)
            {

                Console.WriteLine("ERROR: Could not open specified source file.");
                CommonTools.PromptProgramExit();
            }
        }

        // Display next token to screen and output file
        public void DisplayCurrentToken()
        {
            if (Token != TokenType.CommentToken && Token != TokenType.EndOfFileToken)
            {
                dynamic attribute = Value ?? ValueReal;
                attribute = attribute ?? Literal;

                string[] outputData = new string[] { Lexeme, Token.ToString(), attribute.ToString() };
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
            while (Char.IsWhiteSpace(Character))
            {
                GetNextCharacter();
                if (Character == '\n') LineNumber++;
            }
            if (Character == Char.MinValue)
            {
                Token = TokenType.EndOfFileToken;
            }
            else
            {
                ProcessToken();
            }
        }

        // Get the next character from the source file contents
        private void GetNextCharacter()
        {
            if (SourceFileContents.Length > 0)
            {
                Character = SourceFileContents[0];
                SourceFileContents = SourceFileContents.Remove(0, 1);
            }
            else
            {
                Character = Char.MinValue;
            }
        }

        // Get the next character from the source file contents without making character "read"
        private char PeakNextCharacter()
        {
            if (SourceFileContents.Length > 0) return SourceFileContents[0];
            else return Char.MinValue;
        }

        // Process next token
        public void ProcessToken()
        {
            UpdateLexemeAndCharacter();

            if (IsFirstWordCharacter(Lexeme[0])) ProcessWordToken();
            else if (IsDigitCharacter(Lexeme[0])) ProcessNumberToken();
            else if (IsForwardSlashCharacter(Lexeme[0])) ProcessForwardSlashToken();
            else if (IsSingleCharacterSymbol(Lexeme[0])) ProcessSymbolToken();
            else if (IsQuotationsSymbol(Lexeme[0])) ProcessStringLiteralToken();
            else Token = TokenType.UnknownToken;
        }

        // Check if character is forward slash caracter
        private bool IsForwardSlashCharacter(char character)
        {
            return character == '/';
        }

        // Check if character is a valid English character (A-Z, a-z)
        private bool IsFirstWordCharacter(char character)
        {
            return (character >= 'A' && character <= 'Z') || (character >= 'a' && character <= 'z');
        }

        // Check if character is a valid character after first letter of an identifier
        private bool IsPostWordCharacter(char character)
        {
            return IsFirstWordCharacter(character) || IsDigitCharacter(character) || character == '_';
        }

        // Check if character is a digit (0-9)
        private bool IsDigitCharacter(char character)
        {
            return Char.IsDigit(character);
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

            int maximumLiteralLength
                = Int32.Parse(ConfigurationManager.AppSettings["MaximumLiteralLength"]);

            if (Lexeme.Length > maximumLiteralLength) Literal
                    = new string(Lexeme.Take(maximumLiteralLength).ToArray());

            Token = ReserverdWordTokens.ContainsKey(Lexeme)
                ? ReserverdWordTokens[Lexeme] : TokenType.IdentifierToken;
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
            Token = TokenType.NumberToken;
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

        // Process the forward slash token which has multiple token paths
        private void ProcessForwardSlashToken()
        {
            if (Character == '*') ProcessCommentToken();
            else Token = TokenType.MultiplicationOperatorToken;
        }

        // Process a comment token
        private void ProcessCommentToken()
        {
            Token = TokenType.CommentToken;
            UpdateLexemeAndCharacter();

            while (Character != Char.MinValue)
            {
                GetNextCharacter();
                if (CommentEndSearch()) return;
            }

            CommonTools.WriteOutput("ERROR: Expected comment end '*/' not found");
        }

        // Search for the end of a comment in the input
        private bool CommentEndSearch()
        {
            if (Character == '*')
            {
                GetNextCharacter();
                if (Character == '/')
                {
                    Character = ' ';
                    return true;
                }
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
                    Token = TokenType.UnknownToken;
                    return;
                }

                UpdateLexemeAndCharacter();
            }

            UpdateLexemeAndCharacter();
            Token = TokenType.StringLiteralToken;
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

        // Enumerated type to contain all possible token types
        public enum TokenType
        {
            IfToken, ElseToken, WhileToken, FloatToken, IntToken, CharToken,
            BreakToken, ContinueToken, VoidToken, CommaToken, SemiColonToken,
            AssignmentOperatorToken, EndOfFileToken, AdditionOperatorToken,
            MultiplicationOperatorToken, LeftParenthesisToken, RightParenthesisToken,
            LeftBraceToken, RightBraceToken, LeftBracketToken, RightBracketToken,
            PeriodToken, QuotationsSymbol, RelationalOperatorToken, IdentifierToken,
            NumberToken, CommentToken, StringLiteralToken, UnderscoreToken,
            UnknownToken,
        }
    }
}