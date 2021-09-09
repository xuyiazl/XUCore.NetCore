namespace XUCore.Script
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:         
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/12/1 15:31:01
    *                   mailto:3624091@qq.com
    *                         
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections;

    /// <summary>
    /// Settings for the token reader class.
    /// </summary>
    public class ScannerSettings
    {
        /// <summary>
        /// Char used to escape.
        /// </summary>
        public char EscapeChar;


        /// <summary>
        /// Tokens
        /// </summary>
        public char[] Tokens;


        /// <summary>
        /// White space tokens.
        /// </summary>
        public char[] WhiteSpaceTokens;
    }



    /// <summary>
    /// The result of a scan for a specific token
    /// </summary>
    public class ScanTokenResult
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="success"></param>
        /// <param name="text"></param>
        public ScanTokenResult(bool success, string text)
        {
            Success = success;
            Text = text;
        }


        /// <summary>
        /// Whether or not the token was properly present
        /// </summary>
        public readonly bool Success;


        /// <summary>
        /// The text of the token.
        /// </summary>
        public readonly string Text;


        /// <summary>
        /// Number of lines parsed.
        /// </summary>
        public int Lines;
    }



    /// <summary>
    /// This class implements a token reader.
    /// </summary>
    public class Scanner
    {
        private int _pos;
        private string _text;
        private IDictionary<char, char> _whiteSpaceChars;
        private IDictionary<char, char> _tokens;
        private Dictionary<char, char> _readonlyWhiteSpaceMap;
        private char _currentChar;
        private char _nextChar;
        private char _escapeChar;
        private int LAST_POSITION;
        private List<string> _errors = new List<string>();
        private char END_CHAR = ' ';

        private const char QUOTE = '\"';
        private const char TICK = '\'';

        /// <summary>
        /// Initialize this instance with defaults.
        /// </summary>
        public Scanner()
            : this(string.Empty)
        {
        }


        /// <summary>
        /// Initialize with text to parse.
        /// </summary>
        /// <param name="text"></param>
        public Scanner(string text)
        {
            Init(text, '\\', new char[] { TICK, QUOTE }, new char[] { ' ', '\t' });
        }


        /// <summary>
        /// Initialize with text to parse.
        /// </summary>
        /// <param name="text">The text to scan</param>
        /// <param name="reservedChars">Reserved chars</param>
        public Scanner(string text, char[] reservedChars)
        {
            Init(text, '\\', reservedChars, new char[] { ' ', '\t' });
        }


        /// <summary>
        /// Initialize this instance with supplied parameters.
        /// </summary>
        /// <param name="text">Text to use.</param>
        /// <param name="escapeChar">Escape character.</param>
        /// <param name="tokenChars">Special characters</param>
        /// <param name="whiteSpaceTokens">Array with whitespace tokens.</param>
        public Scanner(string text, char escapeChar, char[] tokenChars, char[] whiteSpaceTokens)
        {
            Init(text, escapeChar, tokenChars, whiteSpaceTokens);
        }


        /// <summary>
        /// Initialize using settings.
        /// </summary>
        /// <param name="text">Text to use.</param>
        public void Init(string text)
        {
            Init(text, '\\', new char[] { TICK, QUOTE }, new char[] { ' ', '\t' });
        }


        /// <summary>
        /// Initialize using settings.
        /// </summary>
        /// <param name="text">Text to use.</param>
        /// <param name="settings">Instance with token reader settings.</param>
        public void Init(string text, ScannerSettings settings)
        {
            Init(text, settings.EscapeChar, settings.Tokens, settings.WhiteSpaceTokens);
        }


        /// <summary>
        /// Initialize using the supplied parameters.
        /// </summary>
        /// <param name="text">Text to read.</param>
        /// <param name="escapeChar">Escape character.</param>
        /// <param name="tokens">Array with tokens.</param>
        /// <param name="whiteSpaceTokens">Array with whitespace tokens.</param>
        public void Init(string text, char escapeChar, char[] tokens, char[] whiteSpaceTokens)
        {
            Reset();
            _text = text;
            LAST_POSITION = _text.Length - 1;
            _escapeChar = escapeChar;
            _tokens = ToDictionary(tokens);
            _whiteSpaceChars = ToDictionary(whiteSpaceTokens);
            _readonlyWhiteSpaceMap = new Dictionary<char, char>(_whiteSpaceChars);
        }


        #region IScanner Members
        /// <summary>
        /// The current position.
        /// </summary>
        public int Position
        {
            get { return _pos; }
        }


        /// <summary>
        /// The text being scanned.
        /// </summary>
        public string Text
        {
            get { return _text; }
        }


        /// <summary>
        /// Store the white space chars.
        /// </summary>
        /// <param name="whitespaceChars">Dictionary with whitespace characters.</param>
        public void RegisterWhiteSpace(IDictionary<char, char> whitespaceChars)
        {
            _whiteSpaceChars = whitespaceChars;
        }


        /// <summary>
        /// Reset reader for parsing again.
        /// </summary>
        public void Reset()
        {
            _pos = -1;
            _text = string.Empty;
            //_backBuffer = new StringBuilder();
            _whiteSpaceChars = new Dictionary<char, char>();
            _tokens = new Dictionary<char, char>();
            _escapeChar = '\\';
        }


        /// <summary>
        /// Returns the char at current position + 1.
        /// </summary>
        /// <returns>Next char or string.empty if end of text.</returns>
        public char PeekChar()
        {
            // Validate.
            // a b c d e
            // 0 1 2 3 4
            // Lenght = 5
            // 4 >= 5 - 1
            if (_pos >= _text.Length - 1)
                return char.MinValue;

            _nextChar = _text[_pos + 1];
            return _nextChar;
        }


        /// <summary>
        /// Returns the chars starting at current position + 1 and
        /// including the <paramref name="count"/> number of characters.
        /// </summary>
        /// <param name="count">Number of characters.</param>
        /// <returns>Range of chars as string or string.empty if end of text.</returns>
        public string PeekChars(int count)
        {
            // Validate.
            if (_pos + count > _text.Length)
                return string.Empty;

            return _text.Substring(_pos + 1, count);
        }


        /// <summary>
        /// Returns the nth char from the current char index
        /// </summary>
        /// <param name="countFromCurrentCharIndex">Number of characters from the current char index</param>
        /// <returns>Single char as string</returns>
        public char PeekChar(int countFromCurrentCharIndex)
        {
            // Validate.
            if (_pos + countFromCurrentCharIndex > _text.Length - 1)
                return END_CHAR;

            return _text[_pos + countFromCurrentCharIndex];
        }


        /// <summary>
        /// Peeks at the next character from the current char index.
        /// </summary>
        /// <returns>Single char</returns>
        public char PeekNextChar()
        {
            return PeekChar(1);
        }


        /// <summary>
        /// Whether of not the current character is an escaped quote.
        /// </summary>
        /// <returns></returns>
        public bool IsEscapedQuote()
        {
            return _currentChar == QUOTE && PeekNextChar() == QUOTE;
        }


        /// <summary>
        /// Peeks at the string all the way until the end of line.
        /// </summary>
        /// <returns>Current line.</returns>
        public string PeekLine()
        {
            int ndxEol = _text.IndexOf(Environment.NewLine, _pos + 1);
            if (ndxEol == -1)
                return _text.Substring(_pos + 1);

            return _text.Substring(_pos + 1, (ndxEol - _pos));
        }


        /// <summary>
        /// Advance and consume the current current char without storing 
        /// the char in the additional buffer for undo.
        /// </summary>
        public void ConsumeChar()
        {
            _pos++;
        }


        /// <summary>
        /// Consume the next <paramref name="count"/> chars without
        /// storing them in the additional buffer for undo.
        /// </summary>
        /// <param name="count">Number of characters to consume.</param>
        public void ConsumeChars(int count)
        {
            _pos += count;
        }


        /// <summary>
        /// Consume the whitespace without reading anything.
        /// </summary>
        public void ConsumeWhiteSpace()
        {
            ConsumeWhiteSpace(false);
        }


        /// <summary>
        /// Consume all white space.
        /// This works by checking the next char against
        /// the chars in the dictionary of chars supplied during initialization.
        /// </summary>
        /// <param name="readFirst">True to read a character
        /// before consuming the whitespace.</param>
        public void ConsumeWhiteSpace(bool readFirst)
        {
            char currentChar = readFirst ? ReadChar() : _currentChar;

            while (!IsEnd() && _whiteSpaceChars.ContainsKey(currentChar))
            {
                // Advance reader and next char.
                ReadChar();
                currentChar = _currentChar;
            }
        }


        /// <summary>
        /// Consume all white space.
        /// This works by checking the next char against
        /// the chars in the dictionary of chars supplied during initialization.
        /// </summary>
        /// <param name="readFirst">True to read a character
        /// before consuming the whitepsace.</param>
        /// <param name="setPosAfterWhiteSpace">True to move position to after whitespace</param>
        public void ConsumeWhiteSpace(bool readFirst, bool setPosAfterWhiteSpace = true)
        {
            if (readFirst) ReadChar();

            bool matched = false;
            while (_pos <= LAST_POSITION)
            {
                if (!_whiteSpaceChars.ContainsKey(_currentChar))
                {
                    matched = true;
                    break;
                }
                ReadChar();
            }

            // At this point the pos is already after token.
            // If matched and need to set at end of token, move back 1 char
            if (matched && !setPosAfterWhiteSpace) MoveChars(-1);
        }


        /// <summary>
        /// Consume new line.
        /// </summary>
        public void ConsumeNewLine()
        {
            // Check 
            if (_currentChar == '\r' && PeekChar() == '\n')
            {
                // Move to \n in \r\n
                ReadChar();
                ReadChar();
                return;
            }

            // Just \n
            if (_currentChar == '\n')
                ReadChar();
        }


        /// <summary>
        /// Read text up to the eol.
        /// </summary>
        /// <returns>String read.</returns>
        public void ConsumeToNewLine(bool includeNewLine = false)
        {
            // Read until ":" colon and while not end of string.
            while (!IsEol() && _pos <= LAST_POSITION)
            {
                MoveChars(1);
            }
            if (includeNewLine) ConsumeNewLine();
        }


        /// <summary>
        /// Consume until the chars found.
        /// </summary>
        /// <param name="pattern">The pattern to consume chars to.</param>
        /// <param name="includePatternInConsumption">Wether or not to consume the pattern as well.</param>
        /// <param name="movePastEndOfPattern">Whether or not to move to the ending position of the pattern</param>
        /// <param name="moveToStartOfPattern">Whether or not to move to the starting position of the pattern</param>
        public bool ConsumeUntil(string pattern, bool includePatternInConsumption, bool moveToStartOfPattern, bool movePastEndOfPattern)
        {
            int ndx = _text.IndexOf(pattern, _pos);
            if (ndx == -1) return false;
            int newCharPos = 0;

            if (!includePatternInConsumption)
                newCharPos = moveToStartOfPattern ? ndx : ndx - 1;
            else
                newCharPos = movePastEndOfPattern ? ndx + pattern.Length : (ndx + pattern.Length) - 1;

            MoveChars(newCharPos - _pos);
            return true;
        }


        /// <summary>
        /// Consume New Lines.
        /// </summary>
        public void ConsumeNewLines()
        {
            string combinedNewLine = _currentChar.ToString() + PeekChar();
            while (_currentChar == '\n' || combinedNewLine == "\r\n" && _pos != LAST_POSITION)
            {
                ConsumeNewLine();
                combinedNewLine = _currentChar.ToString() + PeekChar();
            }
        }


        /// <summary>
        /// Moves forward by count chars.
        /// </summary>
        /// <param name="count"></param>
        public void MoveChars(int count)
        {
            // Pos can never be more than 1 + last index position.
            // e.g. "common"
            // 1. length = 6
            // 2. LAST_POSITION = 5;
            // 3. _pos can not be more than 6. 6 indicating that it's past end
            // 4. _pos == 5 Indicating it's at end.
            if (_pos > LAST_POSITION && count > 0) return;

            // Move past end? Move it just 1 position more than last index.
            if (_pos + count > LAST_POSITION)
            {
                _pos = LAST_POSITION + 1;
                _currentChar = END_CHAR;
                return;
            }

            // Can move forward count chars
            _pos += count;
            _currentChar = _text[_pos];
        }


        /// <summary>
        /// Read back the last char and reset
        /// </summary>
        public void ReadBackChar()
        {
            _pos--;
            //_backBuffer.Remove(_backBuffer.Length - 1, 1);
            _currentChar = _text[_pos];
        }


        /// <summary>
        /// Unwinds the reader by <paramref name="count"/> chars.
        /// </summary>
        /// <param name="count">Number of characters to read.</param>
        public void ReadBackChar(int count)
        {
            // Unwind            
            _pos -= count;
            //_backBuffer.Remove(_backBuffer.Length - count, count);
            _currentChar = _text[_pos];
        }


        /// <summary>
        /// Read the next char.
        /// </summary>
        /// <returns>Character read.</returns>
        public char ReadChar()
        {
            // NEVER GO PAST 1 INDEX POSITION AFTER CHAR
            if (_pos > LAST_POSITION) return END_CHAR;

            _pos++;

            // Still valid?
            if (_pos <= LAST_POSITION)
            {
                _currentChar = _text[_pos];
                return _currentChar;
            }
            _currentChar = END_CHAR;
            return END_CHAR;
        }


        /// <summary>
        /// Read the next <paramref name="count"/> number of chars.
        /// </summary>
        /// <param name="count">Number of characters to read.</param>
        /// <returns>Characters read.</returns>
        public string ReadChars(int count)
        {
            string text = _text.Substring(_pos + 1, count);
            _pos += count;
            _currentChar = _text[_pos];
            return text;
        }


        /// <summary>
        /// Read text up to the eol.
        /// </summary>
        /// <returns>String read.</returns>
        public string ReadToEol()
        {
            StringBuilder buffer = new StringBuilder();

            // Read until ":" colon and while not end of string.
            while (!IsEol() && _pos <= LAST_POSITION)
            {
                buffer.Append(_currentChar);
                ReadChar();
            }
            return buffer.ToString();
        }


        /// <summary>
        /// ReadToken until one of the endchars is found
        /// </summary>
        /// <param name="endChars">List of possible end chars which halts reading further.</param>
        /// <param name="includeEndChar">True to include end character.</param>
        /// <param name="advanceFirst">True to advance before reading.</param>
        /// <param name="readPastEndChar">True to read past the end character.</param>
        /// <returns></returns>
        public string ReadTokenUntil(char[] endChars, bool includeEndChar = false, bool advanceFirst = false, bool readPastEndChar = false)
        {
            var buffer = new StringBuilder();
            bool found = false;
            if (advanceFirst) ReadChar();

            while (_pos < LAST_POSITION && !found)
            {
                for (int ndx = 0; ndx < endChars.Length; ndx++)
                {
                    if (_currentChar == endChars[ndx])
                    {
                        found = true;
                        break;
                    }
                }
                if (!found || (found && includeEndChar))
                    buffer.Append(_currentChar);

                if (!found || (found && readPastEndChar))
                    ReadChar();
            }
            string token = buffer.ToString();
            return token;
        }


        /// <summary>
        /// Read a token.
        /// </summary>
        /// <param name="endChar">Ending character.</param>
        /// <param name="escapeChar">Escape character.</param>
        /// <param name="includeEndChar">True to include end character.</param>
        /// <param name="advanceFirst">True to advance before reading.</param>
        /// <param name="expectEndChar">True to expect an end charachter.</param>
        /// <param name="readPastEndChar">True to read past the end character.</param>
        /// <returns>Contens of token read.</returns>
        public string ReadToken(char endChar, char escapeChar, bool includeEndChar, bool advanceFirst, bool expectEndChar, bool readPastEndChar)
        {
            StringBuilder buffer = new StringBuilder();
            char currentChar = advanceFirst ? ReadChar() : _currentChar;

            // Read until ":" colon and while not end of string.
            while (currentChar != endChar && _pos <= LAST_POSITION)
            {
                // Escape char
                if (currentChar == escapeChar)
                {
                    currentChar = ReadChar();
                    buffer.Append(currentChar);
                }
                else
                    buffer.Append(currentChar);

                currentChar = ReadChar();
            }
            bool matchedEndChar = true;

            // Error out if current char is not ":".
            if (expectEndChar && _currentChar != endChar)
            {
                _errors.Add("Expected " + endChar + " at : " + _pos);
                matchedEndChar = false;
            }

            // Read past char.
            if (matchedEndChar && readPastEndChar)
                ReadChar();

            return buffer.ToString();
        }


        #region New API
        /// <summary>
        /// Read token until endchar
        /// </summary>
        /// <param name="quoteChar">char representing quote ' or "</param>
        /// <param name="escapeChar">Escape character for quote within string.</param>
        /// <param name="advanceFirst">True to advance position first before reading string.</param>
        /// <param name="setPosAfterToken">True to move position to end quote, otherwise past end quote.</param>
        /// <returns>Contents of token read.</returns>
        public ScanTokenResult ReadString(char quoteChar, char escapeChar = '\\', bool advanceFirst = true, bool setPosAfterToken = true)
        {
            // "name" 'name' "name\"s" 'name\'"
            var buffer = new StringBuilder();
            var curr = advanceFirst ? ReadChar() : _currentChar;
            var next = PeekChar();
            bool matched = false;
            while (_pos <= LAST_POSITION)
            {
                // Escape char
                if (curr == escapeChar)
                {
                    curr = ReadChar();
                    buffer.Append(curr);
                }
                else if (curr == quoteChar)
                {
                    matched = true;
                    MoveChars(1);
                    break;
                }
                else buffer.Append(curr);

                curr = ReadChar(); next = PeekChar();
            }
            // At this point the pos is already after token.
            // If matched and need to set at end of token, move back 1 char
            if (matched && !setPosAfterToken) MoveChars(-1);

            return new ScanTokenResult(matched, buffer.ToString());
        }


        /// <summary>
        /// Read token until endchar
        /// </summary>
        /// <param name="quoteChar">char representing quote ' or "</param>
        /// <param name="escapeChar">Escape character for quote within string.</param>
        /// <param name="advanceFirst">True to advance position first before reading string.</param>
        /// <param name="setPosAfterToken">True to move position to end quote, otherwise past end quote.</param>
        /// <returns>Contents of token read.</returns>
        public ScanTokenResult ReadCodeString(char quoteChar, char escapeChar = '\\', bool advanceFirst = true, bool setPosAfterToken = true)
        {
            // "name" 'name' "name\"s" 'name\'"
            var buffer = new StringBuilder();
            char curr = advanceFirst ? ReadChar() : _currentChar;
            char next = PeekChar();
            bool matched = false;
            while (_pos <= LAST_POSITION)
            {
                // End string " or '
                if (curr == quoteChar)
                {
                    matched = true;
                    MoveChars(1);
                    break;
                }
                // Not an \ for escaping so just append.
                else if (curr != escapeChar)
                {
                    buffer.Append(curr);
                }
                // Escape \
                else if (curr == escapeChar)
                {
                    if (next == quoteChar) buffer.Append("\"");
                    else if (next == 'r') buffer.Append('\r');
                    else if (next == 'n') buffer.Append('\n');
                    else if (next == 't') buffer.Append('\t');
                    MoveChars(1);
                }

                curr = ReadChar(); next = PeekChar();
            }
            // At this point the pos is already after token.
            // If matched and need to set at end of token, move back 1 char
            if (matched && !setPosAfterToken) MoveChars(-1);

            return new ScanTokenResult(matched, buffer.ToString());
        }


        /// <summary>
        /// Reads a word which must not have space in it and must have space/tab before and after
        /// </summary>
        /// <param name="advanceFirst">Whether or not to advance position first</param>
        /// <param name="setPosAfterToken">True to move position to end space, otherwise past end space.</param>
        /// <returns>Contents of token read.</returns>
        public ScanTokenResult ReadWord(bool advanceFirst, bool setPosAfterToken = true)
        {
            return ReadChars(() => _currentChar != ' ' && _currentChar != '\t', advanceFirst, setPosAfterToken);
        }


        /// <summary>
        /// Reads an identifier where legal chars for the identifier are [$ . _ a-z A-Z 0-9]
        /// </summary>
        /// <param name="advanceFirst"></param>
        /// <param name="setPosAfterToken">True to move position to after id, otherwise 2 chars past</param>
        /// <returns></returns>
        public ScanTokenResult ReadId(bool advanceFirst, bool setPosAfterToken = true)
        {
            return ReadChars(() =>
            {
                return ('a' <= _currentChar && _currentChar <= 'z')
                    || ('A' <= _currentChar && _currentChar <= 'Z')
                    || _currentChar == '$' || _currentChar == '_'
                    || ('0' <= _currentChar && _currentChar <= '9');
            }, advanceFirst, setPosAfterToken);
        }


        /// <summary>
        /// Reads a number +/-?[0-9]*.?[0-9]*
        /// </summary>
        /// <param name="advanceFirst">Whether or not to advance position first</param>
        /// <param name="setPosAfterToken">True to move position to end space, otherwise past end space.</param>
        /// <returns>Contents of token read.</returns>
        public ScanTokenResult ReadNumber(bool advanceFirst, bool setPosAfterToken = true)
        {
            string sign = "";
            if (advanceFirst) ReadChar();
            if (_currentChar == '+' || _currentChar == '-') { sign = _currentChar.ToString(); ReadChar(); }

            var result = ReadChars(() => { return ('0' <= _currentChar && _currentChar <= '9' || _currentChar == '.'); }, false, setPosAfterToken);
            var finalresult = new ScanTokenResult(result.Success, sign + result.Text);
            finalresult.Lines = result.Lines;
            return finalresult;
        }


        /// <summary>
        /// Reads entire line from curr position
        /// </summary>
        /// <param name="advanceFirst">Whether or not to advance curr position first </param>
        /// <param name="setPosAfterToken">Whether or not to move curr position to starting of new line or after</param>
        /// <returns>String read.</returns>
        public ScanTokenResult ReadLine(bool advanceFirst, bool setPosAfterToken = true)
        {
            var result = ReadChars(() => !(_currentChar == '\r' && _nextChar == '\n'), advanceFirst, setPosAfterToken);
            if (setPosAfterToken)
                MoveChars(2);
            return result;
        }


        /// <summary>
        /// Reads until the 2 chars are reached.
        /// </summary>
        /// <param name="advanceFirst">Whether or not to advance curr position first </param>
        /// <param name="first">The first char expected</param>
        /// <param name="second">The second char expected</param>
        /// <param name="setPosAfterToken">Whether or not to advance to position after chars</param>
        /// <returns>String read.</returns>
        public ScanTokenResult ReadUntilChars(bool advanceFirst, char first, char second, bool setPosAfterToken = true)
        {
            var result = ReadChars(() => !(_currentChar == first && _nextChar == second), advanceFirst, setPosAfterToken);
            if (setPosAfterToken)
                MoveChars(2);
            return result;
        }


        /// <summary>
        /// Reads until the 2 chars are reached.
        /// </summary>
        /// <param name="advanceFirst">Whether or not to advance curr position first </param>
        /// <param name="first">The first char expected</param>
        /// <param name="second">The second char expected</param>
        /// <param name="moveToEndChar">Whether or not to advance to last end char ( second char ) or move past it</param>
        /// <returns>String read.</returns>
        public ScanTokenResult ReadLinesUntilChars(bool advanceFirst, char first, char second, bool moveToEndChar = true)
        {
            int lineCount = 0;
            return ReadChars(() =>
            {
                // Keep track of lines meet
                if (_currentChar == '\r' && _nextChar == '\n') lineCount++;

                return !(_currentChar == first && _nextChar == second);
            }, advanceFirst, moveToEndChar);
        }
        #endregion


        /// <summary>
        /// Read token until endchar
        /// </summary>
        /// <param name="endChar">Ending character.</param>
        /// <param name="escapeChar">Escape character.</param>
        /// <param name="advanceFirst">True to advance before reading.</param>
        /// <param name="expectEndChar">True to expect an end charachter.</param>
        /// <param name="includeEndChar">True to include an end character.</param>
        /// <param name="moveToEndChar">True to move to the end character.</param>
        /// <param name="readPastEndChar">True to read past the end character.</param>
        /// <returns>Contents of token read.</returns>
        public string ReadTokenUntil(char endChar, char escapeChar, bool advanceFirst, bool expectEndChar, bool includeEndChar, bool moveToEndChar, bool readPastEndChar)
        {
            // abcd <div>
            var buffer = new StringBuilder();
            var currentChar = advanceFirst ? ReadChar() : _currentChar;
            var nextChar = PeekChar();
            while (nextChar != endChar && _pos <= LAST_POSITION)
            {
                // Escape char
                if (currentChar == escapeChar)
                {
                    currentChar = ReadChar();
                    buffer.Append(currentChar);
                }
                else
                    buffer.Append(currentChar);

                currentChar = ReadChar();
                nextChar = PeekChar();
            }
            bool matchedEndChar = nextChar == endChar;
            if (expectEndChar && !matchedEndChar)
                _errors.Add("Expected " + endChar + " at : " + _pos);

            if (matchedEndChar)
            {
                buffer.Append(currentChar);
                if (includeEndChar)
                    buffer.Append(nextChar);

                if (moveToEndChar)
                    ReadChar();

                else if (readPastEndChar && !IsAtEnd())
                    ReadChars(2);
            }

            return buffer.ToString();
        }


        /// <summary>
        /// Determines whether the current character is the expected one.
        /// </summary>
        /// <param name="charToExpect">Character to expect.</param>
        /// <returns>True if the current character is the expected one.</returns>
        public bool Expect(char charToExpect)
        {
            bool isMatch = _currentChar == charToExpect;
            if (!isMatch)
                _errors.Add("Expected " + charToExpect + " at : " + _pos);
            return isMatch;
        }


        /// <summary>
        /// Current char.
        /// </summary>
        /// <returns>Current character.</returns>
        public char CurrentChar
        {
            get { return _currentChar; }
        }


        /// <summary>
        /// Get the previous char that was read in.
        /// </summary>
        public char PreviousChar
        {
            get
            {
                // Check.
                if (_pos <= 0)
                    return char.MinValue;

                // Get the last char from the back buffer.
                // This is the last valid char that is not escaped.
                return _text[_pos - 1];
            }
        }


        /// <summary>
        /// Get the previous char that is part of the input and which may be an escape char.
        /// </summary>
        public string PreviousCharAny
        {
            get
            {
                // Check.
                if (_pos <= 0)
                    return string.Empty;

                // Get the last char from the back buffer.
                // This is the last valid char that is not escaped.
                return _text[_pos - 1].ToString();
            }
        }


        /// <summary>
        /// Current position in text.
        /// </summary>
        /// <returns>Current character index.</returns>
        public int CurrentCharIndex()
        {
            return _pos;
        }


        /// <summary>
        /// Determine if current char is token.
        /// </summary>
        /// <returns>True if the current char is a token.</returns>
        public bool IsToken()
        {
            return _tokens.ContainsKey(_currentChar);
        }


        /// <summary>
        /// Whether or not the current sequence of chars matches the token supplied.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ignoreCase">Whether or not to check against case.</param>
        /// <returns></returns>
        public bool IsToken(string token, bool ignoreCase = false)
        {
            return string.Compare(_text, _pos, token, 0, token.Length, ignoreCase) == 0;
        }


        /// <summary>
        /// Determine if current char is escape char.
        /// </summary>
        /// <returns>True if the current char is an escape char.</returns>
        public bool IsEscape()
        {
            return (_currentChar == _escapeChar || IsEscapedQuote());
        }


        /// <summary>
        /// Determine if the end of the text input has been reached.
        /// </summary>
        /// <returns>True if the end of the stream has been reached.</returns>
        public bool IsEnd()
        {
            return _pos >= _text.Length;
        }


        /// <summary>
        /// Determine if at last char.
        /// </summary>
        /// <returns>True if the last character is the current character.</returns>
        public bool IsAtEnd()
        {
            return _pos == _text.Length - 1;
        }


        /// <summary>
        /// Determine if current char is EOL.
        /// </summary>
        /// <returns>True if the current character is an eol.</returns>
        public bool IsEol()
        {
            // Check for "\r\n"
            if (_currentChar == '\r' && PeekChar() == '\n')
                return true;

            return false;
        }


        /// <summary>
        /// Determine if current char is whitespace.
        /// </summary>
        /// <param name="whitespaceChars">Dictionary with whitespace chars.</param>
        /// <returns>True if the current character is a whitespace.</returns>
        public bool IsWhiteSpace(IDictionary whitespaceChars)
        {
            return whitespaceChars.Contains(_currentChar);
        }


        /// <summary>
        /// Determine if current char is whitespace.
        /// </summary>
        /// <returns>True if the current character is a whitespace.</returns>
        public bool IsWhiteSpace()
        {
            return this._whiteSpaceChars.ContainsKey(_currentChar);
        }
        #endregion


        #region Private methods
        private void Init(IDictionary<string, bool> tokens, string[] tokenList)
        {
            foreach (string token in tokenList)
            {
                tokens[token] = true;
            }
        }


        /// <summary>
        /// Reads a word which must not have space in it and must have space/tab before and after
        /// </summary>
        /// <param name="continueReadCheck">Callback function to determine whether or not to continue reading</param>
        /// <param name="advanceFirst">Whether or not to advance position first</param>
        /// <param name="setPosAfterToken">True to move position to end space, otherwise past end space.</param>
        /// <returns>Contents of token read.</returns>
        public ScanTokenResult ReadChars(Func<bool> continueReadCheck, bool advanceFirst, bool setPosAfterToken = true)
        {
            // while for function
            var buffer = new StringBuilder();
            if (advanceFirst) ReadChar();

            bool matched = false;
            bool valid = true;
            while (_pos <= LAST_POSITION)
            {
                if (continueReadCheck())
                    buffer.Append(_currentChar);
                else
                {
                    matched = true;
                    valid = false;
                    break;
                }
                ReadChar();
                if (_pos < LAST_POSITION)
                    _nextChar = _text[_pos + 1];
            }
            // Either 
            // 1. Matched the token
            // 2. Did not match but valid && end_of_file
            bool success = matched || (valid && _pos > LAST_POSITION);

            // At this point the pos is already after token.
            // If matched and need to set at end of token, move back 1 char
            if (success && !setPosAfterToken) MoveChars(-1);

            return new ScanTokenResult(success, buffer.ToString());
        }


        /// <summary>
        /// Reads a word which must not have space in it and must have space/tab before and after
        /// </summary>
        /// <param name="validChars">Dictionary to check against valid chars.</param>
        /// <param name="advanceFirst">Whether or not to advance position first</param>
        /// <param name="setPosAfterToken">True to move position to end space, otherwise past end space.</param>
        /// <returns>Contents of token read.</returns>
        public ScanTokenResult ReadChars(IDictionary<char, bool> validChars, bool advanceFirst, bool setPosAfterToken = true)
        {
            // while for function
            var buffer = new StringBuilder();
            if (advanceFirst) ReadChar();

            bool matched = false;
            bool valid = true;
            while (_pos <= LAST_POSITION)
            {
                if (validChars.ContainsKey(_currentChar))
                    buffer.Append(_currentChar);
                else
                {
                    matched = true;
                    valid = false;
                    break;
                }
                ReadChar();
                if (_pos < LAST_POSITION)
                    _nextChar = _text[_pos + 1];
            }

            // At this point the pos is already after token.
            // If matched and need to set at end of token, move back 1 char
            if (matched && !setPosAfterToken) MoveChars(-1);

            // Either 
            // 1. Matched the token
            // 2. Did not match but valid && end_of_file
            bool success = matched || (valid && _pos > LAST_POSITION);
            return new ScanTokenResult(success, buffer.ToString());
        }


        /// <summary>
        /// Check if all of the items in the collection satisfied by the condition.
        /// </summary>
        /// <typeparam name="T">Type of items.</typeparam>
        /// <param name="items">List of items.</param>
        /// <returns>Dictionary of items.</returns>
        public static IDictionary<T, T> ToDictionary<T>(IList<T> items)
        {
            IDictionary<T, T> dict = new Dictionary<T, T>();
            foreach (T item in items)
            {
                dict[item] = item;
            }
            return dict;
        }
        #endregion
    }
}
