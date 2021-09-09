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
    public class BlockStatement : Statement
    {
        /// <summary>
        /// List of statements
        /// </summary>
        protected List<Statement> _statements = new List<Statement>();


        /// <summary>
        /// Public access to statments.
        /// </summary>
        public List<Statement> Statements
        {
            get { return _statements; }
        }


        /// <summary>
        /// Execute the statements.
        /// </summary>
        public override void Execute()
        {
            LangHelper.Execute(this._statements, this.Parent);
        }


        /// <summary>
        /// String representation
        /// </summary>
        /// <param name="tab">Tab to use for nested statements in blocks</param>
        /// <param name="incrementTab">Whether or not to add another tab</param>
        /// <param name="includeNewLine">Whether or not to include a new line.</param>
        /// <returns></returns>
        public override string AsString(string tab = "", bool incrementTab = false,  bool includeNewLine = true)
        {
            string info = base.AsString(tab, incrementTab);

            // Empty statements?
            if (_statements == null || _statements.Count == 0) return info;

            var buffer = new StringBuilder();

            // Now iterate over all the statements in the block
            foreach (var stmt in _statements)
            {
                buffer.Append(stmt.AsString(tab, true));
            }

            var result = info + buffer.ToString();
            if (includeNewLine) result += Environment.NewLine;

            return result;
        }
    }



    /// <summary>
    /// Conditional based block statement used in ifs/elses/while
    /// </summary>
    public class ConditionalBlockStatement : BlockStatement
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="statements"></param>
        public ConditionalBlockStatement(Expression condition, List<Statement> statements)
        {
            this.Condition = condition;
            this._statements = statements == null ? new List<Statement>() : statements;
        }


        /// <summary>
        /// The condition to check.
        /// </summary>
        public Expression Condition;

    }
}
