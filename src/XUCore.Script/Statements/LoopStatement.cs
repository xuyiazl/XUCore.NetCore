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
    public class LoopStatement : ConditionalBlockStatement, ILoop
    {
        /// <summary>
        /// Whether or not the break the loop
        /// </summary>
        protected bool _breakLoop;


        /// <summary>
        /// Whether or not to continue the loop
        /// </summary>
        protected bool _continueLoop;


        /// <summary>
        /// Whether or not to continue running the loop
        /// </summary>
        protected bool _continueRunning;


        /// <summary>
        /// Create new instance/
        /// </summary>
        public LoopStatement() : base(null, null) { }


        /// <summary>
        /// Create new instance with condition
        /// </summary>
        /// <param name="condition"></param>
        public LoopStatement(Expression condition)
            : base(condition, null)
        {
        }


        /// <summary>
        /// Execute
        /// </summary>
        public override void Execute()
        {
            _continueRunning = true;
            _breakLoop = false;
            _continueLoop = false;
            _continueRunning = Condition.EvaluateAs<bool>();

            while (_continueRunning)
            {
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

                _continueRunning = Condition.EvaluateAs<bool>();
            }
        }


        /// <summary>
        /// Break loop
        /// </summary>
        public void Break()
        {
            _breakLoop = true;
        }


        /// <summary>
        /// Continue loop
        /// </summary>
        public void Continue()
        {
            _continueLoop = true;
        }
    }    
}
