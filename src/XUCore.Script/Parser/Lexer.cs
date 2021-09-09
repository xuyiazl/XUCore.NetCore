using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace XUCore.Script
{
    /// <summary>
    /// Converts script from a series of characters into a series of tokens.
    /// Main method is NextToken();
    /// A script can be broken down into a sequence of tokens.
    /// e.g.
    /// 
    /// 1. var name = "kishore";
    /// Tokens:
    /// 
    ///  TOKEN VALUE:         TOKEN TYPE:
    ///  var         keyword
    ///  ""          literal ( whitespace )
    ///  name        id
    ///  ""          literal ( whitespace )
    ///  =           operator
    ///  ""          literal ( whitespace )
    ///  "kishore"   literal
    ///  ;           operator
    /// </summary>
    public class Lexer
    {
        private Scanner _scanner;
        private Token _lastToken;
        private int _lineNumber = 1;
        private int _lineCharPosition = -1;
        private bool _hasReplacementsOrRemovals = false;
        private Dictionary<string, string> _replacements = new Dictionary<string, string>();
        private Dictionary<string, Tuple<bool, string>> _inserts = new Dictionary<string, Tuple<bool, string>>();
        private Dictionary<string, bool> _removals = new Dictionary<string, bool>();
        private static IDictionary<char, bool> _opChars = new Dictionary<char, bool>()
        {
            { '*', true},
            { '/', true},
            { '+', true},
            { '-', true},
            { '%', true},
            { '<', true},
            { '>', true},
            { '=', true},
            { '!', true},
            { '&', true},
            { '|', true}             
        };


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="scanner"></param>
        public Lexer(Scanner scanner)
        {
            _scanner = scanner;
        }


        /// <summary>
        /// The current line number.
        /// </summary>
        public int LineNumber { get { return _lineNumber; } }


        /// <summary>
        /// The char position on the current line.
        /// </summary>
        public int LineCharPos { get { return _lineCharPosition; } }


        /// <summary>
        /// Replaces a token with another token.
        /// </summary>
        /// <param name="text">The text to replace</param>
        /// <param name="newValue">The replacement text</param>
        public void SetReplacement(string text, string newValue)
        {
            // check if replacements has a space. can do at most 2 words in a replacement for now.
            // e.g. can replace "number of" with "count".
            _replacements[text] = newValue;
            _hasReplacementsOrRemovals = true;
        }


        /// <summary>
        /// Removes a token during the lexing process.
        /// </summary>        
        /// <param name="text">The text to remove</param>
        public void SetRemoval(string text)
        {
            _removals[text] = true;
            _hasReplacementsOrRemovals = true;
        }


        /// <summary>
        /// Adds a token during the lexing process.
        /// </summary>
        /// <param name="before">whether to insert before or after</param>
        /// <param name="text">The text to check for inserting before/after</param>
        /// <param name="newValue">The new value to insert before/after</param>
        public void SetInsert(bool before, string text, string newValue)
        {
            _inserts[text] = new Tuple<bool, string>(before, newValue);
        }


        /// <summary>
        /// The current token.
        /// </summary>
        internal Token LastToken { get { return _lastToken; } }


        /// <summary>
        /// The scanner used for parsing.
        /// </summary>
        internal Scanner Scanner { get { return _scanner; } }


        /// <summary>
        /// Returns a list of tokens of the entire script.
        /// </summary>
        /// <returns></returns>
        internal List<TokenData> Tokenize()
        {
            var tokens = new List<TokenData>();
            while (true)
            {
                var token = NextToken();

                if (token.Token != null)
                {
                    if (token.Token == Token.EndToken) 
                        tokens.Add(token);
                    else
                    {
                        if (!_hasReplacementsOrRemovals)
                            tokens.Add(token);

                        // Case 1: Replace token ?
                        else if (_replacements.ContainsKey(token.Token.Text))
                        {
                            var replaceVal = _replacements[token.Token.Text];

                            // Replaces system token?
                            if (Tokens.AllTokens.ContainsKey(replaceVal))
                            {
                                Token t = Tokens.AllTokens[replaceVal];
                                token.Token = t;
                            }
                            else
                            {
                                token.Token.Replace(replaceVal);
                            }                            
                            tokens.Add(token);
                        }
                        // Case 2: Remove token ?
                        else if (!_removals.ContainsKey(token.Token.Text))
                            tokens.Add(token);
                    }
                }
                if (token.Token == Token.EndToken)
                    break;
            }
            return tokens;
        }

        
        /// <summary>
        /// Reads the next token from the reader.
        /// </summary>
        /// <returns> A token, or <c>null</c> if there are no more tokens. </returns>
        internal TokenData NextToken()
        {
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // LEXER ALWAYS READS NEXT CHAR
            char c = _scanner.ReadChar();
            char n = _scanner.PeekChar();
            _lineCharPosition++;

            if (_scanner.IsEnd())
            {
                _lastToken = Token.EndToken;
            }
            // Empty space.
            else if ( c == ' ' || c == '\t' )
            {
                _scanner.ConsumeWhiteSpace(false, false);
                _lastToken = Tokens.WhiteSpace;
            }
            // Variable
            else if (IsStartChar(c))
            {
                _lastToken = ReadWord();
            }
            // Operator ( Math, Compare, Increment ) * / + -, < < > >= ! =
            else if ( IsOp(c) == true)
            {
                _lastToken = ReadOperator();
            }
            else if (c == '(')
            {
                _lastToken = Tokens.LeftParenthesis;
            }
            else if (c == ')')
            {
                _lastToken = Tokens.RightParenthesis;
            }
            else if (c == '{')
            {
                _lastToken = Tokens.LeftBrace;
            }
            else if (c == '}')
            {
                _lastToken = Tokens.RightBrace;
            }
            else if (c == '[')
            {
                _lastToken = Tokens.LeftBracket;
            }
            else if (c == ']')
            {
                _lastToken = Tokens.RightBracket;
            }
            else if (c == '.')
            {
                _lastToken = Tokens.Dot;
            }
            else if (c == ',')
            {
                _lastToken = Tokens.Comma;
            }
            else if (c == ';')
            {
                _lastToken = Tokens.Semicolon;
            }
            else if (c == ':')
            {
                _lastToken = Tokens.Colon;
            }
            // String literal
            else if (c == '"' || c == '\'')
            {
                _lastToken = ReadString();
            }
            else if (IsNumeric(c))
            {
                _lastToken = ReadNumber();
            }
            // Single line
            else if (c == '/' && n == '/')
            {
                var result = _scanner.ReadLine(false);
                if (result.Success) _lineNumber++;
                _lineCharPosition = -1;
                _lastToken = Tokens.CommentSLine;
            }
            // Multi-line
            else if (c == '/' && n == '*')
            {
                var result = _scanner.ReadUntilChars(false, '*', '/');
                if (result.Success)
                {
                    _lineNumber += result.Lines;
                }
                _lineCharPosition = -1;
                _lastToken = Tokens.CommentMLine;
            }
            else if (c == '\r' && n == '\n')
            {
                _scanner.ReadChar();
                _lineNumber++;
                _lineCharPosition = -1;
                _lastToken = Tokens.NewLine;
            }           
            var t = new TokenData() { Token = _lastToken, Line = _lineNumber, CharPos = _lineCharPosition };

            // Before returning, set the next line char position.
            if (_lineCharPosition != -1 && _lastToken != null && !string.IsNullOrEmpty(_lastToken.Text))
            {
                _lineCharPosition += _lastToken.Text.Length;
            }
            return t;
        }


        /// <summary>
        /// Read word
        /// </summary>
        /// <returns></returns>
        private Token ReadWord()
        {
            var result = _scanner.ReadId(false, false);

            // true / false / null
            if (Tokens.IsLiteral(result.Text))
                return Tokens.ToLiteral(result.Text);

            // var / for / while
            if (Tokens.IsKeyword(result.Text))
                return Tokens.ToKeyWord(result.Text);

            return new IdToken(result.Text);
        }


        /// <summary>
        /// Read number
        /// </summary>
        /// <returns></returns>
        private Token ReadNumber()
        {
            var result = _scanner.ReadNumber(false, false);
            return new LiteralToken(result.Text, Convert.ToDouble(result.Text), false);
        }


        /// <summary>
        /// Read an operator
        /// </summary>
        /// <returns></returns>
        private Token ReadOperator()
        {
            var result = _scanner.ReadChars(_opChars, false, false);
            return Tokens.ToOperator(result.Text);
        }


        /// <summary>
        /// Reads a string either in quote or double quote format.
        /// </summary>
        /// <returns></returns>
        private Token ReadString(bool handleInterpolation = false)
        {
            char quote = _scanner.CurrentChar;
                
            // 1. Starts with either ' or "
            // 2. Handles interpolation "homepage of ${user.name} is ${url}"
            if (!handleInterpolation)
            {
                var result = _scanner.ReadCodeString(quote, setPosAfterToken: false);
                return new LiteralToken(result.Text, result.Text, false);
            }
            // Only supporting following:
            // 1. id's abcd with "_"
            // 2. "."
            // 3. math ops ( + - / * %)
            return null;
        }


        private static bool IsStartChar(int c)
        {
            return c == '$' || c == '_' || ('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z');
        }


        private static bool IsOp(char c)
        {
            return IsMathOp(c) || IsCompareOp(c) || IsLogicOp(c);
        }


        private static bool IsMathOp(char c)
        {
            return  c == '*' || c == '/' || c == '+' || c == '-' || c == '%';
        }


        private static bool IsLogicOp(char c)
        {
            return c == '&' || c == '|';
        }


        private static bool IsCompareOp(char c)
        {
            return c == '<' || c == '>' || c == '!' || c == '=';
        }

        
        private bool IsNumeric(char c)
        {
            return c == '.' || (c >= '0' && c <= '9');
        }

        
        private bool IsStringStart(char c)
        {
            return c == '"' || c == '\'';
        }


        private bool IsWhiteSpace(char c)
        {
            return c == ' ' || c == '\t';
        }
    }
}
