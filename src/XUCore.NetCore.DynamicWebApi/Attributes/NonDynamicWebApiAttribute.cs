using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.DynamicWebApi
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class NonDynamicWebApiAttribute : Attribute
    {

    }
}
