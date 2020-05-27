using XUCore.NetCore.Data.DbService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XUCore.WebTests.Data.Repository.WriteRepository
{
    /// <summary>
    /// 写入仓库
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWriteRepository<T> : IMySqlRepository<T> where T : class, new()
    {

    }
}
