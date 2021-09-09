using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// Helper class for calling functions
    /// </summary>
    public class RegisteredFunctions
    {
        private Dictionary<string, FunctionStatement> _functions;


        /// <summary>
        /// Initialize
        /// </summary>
        public RegisteredFunctions()
        {
            _functions = new Dictionary<string, FunctionStatement>();
        }


        /// <summary>
        /// Registers a custom function callback.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="stmt">The function</param>
        public void Register(string pattern, FunctionStatement stmt)
        {
            _functions[pattern] = stmt;
        }

        
        /// <summary>
        /// Whether or not the function call supplied is a custom function callback that is 
        /// outside of the script.
        /// </summary>
        /// <param name="name">Name of the function</param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            return _functions.ContainsKey(name);
        }


        /// <summary>
        /// Get the custom function callback
        /// </summary>
        /// <param name="name">Name of the function</param>
        /// <returns></returns>
        public FunctionStatement GetByName(string name)
        {
            return _functions[name];
        }


        /// <summary>
        /// Calls the custom function.
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public object Call(FunctionCallExpression exp)
        {
            var function = GetByName(exp.Name);
            FunctionHelper.ResolveParameters(exp.ParamListExpressions, exp.ParamList);
            function.ArgumentValues = exp.ParamList;
            function.Execute();
            object result = null;
            if (function.HasReturnValue)
                result = function.ReturnValue;
            return result;
        }
    }
}
