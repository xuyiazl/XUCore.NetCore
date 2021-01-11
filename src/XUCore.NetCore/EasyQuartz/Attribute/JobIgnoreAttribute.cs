using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.EasyQuartz
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class JobIgnoreAttribute : Attribute
    {
    }
}
