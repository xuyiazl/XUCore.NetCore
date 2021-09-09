using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;


namespace XUCore.Script
{
    /// <summary>
    /// Condition expression less, less than equal, more, more than equal etc.
    /// </summary>
    public class ConditionExpression : Expression
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="left">Left hand expression</param>
        /// <param name="op">Operator</param>
        /// <param name="right">Right expression</param>
        public ConditionExpression(Expression left, Operator op, Expression right)
        {
            Left = left;
            Right = right;
            Op = op;
        }


        /// <summary>
        /// Left hand expression
        /// </summary>
        public Expression Left;


        /// <summary>
        /// Operator > >= == != less less than
        /// </summary>
        public Operator Op;


        /// <summary>
        /// Right hand expression
        /// </summary>
        public Expression Right;


        /// <summary>
        /// Evaluate > >= != == less less than
        /// </summary>
        /// <returns></returns>
        public override object Evaluate()
        {
            // Validate
            if (Op != Operator.And && Op != Operator.Or)
                throw new ArgumentException("Only && || supported");

            bool result = false;
            bool left = Left.EvaluateAs<bool>();
            bool right = Right.EvaluateAs<bool>();

            if (Op == Operator.Or)
            {
                result = left || right;
            }
            else if (Op == Operator.And)
            {
                result = left && right;
            }
            return result;
        }
    }    
}
