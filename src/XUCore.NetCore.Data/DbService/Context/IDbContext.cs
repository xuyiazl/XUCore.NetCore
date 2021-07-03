using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.Data.DbService
{
    /// <summary>
    /// 多数据库操作继承该接口
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        string ConnectionStrings { get; set; }
        /// <summary>
        /// dbset
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        /// <summary>
        /// database
        /// </summary>
        /// <returns></returns>
        DatabaseFacade Database { get; }
    }
}
