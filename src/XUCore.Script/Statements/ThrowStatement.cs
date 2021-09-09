using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;


namespace XUCore.Script
{
    /// <summary>
    /// For loop Expression data
    /// </summary>
    public class ThrowStatement : Statement
    {
        /// <summary>
        /// Create new instance
        /// </summary>
        public ThrowStatement()
        {
        }


        /// <summary>
        /// Name for the error in the catch clause.
        /// </summary>
        public Expression Exp;


        /// <summary>
        /// Execute
        /// </summary>
        public override void Execute()
        {
            object message = null;
            if (Exp != null)
                message = Exp.Evaluate();

            throw new LangException("TypeError", message.ToString(), this.Ref.ScriptName, this.Ref.LineNumber);
        }
    }    
}
