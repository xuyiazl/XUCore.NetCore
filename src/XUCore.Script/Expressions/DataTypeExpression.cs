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
    public class DataTypeExpression : Expression
    {
        private List<Expression> _arrayExpressions;
        private List<Tuple<string, Expression>> _mapExpressions;


        /// <summary>
        /// Initialize
        /// </summary>
        public DataTypeExpression(List<Expression> expressions)
        {
            // Used for lists/arrays
            _arrayExpressions = expressions;
        }


        /// <summary>
        /// Initialize
        /// </summary>
        public DataTypeExpression(List<Tuple<string, Expression>> expressions)
        {
            // Used for maps
            _mapExpressions = expressions;
        }


        /// <summary>
        /// Evaluate
        /// </summary>
        /// <returns></returns>
        public override object Evaluate()
        {
            // Case 1: array type
            if (_arrayExpressions != null)
            {
                List<object> items = new List<object>();

                foreach (var exp in _arrayExpressions)
                {
                    object result = exp == null ? null : exp.Evaluate();
                    items.Add(result);
                }
                LArray array = new LArray(items);
                return array;
            }

            // Case 2: Map type
            var dictionary = new Dictionary<string, object>();
            foreach (var pair in _mapExpressions)
            {
                var expression = pair.Item2;
                object result = expression == null ? null : expression.Evaluate();
                dictionary[pair.Item1] = result;
            }
            var map = new LMap(dictionary);
            return map;
        }


        /// <summary>
        /// Evaluate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override T EvaluateAs<T>()
        {
            object result = Evaluate();
            return (T)result;
        }
    }
}
