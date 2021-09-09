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
    public class ContinueStatement : Statement
    {
        /// <summary>
        /// Execute the statement.
        /// </summary>
        public override void Execute()
        {
            var loop = this.FindParent<ILoop>();
            if (loop == null) throw new LangException("syntax error", "unable to break, loop not found", string.Empty, 0);

            loop.Continue();
        }
    }
}
