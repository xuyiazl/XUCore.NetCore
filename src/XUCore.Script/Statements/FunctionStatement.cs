using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// Represents a function declaration
    /// </summary>
    public class FunctionStatement : BlockStatement
    {
        private bool _continueRunning;
        private object _result = null;
        private bool _hasReturnValue;


        /// <summary>
        /// Create new instance.
        /// </summary>
        public FunctionStatement() { }


        /// <summary>
        /// Create new instance with function name and argument names.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="argumentNames"></param>
        public FunctionStatement(string name, List<string> argumentNames)
        {
            Init(name, argumentNames);
        }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="name">name of function</param>
        /// <param name="argumentNames">names of the arguments.</param>
        public void Init(string name, List<string> argumentNames)
        {
            Name = name;
            Arguments = argumentNames;
        }


        /// <summary>
        /// Function declaration
        /// </summary>
        public string Name;


        /// <summary>
        /// List of arguments
        /// </summary>
        public List<string> Arguments;


        /// <summary>
        /// Values passed to the function.
        /// </summary>
        public List<object> ArgumentValues;


        /// <summary>
        /// The caller of the this function.
        /// </summary>
        public Expression Caller;

        /// <summary>
        /// Whether or not this function has arguments.
        /// </summary>
        public bool HasArguments { get { return Arguments != null && Arguments.Count > 0; } }


        /// <summary>
        /// Whether or not this function has a return value.
        /// </summary>
        public bool HasReturnValue { get { return _hasReturnValue; } }


        /// <summary>
        /// The return value;
        /// </summary>
        public object ReturnValue { get { return _result; } }


        /// <summary>
        /// Evaluate the function
        /// </summary>
        /// <returns></returns>
        public override void  Execute()
        {
            _continueRunning = true;
            _result = null;
            _hasReturnValue = false;
            PushScope();
            try
            {
                foreach (var statement in _statements)
                {
                    statement.Execute();
                    if (!_continueRunning) break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Ctx.Scope.Pop();
            }
        }


        /// <summary>
        /// set the return value.
        /// </summary>
        public void Return(object val, bool hasReturnValue)
        {
            _result = val;
            _hasReturnValue = hasReturnValue;
            _continueRunning = false;
        }


        private void PushScope()
        {
            Ctx.Scope.Push();
            if (this.ArgumentValues == null || this.ArgumentValues.Count == 0) return;
            if (this.Arguments == null || this.Arguments.Count == 0) return;
            if (this.ArgumentValues.Count > this.Arguments.Count)
                throw new ArgumentException("Invalid function call, more arguments passed than arguments in function: line " + Caller.Ref.LineNumber + ", pos: " + Caller.Ref.CharPosition);

            // Add function arguments to scope.
            for (int ndx = 0; ndx < this.ArgumentValues.Count; ndx++)
            {
                Ctx.Scope.SetValue(this.Arguments[ndx], this.ArgumentValues[ndx]);
            }
        }
    }
}
