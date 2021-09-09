using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;


namespace XUCore.Script
{
    /// <summary>
    /// For loop Expression data
    /// </summary>
    public class ForEachStatement : LoopStatement
    {
        private string _varName;
        private string _sourceName;


        /// <summary>
        /// Initialize using the variable names.
        /// </summary>
        /// <param name="varname">Name of the variable in the loop</param>
        /// <param name="sourceName">Name of the variable containing the items to loop through.</param>
        public ForEachStatement(string varname, string sourceName) : base(null)
        {
            _varName = varname;
            _sourceName = sourceName;
        }


        /// <summary>
        /// Execute each expression.
        /// </summary>
        /// <returns></returns>
        public override void Execute()
        {
            _continueRunning = true;
            _breakLoop = false;
            _continueLoop = false;

            // for(user in users)
            // Push scope for var name 
            Ctx.Scope.Push();
            object source = Ctx.Scope.Get<object>(_sourceName);

            IEnumerator enumerator = null;
            if (source is LArray) enumerator = ((LArray)source).Raw.GetEnumerator();
            else if (source is IList) enumerator = ((IList)source).GetEnumerator();
            else if (source is Array) enumerator = ((Array)source).GetEnumerator();
            else if (source is LMap) enumerator = ((LMap)source).Raw.GetEnumerator();

            _continueRunning = enumerator.MoveNext();

            while (_continueRunning)
            {
                // Set the next value of "x" in for(x in y).
                Ctx.Scope.SetValue(_varName, enumerator.Current);

                if (_statements != null && _statements.Count > 0)
                {
                    foreach (var stmt in _statements)
                    {
                        stmt.Execute();

                        Ctx.Limits.CheckLoop(this);
                         
                        // If Break statment executed.
                        if (_breakLoop)
                        {
                            _continueRunning = false;
                            break;
                        }
                        // Continue statement.
                        else if (_continueLoop)
                            break;
                    }
                }
                else break;

                // Break loop here.
                if (_continueRunning == false)
                    break;

                // Increment.
                _continueRunning = enumerator.MoveNext();
            }
        }
    }    
}
