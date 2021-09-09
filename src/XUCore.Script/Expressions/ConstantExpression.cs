using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;


namespace XUCore.Script
{
    /// <summary>
    /// Variable expression data
    /// </summary>
    public class ConstantExpression : ValueExpression
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="val"></param>
        public ConstantExpression(object val)
        {
            this.Value = val;
            this.DataType = val.GetType();
        }


        /// <summary>
        /// Evaluate value.
        /// </summary>
        /// <returns></returns>
        public override object Evaluate()
        {
            return this.Value;
        }
    }
}
