using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// Interface for a loop
    /// </summary>
    internal interface ILoop
    {
        /// <summary>
        /// Continue to next iteration.
        /// </summary>
        void Continue();


        /// <summary>
        /// Break the loop.
        /// </summary>
        void Break();
    }



    /// <summary>
    /// Interface for expression that uses parameters. right now "new" and "function".
    /// </summary>
    internal interface IParameterExpression
    {
        List<object> ParamList { get; set; }


        List<Expression> ParamListExpressions { get; set; }
    }
}
