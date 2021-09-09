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
    public class ForLoopExpression : Expression
    {
        private List<Expression> _expressions = null;


        /// <summary>
        /// Name of the variable
        /// </summary>
        public string Variable;


        /// <summary>
        /// Expression representing the start value.
        /// </summary>
        public string StartExpression;


        /// <summary>
        /// Operator for condition check in loop ndx >= 4
        /// </summary>
        public string CheckOp;


        /// <summary>
        /// Expression representing the bound value ndx > = 4 
        /// </summary>
        public string CheckExpression;


        /// <summary>
        /// The operator to increment by ++ or +=
        /// </summary>
        public string IncrementOp;


        /// <summary>
        /// The expression if not ++ but += then the number after +=
        /// </summary>
        public string IncrementExpression;


        /// <summary>
        /// Execute each expression.
        /// </summary>
        /// <returns></returns>
        public override object Evaluate()
        {
            // No Loop limit.
            foreach (var exp in _expressions)
            {                
                exp.Evaluate();                
            }
            return null;
        }
    }    
}
