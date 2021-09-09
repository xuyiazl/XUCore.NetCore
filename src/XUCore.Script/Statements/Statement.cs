using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// Base class for statements.
    /// </summary>
    public class Statement : AstNode
    {
        /// <summary>
        /// Parent of this statement
        /// </summary>
        public Statement Parent { get; set; }


        /// <summary>
        /// Executes the statement.
        /// </summary>
        public virtual void Execute()
        {
        }


        /// <summary>
        /// Finds the first parent that is of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T FindParent<T>() where T : class
        {
            T found = default(T);
            Statement current = Parent;
            while (current != null)
            {
                if (current is T)
                {
                    found = current as T;
                    break;
                }
                current = Parent.Parent;
            }
            return found;
        }


        /// <summary>
        /// String representation of statement.
        /// </summary>
        /// <param name="tab">Tab to use</param>
        /// <param name="incrementTab">Whether or not to add another tab</param>
        /// <param name="includeNewLine">Whether or not to include a new line.</param>
        /// <returns></returns>
        public virtual string AsString(string tab = "", bool incrementTab = false, bool includeNewLine = true)
        {   
            var stmtType = this.GetType().Name.Replace("Statement", "");
            string info = string.Format("{0}, {1}, {2} ", stmtType, Ref.LineNumber, Ref.CharPosition);

            if (incrementTab)
                tab = tab + "\t";

            var result = tab + info;
            if (includeNewLine) result += Environment.NewLine;

            return result;
        }

    }
}
