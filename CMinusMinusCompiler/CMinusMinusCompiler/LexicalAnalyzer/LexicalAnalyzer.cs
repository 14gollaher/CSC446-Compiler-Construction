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
        public enum Symbol
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

        public Symbol Token { get; set; }
        public string Lexeme { get; set; }
        public char Character { get; set; } = ' ';
        public int LineNumber { get; set; } = 1;
        public int? Value { get; set; }
        public float? ValueReal { get; set; }
        public string Literal { get; set; }

        private string SourceFileContents { get; set; }
        private string OutputFormat { get; } = "{0,-38} {1,-30} {2}";

        private Dictionary<string, Symbol> ReserverdWordTokens { get; } = new Dictionary<string, Symbol>
        {
            { "if", Symbol.IfToken }, { "else", Symbol.ElseToken }, { "while", Symbol.WhileToken },
            { "float", Symbol.FloatToken }, { "int", Symbol.IntToken }, { "char", Symbol.CharToken },
            { "break", Symbol.BreakToken }, { "continue", Symbol.ElseToken }, { "void", Symbol.VoidToken }
        };

        private Dictionary<char, Symbol> SingleCharacterSymbols { get; } = new Dictionary<char, Symbol>
        {
            { ';', Symbol.SemiColonToken }, { '.', Symbol.PeriodToken },
            { '(', Symbol.LeftParenthesisToken }, { ')', Symbol.RightParenthesisToken },
            { '{', Symbol.LeftBraceToken }, { '}', Symbol.RightBraceToken },
            { '[', Symbol.LeftBracketToken }, { ']', Symbol.RightBracketToken },
            { '+', Symbol.AdditionOperatorToken }, { '-', Symbol.AdditionOperatorToken },
            { '*', Symbol.MultiplicationOperatorToken }, { '/', Symbol.MultiplicationOperatorToken },
            { '%', Symbol.MultiplicationOperatorToken }, { ',', Symbol.CommaToken },
            { '=', Symbol.AssignmentOperatorToken }, { '!', Symbol.UnknownToken },
            { '<', Symbol.RelationalOperatorToken }, { '>', Symbol.RelationalOperatorToken },
            { '|', Symbol.UnknownToken }, { '&', Symbol.UnknownToken }
        };

        private Dictionary<string, Symbol> DoubleCharactersSymbols { get; } = new Dictionary<string, Symbol>
        {
            { "==", Symbol.RelationalOperatorToken }, { "!=", Symbol.RelationalOperatorToken },
            { "<=", Symbol.RelationalOperatorToken }, { ">=", Symbol.RelationalOperatorToken },
            { "||", Symbol.AdditionOperatorToken }, { "&&", Symbol.MultiplicationOperatorToken }
        };

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
                CommonTools.ExitProgram();
            }
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
                Token = Symbol.EndOfFileToken;
            }
            else
            {
                ProcessToken();
            }
        }

        // Display next token to screen and output file
        public void DisplayCurrentToken()
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
        public void DisplayTokenHeader()
        {
            File.Delete(CommonTools.OutputFilePath);
            string[] headingData = new string[] { "Lexeme", "Token", "Attributes" };
            string headerRule = Environment.NewLine + new string('-', 80);
            CommonTools.WriteOutput(string.Format(OutputFormat, headingData) + headerRule);
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

        // Process next token
        public void ProcessToken()
        {
            UpdateLexemeAndCharacter();

            if (IsFirstWordCharacter(Lexeme[0])) ProcessWordToken();
            else if (IsDigitCharacter(Lexeme[0])) ProcessNumberToken();
            else if (IsForwardSlashCharacter(Lexeme[0])) ProcessForwardSlashToken();
            else if (IsSingleCharacterSymbol(Lexeme[0])) ProcessSymbolToken();
            else if (IsQuotationsSymbol(Lexeme[0])) ProcessStringLiteralToken();
            else Token = Symbol.UnknownToken;
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

        private bool IsSingleCharacterSymbol(char character)
        {
            return SingleCharacterSymbols.ContainsKey(character) ;
        }

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
                ? ReserverdWordTokens[Lexeme] : Symbol.IdentifierToken;
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
            ProcessRemainingNumberToken();

            if (Character == '.')
            {
                GetNextCharacter();
                if (IsDigitCharacter(Character))
                {
                    Lexeme = Lexeme + '.' + Character;
                    GetNextCharacter();
                    ProcessRemainingNumberToken();
                    ValueReal = Convert.ToSingle(Lexeme);
                }
            }
            else
            {
                Value = Convert.ToInt32(Lexeme);
            }

            Token = Symbol.NumberToken;
        }

        // Process remaining expected number tokens
        private void ProcessRemainingNumberToken()
        {
            while (IsDigitCharacter(Character))
            {
                UpdateLexemeAndCharacter();
            }
        }

        private void ProcessForwardSlashToken()
        {
            if (Character == '*') ProcessCommentToken();
            else Token = Symbol.MultiplicationOperatorToken;
        }

        private void ProcessCommentToken()
        {
            UpdateLexemeAndCharacter();

            while (Character != Char.MinValue)
            {
                GetNextCharacter();
                if (CommentEndSearch()) return;
            }

            CommonTools.WriteOutput("ERROR: Expected comment end '*/' not found");
        }

        private bool CommentEndSearch()
        {
            if (Character == '*')
            {
                GetNextCharacter();
                if (Character == '/')
                {
                    Token = Symbol.CommentToken;
                    Character = ' ';
                    return true;
                }
            }

            return false;
        }

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

        private void ProcessStringLiteralToken()
        {
            while (!IsQuotationsSymbol(Character))
            {
                if (IsEndOfLineCharacter(Character))
                {
                    Token = Symbol.UnknownToken;
                    return;
                }

                UpdateLexemeAndCharacter();
            }

            UpdateLexemeAndCharacter();
            Token = Symbol.StringLiteralToken;
            Literal = Lexeme;
        }

        private bool IsEndOfLineCharacter(char character)
        {
            return character == '\n' || character == Char.MinValue;
        }

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
}

