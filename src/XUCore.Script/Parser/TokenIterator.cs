using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// Iterates over a series of tokens.
    /// </summary>
    public class TokenIterator
    {
        private int _lastLineNumber;
        private int _lastCharPosition;


        /// <summary>
        /// The parsed tokens from the script
        /// </summary>
        public List<TokenData> TokenList;


        /// <summary>
        /// The index position of the currrent token being processed
        /// </summary>
        protected int CurrentIndex;


        /// <summary>
        /// Last token parsed.
        /// </summary>
        public TokenData LastToken;


        /// <summary>
        /// The next token
        /// </summary>
        public TokenData NextToken;



        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="tokens">The list of tokens</param>
        /// <param name="lastLineNumber">Last line number of script</param>
        /// <param name="lastCharPos">Last char position of script.</param>
        public void Init(List<TokenData> tokens, int lastLineNumber, int lastCharPos)
        {
            TokenList = tokens;
            CurrentIndex = -1;
            _lastCharPosition = lastCharPos;
            _lastLineNumber = lastLineNumber;
        }
        

        /// <summary>
        /// Advance to the next token
        /// </summary>
        public void Advance()
        {
            while (true)
            {
                LastToken = NextToken;

                // Gaurd against empty or going past the last token
                if (TokenList.Count == 0 || CurrentIndex + 1 >= TokenList.Count)
                    return;

                CurrentIndex++;
                NextToken = TokenList[CurrentIndex];
                if (NextToken.Token != Tokens.WhiteSpace && NextToken.Token != Tokens.CommentMLine
                    && NextToken.Token != Tokens.CommentSLine && NextToken.Token != Tokens.NewLine)
                    break;
            }
        }


        /// <summary>
        /// Peek into and get the token ahead of the current token.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public TokenData Peek(int count = 1)
        {
            if (CurrentIndex >= TokenList.Count - 1)
                return new TokenData() { Token = Token.EndToken, Line = _lastLineNumber, CharPos = _lastCharPosition };

            int ndx = CurrentIndex + 1;
            TokenData next = null;
            int advanced = 0;
            while (ndx < TokenList.Count - 1)
            {
                next = TokenList[ndx];
                if (next.Token != Tokens.WhiteSpace && next.Token != Tokens.CommentMLine
                    && next.Token != Tokens.CommentSLine && next.Token != Tokens.NewLine)
                    advanced++;

                if (advanced == count) break;
                ndx++;
            }
            return next;
        }
    }
}
