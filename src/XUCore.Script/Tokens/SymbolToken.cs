using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// Operator as token
    /// </summary>
    internal class SymbolToken : Token
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="op"></param>
        public SymbolToken(string op)
        {
            this._text = op;
            this._isKeyword = false;
        }
    }
}
