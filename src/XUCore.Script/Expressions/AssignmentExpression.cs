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
    public class AssignmentExpression : Expression
    {
        private string Name;
        private Scope Scope;
        private Expression Exp;


        /// <summary>
        /// Initialize
        /// </summary>
        public AssignmentExpression()
        {
        }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="scope">Scope containing objects</param>
        /// <param name="exp">Expression to evaluate for value of variable</param>
        /// <param name="name">Variable name</param>
        public AssignmentExpression(string name, Expression exp, Scope scope)
        {
            this.Name = name;
            this.Scope = scope;
            this.Exp = exp;
        }
        

        /// <summary>
        /// Evaluate
        /// </summary>
        /// <returns></returns>
        public override object Evaluate()
        {
            object result = this.Exp.Evaluate();
            this.Scope.SetValue(this.Name, result);
            return result;
        }
    }
}
