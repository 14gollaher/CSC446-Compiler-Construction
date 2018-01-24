﻿using System;
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
            AssignmentOperatorToken, EndOfFileToken, AddOperationToken,
            MultiplicationOperationToken, DivisionOperatorationToken,
            LeftParenthesisToken, RightParenthesisToken, LeftBraceToken,
            RightBraceToken, LeftBracketToken, RightBracketToken, PeriodToken,
            QuotationsSymbol, RelationalOperationToken, IdentifierToken,
            NumberToken, UnknownToken
        }

        public Symbol Token { get; set; }
        public string Lexeme { get; set; }
        public char Character { get; set; }
        public int LineNumber { get; set; } = 1;
        public int? Value { get; set; }
        public float? ValueReal { get; set; }
        public string Literal { get; set; }

        private string SourceFileContents { get; set; }
        private string OutputFormat { get; } = "{0,-35} {1,-20} {2,-15}";

        private Dictionary<string, Symbol> ReserverdWordTokens { get; } = new Dictionary<string, Symbol>
        {
            { "if", Symbol.IfToken }, { "else", Symbol.ElseToken }, { "while", Symbol.WhileToken },
            { "float", Symbol.FloatToken }, { "int", Symbol.IntToken }, { "char", Symbol.CharToken },
            { "break", Symbol.BreakToken }, { "continue", Symbol.ElseToken }, { "void", Symbol.VoidToken }
        };

        private Dictionary<char, Symbol> SingleTokens { get; } = new Dictionary<char, Symbol>
        {
            { ';', Symbol.SemiColonToken }, { '.', Symbol.PeriodToken },
            { ',', Symbol.CommaToken }, { '"', Symbol.QuotationsSymbol },
            { '(', Symbol.LeftParenthesisToken }, { ')', Symbol.RightParenthesisToken },
            { '{', Symbol.LeftBraceToken }, { '}', Symbol.RightBraceToken },
            { '[', Symbol.LeftBracketToken }, { ']', Symbol.RightBracketToken },
            { '+', Symbol.AddOperationToken }, { '-', Symbol.RightBracketToken },
            { '*', Symbol.MultiplicationOperationToken }, { '/', Symbol.MultiplicationOperationToken },
            { '%', Symbol.MultiplicationOperationToken }, { '=', Symbol.AssignmentOperatorToken },
            { '<', Symbol.RelationalOperationToken }, { '>', Symbol.RelationalOperationToken }
        };

        private Dictionary<char, Symbol> DoubleTokensSecondValue { get; } = new Dictionary<char, Symbol>
        {
            { '|', Symbol.AddOperationToken }, { '&', Symbol.MultiplicationOperationToken },
            { '=', Symbol.RelationalOperationToken }
        };

        private List<string> WordPreceedingValidTokens { get; } = new List<string>
        {
            ";", ",", "(", ")", "{", "}", "[", "]", "+", "-", "*", "/", "%", "=", "<", ">"
        };

        private List<string> NumberPreceedingValidTokens { get; } = new List<string>
        {
            ";", ",", "(", ")", "{", "}", "+", "-", "*", "/", "%", "=", "<", ">"
        };


        // Constructor that opens specified source file path to begin parsing
        public LexicalAnalyzer(string sourceFilePath)
        {
            try
            {
                SourceFileContents = File.ReadAllText(sourceFilePath);
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
            while (Char.IsWhiteSpace(GetNextCharacter()))
            {
                if (Character == '\n') LineNumber++;
            }
            if (Character == Char.MinValue)
            {
                Token = Symbol.EndOfFileToken;
                Lexeme = "\\n";
            }
            else
            {
                ProcessToken();
            }
        }

        // Display next token to screen and output file
        public void DisplayCurrentToken()
        {
            dynamic attribute = Value ?? ValueReal;
            attribute = attribute ?? Literal;

            string[] outputData = new string[] { Lexeme, Token.ToString(), attribute.ToString() };
            CommonTools.WriteOutput(string.Format(OutputFormat, outputData));
        }

        // Display token header to screen and output file
        public void DisplayTokenHeader()
        {
            File.Delete(CommonTools.OutputFilePath);
            string[] headingData = new string[] { "Lexeme", "Token", "Attributes" };
            string headerRule = Environment.NewLine + new string('-', 67);
            CommonTools.WriteOutput(string.Format(OutputFormat, headingData) + headerRule);
        }

        // Get the next character from the source file contents
        private char GetNextCharacter()
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
            return Character;
        }

        public void ProcessToken()
        {
            Lexeme += Character;
            GetNextCharacter();

            if (IsFirstWordCharacter(Lexeme[0])) ProcessWordToken();
            else if (IsDigitCharacter(Lexeme[0])) ProcessNumberToken();
            else Token = Symbol.UnknownToken;
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

        // Proccess a potential word token
        private void ProcessWordToken()
        {
            Lexeme += Character;

            while (IsPostWordCharacter(GetNextCharacter()))
            {
                Lexeme += Character;
            }

            int maximumLiteralLength = Int32.Parse(ConfigurationManager.AppSettings["MaximumLiteralLength"]);
            if (Lexeme.Length > maximumLiteralLength) Literal = new string(Lexeme.Take(maximumLiteralLength).ToArray());
    
            Token = ReserverdWordTokens.ContainsKey(Lexeme) ? ReserverdWordTokens[Lexeme] : Symbol.IdentifierToken;
        }

        // Process a potential number token
        private void ProcessNumberToken()
        {
            ProcessRemainingNumberToken();

            if (Character == '.')
            {
                GetNextCharacter();
                ProcessRemainingNumberToken();
            }
            else
            {
                Value = Convert.ToInt32(Lexeme);
            }

            if (SingleTokens.ContainsKey(Character) || Char.IsWhiteSpace(Character))
            {
                Token = Symbol.NumberToken;
            }
            else
            {
                // Unexpected characters in current token
                ProcessRemainingCharactersToSpace();
                Token = Symbol.UnknownToken;
            }
        }

        // Process remaining characters until whitespace
        private void ProcessRemainingCharactersToSpace()
        {
            Lexeme += Character;
            while (Char.IsWhiteSpace(GetNextCharacter()))
            {
                Lexeme += Character;
            }
        }

        // Process remaining expected number tokens
        private void ProcessRemainingNumberToken()
        {
            Lexeme += Character;
            while (IsDigitCharacter(GetNextCharacter()))
            {
                Lexeme += Character;
            }
        }

        // Clear public variables 
        private void ClearLexicalResources()
        {
            Lexeme = string.Empty;
            Literal = string.Empty;
            Character = ' ';
            Value = null;
            ValueReal = null;
        }

        // Clear public attributes
        private void ClearTokenAttributes()
        {
            Literal = string.Empty;
            Character = ' ';
            Value = null;
            ValueReal = null;
        }

        //// Writes an error specific to the lexical analysis
        //private void WriteLexicalError(string message)
        //{
        //    CommonTools.WriteOutput("ERROR: Line " + LineNumber + " - " + message);
        //    Token = Symbol.UnknownToken;
        //}
    }
}

