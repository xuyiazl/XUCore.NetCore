using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Mongo
{
    /// <summary>
    /// Mongo Property Attribute
    /// </summary>
    public class MongoAttribute : Attribute
    {
        /// <summary>
        /// 数据库连接别名，等同于配置文件里mongo数据库连接里的ConnectionName，两者必须一致，多个mongo连接串请配置不同的名称
        /// </summary>
        public string ConnectionName { get; set; }
        /// <summary>
        /// Table Name
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MongoAttribute()
        {
        }
    }
}
