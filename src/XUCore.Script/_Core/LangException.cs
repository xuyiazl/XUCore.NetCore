using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// Exception used in script parsing
    /// </summary>
    public class LangException : Exception
    {
        /// <summary>
        /// Line number of exception
        /// </summary>
        public int LineNumber;


        /// <summary>
        /// Char position of the error.
        /// </summary>
        public int CharPostion;
         

        /// <summary>
        /// The path to the script that caused the exception
        /// </summary>
        public string ScriptPath;


        /// <summary>
        /// THe type of error.
        /// </summary>
        public string Name;


        /// <summary>
        /// Type of the error.
        /// </summary>
        public string ErrorType;


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="errorType">Type of the error. "Syntax Error"</param>
        /// <param name="error">Error message</param>
        /// <param name="scriptpath">Path of the script</param>
        /// <param name="lineNumber">Line number of the error.</param>
        /// <param name="charPos">The char position of the error.</param>
        public LangException(string errorType, string error, string scriptpath, int lineNumber, int charPos = 0) : base(error)
        {
            LineNumber = lineNumber;
            Name = error;
            ErrorType = errorType;
            ScriptPath = scriptpath;
            CharPostion = charPos;
        }
    }



    /// <summary>
    /// Exception used in script for sandbox/limits functionality. e.g. loop/callstack limits.
    /// </summary>
    public class LangLimitException : LangException
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="error">Error message</param>
        /// <param name="scriptpath">Script where error occurred.</param>
        /// <param name="lineNumber">Line number where error occurred.</param>
        public LangLimitException(string error, string scriptpath, int lineNumber)
            : base("Limit Error", error, scriptpath, lineNumber) {  }
    }
}
