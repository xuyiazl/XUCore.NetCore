using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// New instance creation.
    /// </summary>
    public class NewExpression : Expression, IParameterExpression
    {
        /// <summary>
        /// Initialize
        /// </summary>
        public NewExpression()
        {
            ParamList = new List<object>();
            ParamListExpressions = new List<Expression>();
        }


        /// <summary>
        /// Name of 
        /// </summary>
        public string TypeName { get; set; }



        /// <summary>
        /// List of expressions.
        /// </summary>
        public List<Expression> ParamListExpressions { get; set; }


        /// <summary>
        /// List of arguments.
        /// </summary>
        public List<object> ParamList { get; set; }


        /// <summary>
        /// Creates new instance of the type.
        /// </summary>
        /// <returns></returns>
        public override object Evaluate()
        {
            if (string.Compare(TypeName, "Date", false) == 0)
                return new DateTime(DateTime.Now.Ticks);
            
            object[] constructorArgs = null;
            if (ParamListExpressions != null && ParamListExpressions.Count > 0)
            {
                ParamList = new List<object>();
                FunctionHelper.ResolveParameters(ParamListExpressions, ParamList);
                constructorArgs = ParamList.ToArray();
            }
            return Ctx.Types.Create(TypeName, constructorArgs);
        }
    }
}
