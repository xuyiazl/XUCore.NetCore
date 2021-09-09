using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// Used to hold the state of the parser.
    /// </summary>
    public class ParserState
    {
        /// <summary>
        /// The last statement.
        /// </summary>
        public Statement LastStmt;


        /// <summary>
        /// Last expression
        /// </summary>
        public Expression LastExpPart;


        /// <summary>
        /// Number of times a member access is performed during parsing.
        /// </summary>
        public int MemberAccess;


        /// <summary>
        /// Nested statement.
        /// </summary>
        public int StatementNested;


        /// <summary>
        /// Stack count for parsing currently being inside a index expression.
        /// </summary>
        public int IndexExp;


        /// <summary>
        /// Stack count for parsing currently being inside a function call expression.
        /// </summary>
        public int FunctionCall;


        /// <summary>
        /// Stack count for parsing currently being inside a conditional e.g. parenthesis (condition)
        /// </summary>
        public int Conditional;


        /// <summary>
        /// Used to keep track of number of consequetive expressions.
        /// </summary>
        public int ExpressionCount;


        /// <summary>
        /// Number of parenthesis.
        /// </summary>
        public int ParenthesisCount;


        /// <summary>
        /// Whether or not in function call.
        /// </summary>
        public bool IsInFunctionCall { get { return FunctionCall > 0; } }


        /// <summary>
        /// Whether or not in conditional call.
        /// </summary>
        public bool IsInConditional { get { return Conditional > 0; } }


        /// <summary>
        /// Whether or not in index exp.
        /// </summary>
        public bool IsInIndex { get { return IndexExp > 0; } }
    }
}
