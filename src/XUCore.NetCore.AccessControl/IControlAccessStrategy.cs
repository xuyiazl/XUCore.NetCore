using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.AccessControl
{
    /// <summary>
    /// view component access strategy
    /// </summary>
    public interface IControlAccessStrategy
    {
        /// <summary>
        /// view component access strategy
        /// </summary>
        bool IsControlCanAccess(string accessKey);
    }
}
