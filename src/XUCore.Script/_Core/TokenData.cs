using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// Wraps a token with contextual information about it's script location.
    /// </summary>
    public class TokenData
    {
        /// <summary>
        /// The token
        /// </summary>
        public Token Token;


        /// <summary>
        /// Line number of the token
        /// </summary>
        public int Line { get; set; }


        /// <summary>
        /// Char position in the line of the token.
        /// </summary>
        public int CharPos { get; set; }


        /// <summary>
        /// String representation of tokendata.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {   
            string tokenType = Token.GetType().Name.Replace("Token", "");
            string info = string.Format("{0}, {1}, {2}, {3}, {4}", tokenType, Line, CharPos, Token.IsKeyWord, Token.Text);
            return info;
        }
    }
}
