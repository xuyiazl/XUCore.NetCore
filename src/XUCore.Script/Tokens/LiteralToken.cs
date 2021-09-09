using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// String, number, bool(true/false), null
    /// </summary>
    internal class LiteralToken : Token
    {
        private object _value;


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="text">The raw text value</param>
        /// <param name="value">The actual value of the literal</param>
        /// <param name="isKeyword">Whether this is a keyword</param>
        public LiteralToken(string text, object value, bool isKeyword)
        {
            this._text = text;
            this._value = value;
            this._isKeyword = isKeyword;

        }


        /// <summary>
        /// Value of the literal
        /// </summary>
        public override object Value
        {
            get { return _value; }
        }
    }
}
