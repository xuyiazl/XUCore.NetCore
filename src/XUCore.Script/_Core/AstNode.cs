using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// Abstract syntax tree node.
    /// </summary>
    public class AstNode
    {
        /// <summary>
        /// Reference to the script.
        /// </summary>
        public ScriptRef Ref;


        /// <summary>
        /// Context information of the script.
        /// </summary>
        public Context Ctx;
    }
}
