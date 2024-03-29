﻿using System;
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
    public class ValueExpression : Expression
    {
        /// <summary>
        /// Name of the variable.
        /// </summary>
        public string Name;


        /// <summary>
        /// Datatype of the variable.
        /// </summary>
        public Type DataType;


        /// <summary>
        /// Value of the variable.
        /// </summary>
        public object Value;
    }
}
