using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;


namespace XUCore.Script
{
    /// <summary>
    /// Variable expression data
    /// </summary>
    public class UnaryExpression : VariableExpression
    {
        private double Increment;
        private Operator Op;
        private Expression Expression;


        /// <summary>
        /// Initialize
        /// </summary>
        public UnaryExpression()
        {
        }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="incValue">Value to increment</param>
        /// <param name="op">The unary operator</param>
        /// <param name="name">Variable name</param>
        /// <param name="ctx">Context of the script</param>
        public UnaryExpression(string name, double incValue, Operator op, Context ctx)
        {
            this.Name = name;
            this.Op = op;
            this.Increment = incValue;
            this.Ctx = ctx;
        }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="exp">Expression representing value to increment by</param>
        /// <param name="op">The unary operator</param>
        /// <param name="name">Variable name</param>
        /// <param name="ctx">Context of the script</param>
        public UnaryExpression(string name, Expression exp, Operator op, Context ctx)
        {
            this.Name = name;
            this.Op = op;
            this.Expression = exp;
            this.Ctx = ctx;
        }
        

        /// <summary>
        /// Evaluate
        /// </summary>
        /// <returns></returns>
        public override object Evaluate()
        {
            // Logical not?
            if (Op == Operator.LogicalNot)
                return HandleLogicalNot();

            object valobj = this.Ctx.Scope.Get<object>(this.Name);

            // Double ? 
            if (valobj is double || valobj is int) return IncrementNumber(Convert.ToDouble(valobj));

            // String ?
            if (valobj is string) return IncrementString((string)valobj);

            throw new LangException("Syntax Error", "Unexpected operation", Ref.ScriptName, Ref.LineNumber, Ref.CharPosition);
        }


        private string IncrementString(string sourceVal)
        {
            if (Op != Operator.PlusEqual)
                throw new LangException("Syntax Error", "string operation with " + Op.ToString() + " not supported", Ref.ScriptName, Ref.LineNumber, Ref.CharPosition);

            this.DataType = typeof(string);
            string val = this.Expression.EvaluateAs<string>();

            // Check limit
            Ctx.Limits.CheckStringLength(this, sourceVal, val);

            string appended = sourceVal + val;
            this.Value = appended;
            this.Ctx.Scope.SetValue(this.Name, appended);
            return appended;
        }


        private double IncrementNumber(double val)
        {
            this.DataType = typeof(double);
            if (this.Expression != null)
                Increment = this.Expression.EvaluateAs<double>();
            else if (Increment == 0)
                Increment = 1;

            if (Op == Operator.PlusPlus)
            {
                val++;
            }
            else if (Op == Operator.MinusMinus)
            {
                val--;
            }
            else if (Op == Operator.PlusEqual)
            {
                val = val + Increment;
            }
            else if (Op == Operator.MinusEqual)
            {
                val = val - Increment;
            }
            else if (Op == Operator.MultEqual)
            {
                val = val * Increment;
            }
            else if (Op == Operator.DivEqual)
            {
                val = val / Increment;
            }            
            
            // Set the value back into scope
            this.Value = val;
            this.Ctx.Scope.SetValue(this.Name, val);

            return val;
        }


        private object HandleLogicalNot()
        {
            object result = this.Expression.Evaluate();
            if (result.GetType() == typeof(double))
                return false;
            if (result.GetType() == typeof(string))
                return false;
            if (result.GetType() == typeof(DateTime))
                return false;
            if (result.GetType() == typeof(bool))
                return !((bool)result);
            return false;
        }
    }
}
