using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XUCore.WebTests.Data.Entity
{
    public class BaseEntity
    {
        /// <summary>
        /// 自增id
        /// </summary>
        [Key(0)]
        public long Id { get; set; }
    }
}
