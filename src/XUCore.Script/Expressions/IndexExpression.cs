using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Reflection;


namespace XUCore.Script
{
    /// <summary>
    /// Member access expressions for "." property or "." method.
    /// </summary>
    public class IndexExpression : Expression
    {
       
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="variableExp">The variable expression to use instead of passing in name of variable.</param>
        /// <param name="indexExp">The expression representing the index value to get</param>
        /// <param name="isAssignment">Whether or not this is part of an assigment</param>
        public IndexExpression(Expression variableExp, Expression indexExp, bool isAssignment)
        {
            VariableExp = variableExp;
            IndexExp = indexExp;
            IsAssignment = isAssignment;
        }


        /// <summary>
        /// Expression representing the index
        /// </summary>
        public Expression IndexExp;


        /// <summary>
        /// The variable expression representing the list.
        /// </summary>
        public Expression VariableExp;


        /// <summary>
        /// The object to get the index value from. Used if ObjectName is null or empty.
        /// </summary>
        public object ListObject;


        /// <summary>
        /// Whether or not this member access is part of an assignment.
        /// </summary>
        public bool IsAssignment;


        /// <summary>
        /// Evaluate object[index]
        /// </summary>
        /// <returns></returns>
        public override object Evaluate()
        {
            object result = null;
            int ndx = IndexExp.EvaluateAs<int>();

            // Either get from scope or from exp.
            if (VariableExp is VariableExpression)
                ListObject = Ctx.Scope.Get<object>(((VariableExpression)VariableExp).Name);
            else
                ListObject = VariableExp.Evaluate();

            MethodInfo method = null;
            if(!this.IsAssignment)
            {
                // 1. Array
                if (ListObject is Array)
                    method = ListObject.GetType().GetMethod("GetValue", new Type[] { typeof(int) });
                // 2. LArray
                else if (ListObject is LArray)
                    method = ListObject.GetType().GetMethod("GetByIndex");
                // 3. IList
                else
                    method = ListObject.GetType().GetMethod("get_Item");

                // Getting value?
                result = method.Invoke(ListObject, new object[] { ndx });
                return result;            
            }
            return new Tuple<object, int>(ListObject, ndx);
        }     
    }    
}
