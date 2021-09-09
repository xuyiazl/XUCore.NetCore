using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// Boolean datatype.
    /// </summary>
    public class LNumber : LObject
    {   
        /// <summary>
        /// Get boolean value.
        /// </summary>
        /// <returns></returns>
        public bool ToDouble()
        {
            return (bool)_value;
        }
    }
}
