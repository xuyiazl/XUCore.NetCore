using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Mongo
{
    /// <summary>
    /// mongo的异常对象
    /// </summary>
    public class MongoException : Exception
    {
        public MongoException(string CustomeMsg) : base(string.Format("错误信息：{0}， 异常错误,请及时排查", CustomeMsg))
        {

        }
    }
}
