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
    public class CompareExpression : Expression
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="left">Left hand expression</param>
        /// <param name="op">Operator</param>
        /// <param name="right">Right expression</param>
        public CompareExpression(Expression left, Operator op, Expression right)
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
            bool result = false;
            object left = Left.Evaluate();
            object right = Right.Evaluate();

            if (left is int && right is double)
                left = Convert.ToDouble(left);
            else if (left is double && right is int)
                right = Convert.ToDouble(right);
            
            // Both double
            if (left is double && right is double) 
                result = CompareNumbers((double)left, (double)right, Op);

            else if (left is int && right is int)  
                result = CompareNumbers(Convert.ToDouble(left), Convert.ToDouble(right), Op);

            // Both strings
            else if (left is string && right is string) 
                result = CompareStrings((string)left, (string)right, Op);

            // Both bools
            else if (left is bool && right is bool)
                result = CompareNumbers(Convert.ToDouble(left), Convert.ToDouble(right), Op);

            // Both dates
            else if (left is DateTime && right is DateTime)
                result = CompareDates((DateTime)left, (DateTime)right, Op);

            // 1 or both null
            else if (left == LNull.Instance || right == LNull.Instance)
                result = CompareNull(left, right, Op);

            return result;
        }


        private static bool CompareNull(object left, object right, Operator op)
        {
            // Both null
            if (left == LNull.Instance && right == LNull.Instance && op == Operator.EqualEqual) return true;
            if (left == LNull.Instance && right == LNull.Instance && op == Operator.NotEqual) return false;
            // Both not null
            if (left != LNull.Instance && right != LNull.Instance && op == Operator.EqualEqual) return left == right;
            if (left != LNull.Instance && right != LNull.Instance && op == Operator.NotEqual) return left != right;
            // Check for one
            if (op == Operator.NotEqual) return true;

            return false;
        }


        private static bool CompareNumbers(double left, double right, Operator op)
        {
            if (op == Operator.LessThan) return left < right;
            if (op == Operator.LessThanEqual) return left <= right;
            if (op == Operator.MoreThan) return left > right;            
            if (op == Operator.MoreThanEqual) return left >= right;            
            if (op == Operator.EqualEqual) return left == right;            
            if (op == Operator.NotEqual) return left != right;            
            return false;
        }


        private static bool CompareStrings(string left, string right, Operator op)
        {
            if (op == Operator.EqualEqual) return left == right;            
            if (op == Operator.NotEqual)   return left != right;            
            int compareResult = string.Compare(left, right);
            if (op == Operator.LessThan)      return compareResult == -1;
            if (op == Operator.LessThanEqual) return compareResult != 1;            
            if (op == Operator.MoreThan)      return compareResult == 1;
            if (op == Operator.MoreThanEqual) return compareResult != -1;

            return false;
        }


        private static bool CompareDates(DateTime left, DateTime right, Operator op)
        {
             if (op == Operator.LessThan)      return left < right;            
            if (op == Operator.LessThanEqual) return left <= right;
            if (op == Operator.MoreThan)      return left > right;
            if (op == Operator.MoreThanEqual) return left >= right;
            if (op == Operator.EqualEqual)    return left == right;            
            if (op == Operator.NotEqual)      return left != right;            
            return false;
        }
    }    
}
