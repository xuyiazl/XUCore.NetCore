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
    public class ForLoopStatement : LoopStatement
    {
        /// <summary>
        /// Initialize
        /// </summary>
        public ForLoopStatement() : this(null, null, null)
        {
        }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="start">start expression</param>
        /// <param name="condition">condition for loop</param>
        /// <param name="inc">increment expression</param>
        public ForLoopStatement(Statement start, Expression condition, Statement inc) : base(condition)
        {
            Init(start, condition, inc);
        }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="start">start expression</param>
        /// <param name="condition">condition for loop</param>
        /// <param name="inc">increment expression</param>
        public void Init(Statement start, Expression condition, Statement inc) 
        {
            Start = start;
            Increment = inc;
            Condition = condition;
        }


        /// <summary>
        /// Start statement.
        /// </summary>
        public Statement Start;


        /// <summary>
        /// Increment statement.
        /// </summary>
        public Statement Increment;



        /// <summary>
        /// Execute each expression.
        /// </summary>
        /// <returns></returns>
        public override void Execute()
        {
            Start.Execute();
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

                Increment.Execute();
                _continueRunning = Condition.EvaluateAs<bool>();
            }
        }
    }    
}
