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
    public class BinaryExpression : Expression
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="left">Left hand expression</param>
        /// <param name="op">Operator</param>
        /// <param name="right">Right expression</param>
        public BinaryExpression(Expression left, Operator op, Expression right)
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
        /// Operator * - / + % 
        /// </summary>
        public Operator Op;


        /// <summary>
        /// Right hand expression
        /// </summary>
        public Expression Right;


        /// <summary>
        /// Evaluate * / + - % 
        /// </summary>
        /// <returns></returns>
        public override object Evaluate()
        {
            // Validate
            object result = 0;
            object left = Left.Evaluate();
            object right = Right.Evaluate();

            // Both numbers
            if (left is double && right is double)
            {
                result = EvalNumbers((double)left, (double)right, Op);
            }
            else if (left is int && right is int)
            {
                result = EvalNumbers(Convert.ToDouble(left), Convert.ToDouble(right), Op);
            }
            // Both strings.
            else if (left is string && right is string)
            {
                string strleft = left.ToString();
                string strright = right.ToString();

                // Check string limit.
                Ctx.Limits.CheckStringLength(this, strleft, strright);

                result = strleft + strright;
            }
            // Double and Bool
            else if (left is double && right is bool)
            {
                bool r = (bool)right;
                double rval = r ? 1 : 0;
                result = EvalNumbers((double)left, rval, Op);
            }
            // Bool Double
            else if (left is bool && right is double)
            {
                bool l = (bool)left;
                double lval = l ? 1 : 0;
                result = EvalNumbers(lval, (double)right, Op);
            }
            // Append as strings.
            else if (left is string && right is bool)
            {
                result = left.ToString() + right.ToString().ToLower();
            }
            // Append as strings.
            else if (left is bool && right is string)
            {
                result = left.ToString().ToLower() + right.ToString();
            }
            else
            {
                result = left.ToString() + right.ToString();
            }
            return result;
        }


        private static double EvalNumbers(double left, double right, Operator op)
        {
            double result = 0;
            if (op == Operator.Multiply)
            {
                result = left * right;
            }
            else if (op == Operator.Divide)
            {
                result = left / right;
            }
            else if (op == Operator.Add)
            {
                result = left + right;
            }
            else if (op == Operator.Subtract)
            {
                result = left - right;
            }
            else if (op == Operator.Modulus)
            {
                result = left % right;
            }
            return result;
        }
    }    
}
