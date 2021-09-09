using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// Token class
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Text of the token
        /// </summary>
        protected string _text;

        /// <summary>
        /// Whether or not this a keyword.
        /// </summary>
        protected bool _isKeyword;


        internal readonly static Token EndToken = new Token();        


        /// <summary>
        /// Text of the token
        /// </summary>
        public virtual string Text
        {
            get { return _text; }
        }


        /// <summary>
        /// Value of the token.
        /// </summary>
        public virtual object Value
        {
            get { return _text; }
        }


        /// <summary>
        /// Whether or not this ia keyword in the lang
        /// </summary>
        public virtual bool IsKeyWord
        {
            get { return _isKeyword; }
        }


        /// <summary>
        /// Replace the 
        /// </summary>
        /// <param name="text"></param>
        internal void Replace(string text)
        {
            _text = text;
        }
    }
}
