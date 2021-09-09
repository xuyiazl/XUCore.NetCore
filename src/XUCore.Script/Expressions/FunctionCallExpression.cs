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
    /// Function call expression data.
    /// </summary>
    public class FunctionCallExpression : Expression, IParameterExpression
    {
        /// <summary>
        /// Function call expression
        /// </summary>
        public FunctionCallExpression()
        {
            ParamList = new List<object>();
            ParamListExpressions = new List<Expression>();
        }


        /// <summary>
        /// Handle method calls.
        /// </summary>
        /// <param name="scope">Variable scope of the script</param>
        /// <param name="isScopeVariable">Whether or not the variable name is a scope variable.</param>
        /// <param name="name">Name of the variable</param>
        /// <param name="method">Name of the method on the variable to call</param>
        /// <param name="args">Arguments to the method</param>
        public FunctionCallExpression(Scope scope, bool isScopeVariable, string name, string method, List<object> args)
        {
            Scope = scope;
            _isScopeVariable = isScopeVariable;
            _name = name;
            _member = method;
            ParamList = args;
        }


        /// <summary>
        /// Expression represnting the name of the function call.
        /// </summary>
        public Expression NameExp;


        /// <summary>
        /// List of expressions.
        /// </summary>
        public List<Expression> ParamListExpressions { get; set; }


        /// <summary>
        /// List of arguments.
        /// </summary>
        public List<object> ParamList { get; set; }


        /// <summary>
        /// Arguments to the function.
        /// </summary>
        public IDictionary ParamMap;


        /// <summary>
        /// Scope of variables.
        /// </summary>
        public Scope Scope;


        /// <summary>
        /// Whether or not this is a method call or a member access.
        /// </summary>
        public bool IsScopeVariable { get { return _isScopeVariable; } set { _isScopeVariable = value; } }


        /// <summary>
        /// Evauate and run the function
        /// </summary>
        /// <returns></returns>
        public override object Evaluate()
        {
            object result = null;
            bool called = false;
            
            // CASE 1: object . method ( "user" . "create" ) passed in explicity
            if (!string.IsNullOrEmpty(_name) && _isScopeVariable)
            {
                var obj = Ctx.Scope.Get<object>(_name);
                Ctx.State.Stack.Push(_name, this);
                called = true;
                result = FunctionHelper.Call(Ctx, obj, _name, _member, null, this.ParamListExpressions, this.ParamList);
            }
            // CASE 2: Exp is variable -> internal/external script. "getuser()".            
            else if (NameExp is VariableExpression)
            {
                string name = ((VariableExpression)NameExp).Name;
                Ctx.State.Stack.Push(name, this);
                called = true;
                result = FunctionHelper.Call(Ctx, name, this);
            }
            // Remove current function call.
            if (called)
            {
                Ctx.State.Stack.Pop();
                return result;
            }

            object member = this.NameExp.Evaluate();            
            // CASE 3: object "." method call from script is a external/internal function
            if (member is string)
            {
                Ctx.State.Stack.Push((string)member, this);
                called = true;
                result = FunctionHelper.Call(Ctx, member as string, this);
            }
            // CASE 4: string method call
            else if (member is Tuple<LString, string, string>)
            {
                var sinfo = member as Tuple<LString, string, string>;
                Ctx.State.Stack.Push((string)sinfo.Item3, this);
                called = true;
                result = FunctionHelper.Call(Ctx, sinfo.Item2, "", sinfo.Item3, null, this.ParamListExpressions, this.ParamList);
            }
            // CASE 5: date method call
            else if (member is Tuple<LDate, string, object, string>)
            {
                var dinfo = member as Tuple<LDate, string,  object, string>;
                Ctx.State.Stack.Push((string)dinfo.Item4, this);
                called = true;
                result = FunctionHelper.Call(Ctx, dinfo.Item3, dinfo.Item2, dinfo.Item4, null, this.ParamListExpressions, this.ParamList);
            }
            // CASE 6: Static method call "Person.Create"
            else if (member is Tuple<string, string, MethodInfo>)
            {
                var callInfo = member as Tuple<string, string, MethodInfo>;
                Ctx.State.Stack.Push(callInfo.Item1 + "." + callInfo.Item2, this);
                called = true;
                result = FunctionHelper.MethodCall(Ctx, null, null, callInfo.Item3, this.ParamListExpressions, this.ParamList);
            }
            // CASE 6: object "." method call from script is a member access.
            else if (member is Tuple<object, string, string, MethodInfo>)
            {
                var callInfo = member as Tuple<object, string, string, MethodInfo>;
                Ctx.State.Stack.Push(callInfo.Item2 + "." + callInfo.Item4, this);
                called = true;
                result = FunctionHelper.Call(Ctx, callInfo, this.ParamListExpressions, this.ParamList);
            }
            // Remove current function call.
            if(called) Ctx.State.Stack.Pop();

            return result;
        }


        private bool _isScopeVariable;
        private string _name;
        private string _member;
        /// <summary>
        /// Get the name of the function.
        /// </summary>
        public string Name
        {
            get
            {
                if (_name != null)
                    return _name;

                if (NameExp is VariableExpression)
                    return ((VariableExpression)NameExp).Name;

                object name = NameExp.Evaluate();
                return (string)name;
            }
            set
            {
                _name = value;
                if (_name.Contains("."))
                {
                    int ndxDot = _name.IndexOf(".");
                    _member = _name.Substring(ndxDot + 1); 
                    _name = _name.Substring(0, ndxDot);                    
                }
                _isScopeVariable = true;
            }
        }
    }
}
