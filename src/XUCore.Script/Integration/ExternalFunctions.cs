using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// Helper class for calling functions
    /// </summary>
    public class ExternalFunctions
    {
        private Dictionary<string, Func<FunctionCallExpression, object>> _customCallbacks;


        /// <summary>
        /// Initialize
        /// </summary>
        public ExternalFunctions()
        {
            _customCallbacks = new Dictionary<string, Func<FunctionCallExpression, object>>();
        }


        /// <summary>
        /// Registers a custom function callback.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="callback">The custom callback</param>
        public void Register(string pattern, Func<FunctionCallExpression, object> callback)
        {
            _customCallbacks[pattern] = callback;
        }

        
        /// <summary>
        /// Whether or not the function call supplied is a custom function callback that is 
        /// outside of the script.
        /// </summary>
        /// <param name="name">Name of the function</param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            var callback = GetByName(name);
            return callback != null;
        }


        /// <summary>
        /// Get the custom function callback
        /// </summary>
        /// <param name="name">Name of the function</param>
        /// <returns></returns>
        public Func<FunctionCallExpression, object> GetByName(string name)
        {
            // Contains callback for full function name ? e.g. CreateUser
            if (_customCallbacks.ContainsKey(name))
                return _customCallbacks[name];
                    

            // Contains callback that handles multiple methods on a "object".
            // e.g. Blog.Create, Blog.Delete etc.
            if (name.Contains("."))
            {
                string prefix = name.Substring(0, name.IndexOf("."));
                if (_customCallbacks.ContainsKey(prefix + ".*"))
                    return _customCallbacks[prefix + ".*"];
            }
            return null;
        }


        /// <summary>
        /// Calls the custom function.
        /// </summary>
        /// <param name="name">Name of the function</param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public object Call(string name, FunctionCallExpression exp)
        {
            var callback = GetByName(name);
            FunctionHelper.ResolveParameters(exp.ParamListExpressions, exp.ParamList);
            object result = callback(exp);
            return result;
        }
    }
}
