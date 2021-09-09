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
    public class IfStatement : ConditionalBlockStatement
    {
        /// <summary>
        /// Create new instance
        /// </summary>
        public IfStatement() : base(null, null) { }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="condition"></param>
        public IfStatement(Expression condition)
            : base(condition, null)
        {
        }


        /// <summary>
        /// Else statement.
        /// </summary>
        public BlockStatement Else;



        /// <summary>
        /// Execute
        /// </summary>
        public override void Execute()
        {
            // Case 1: If is true
            if (Condition.EvaluateAs<bool>())
            {
                LangHelper.Execute(_statements, this);
            }
            // Case 2: Else available to execute
            else if (Else != null)
            {
                Else.Execute();
            }
        }
    }    
}
