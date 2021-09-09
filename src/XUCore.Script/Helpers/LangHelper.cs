using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// Helper class
    /// </summary>
    public class LangHelper
    {
        /// <summary>
        /// Whether or not the type supplied is a basic type.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsBasicType(object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            Type type = obj.GetType();
            if (type == typeof(int)) return true;
            if (type == typeof(long)) return true;
            if (type == typeof(double)) return true;
            if (type == typeof(bool)) return true;
            if (type == typeof(string)) return true;
            if (type == typeof(DateTime)) return true;

            return false;
        }



        /// <summary>
        /// Executes the statements.
        /// </summary>
        /// <param name="statements"></param>
        /// <param name="parent"></param>
        public static void Execute(List<Statement> statements, Statement parent)
        {
            if (statements != null && statements.Count > 0)
            {
                foreach (var stmt in statements)
                {
                    stmt.Execute();
                }
            }
        }
    }
}
