using XUCore.NetCore.Data.DbService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XUCore.WebTests.Data.Repository.ReadRepository
{
    /// <summary>
    /// 读取仓库
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadRepository<T> : IMsSqlRepository<T> where T : class, new()
    {

    }
}
