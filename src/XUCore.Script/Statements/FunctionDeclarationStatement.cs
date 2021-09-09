using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// Represents a function declaration
    /// </summary>
    public class FunctionDeclarationStatement : BlockStatement
    {
        private FunctionStatement _function = new FunctionStatement();


        /// <summary>
        /// Function 
        /// </summary>
        public FunctionStatement Function
        {
            get { return _function; }
        }


        /// <summary>
        /// Execute the function statement.
        /// </summary>
        public override void Execute()
        {
            Ctx.Functions.Register(Function.Name, Function);
        }


        /// <summary>
        /// String representation
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="incrementTab"></param>
        /// <param name="includeNewLine"></param>
        /// <returns></returns>
        public override string AsString(string tab = "", bool incrementTab = false, bool includeNewLine = true)
        {
            return _function.AsString(tab, incrementTab, includeNewLine);
        }
    }
}
