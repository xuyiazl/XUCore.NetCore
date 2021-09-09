﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{

    /// <summary>
    /// Expression to handle string interpolations such as "${username}'s email is ${email}".
    /// </summary>
    public class InterpolatedExpression : Expression
    {
        private List<Expression> _expressions;


        /// <summary>
        /// Adds an expression to be interpolated.
        /// </summary>
        /// <param name="exp"></param>
        public void Add(Expression exp)
        {
            if (_expressions == null)
                _expressions = new List<Expression>();

            _expressions.Add(exp);
        }


        /// <summary>
        /// Clears the expression.
        /// </summary>
        public void Clear()
        {
            if (_expressions != null)
                _expressions.Clear();
        }


        /// <summary>
        /// Evaluates the expression by appending all the sub-expressions.
        /// </summary>
        /// <returns></returns>
        public override object Evaluate()
        {
            if (_expressions == null || _expressions.Count == 0)
                return string.Empty;

            string total = "";
            foreach (var exp in _expressions)
            {
                if(exp != null)
                    total += exp.Evaluate().ToString();
            }
            return total;
        }
    }
}
