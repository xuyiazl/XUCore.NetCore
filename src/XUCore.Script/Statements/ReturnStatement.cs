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
    public class ReturnStatement : Statement
    {
        /// <summary>
        /// Return value.
        /// </summary>
        public Expression Exp;


        /// <summary>
        /// Execute the statement.
        /// </summary>
        public override void Execute()
        {
            var parent = this.FindParent<FunctionStatement>();
            if (parent == null) throw new LangException("syntax error", "unable to return, parent not found", string.Empty, 0);
            
            object result = Exp == null ? null : Exp.Evaluate();
            bool hasReturnVal = Exp != null;
            parent.Return(result, hasReturnVal);   
        }
    }
}
