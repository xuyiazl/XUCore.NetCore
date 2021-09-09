using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;


namespace XUCore.Script
{
    /// <summary>
    /// Base class for Expressions
    /// </summary>
    public class Expression : AstNode
    {
        /// <summary>
        /// Evaluate
        /// </summary>
        /// <returns></returns>
        public virtual object Evaluate()
        {
            return null;
        }


        /// <summary>
        /// Evaluate and return as datatype T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T EvaluateAs<T>()
        {
            object result = Evaluate();
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}
