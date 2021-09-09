using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// References to the script name, line number, char position.
    /// </summary>
    public class ScriptRef
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="name"></param>
        /// <param name="line"></param>
        /// <param name="charPos"></param>
        public ScriptRef(string name, int line, int charPos)
        {
            this.LineNumber = line;
            this.CharPosition = charPos;
            this.ScriptName = name;
        }


        /// <summary>
        /// Script info.
        /// </summary>
        public readonly string ScriptName;


        /// <summary>
        /// Line number in the script.
        /// </summary>
        public readonly int LineNumber;


        /// <summary>
        /// Char position in the line in the script.
        /// </summary>
        public readonly int CharPosition; 
    }
}
