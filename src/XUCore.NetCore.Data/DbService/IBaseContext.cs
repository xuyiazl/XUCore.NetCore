using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.Data.DbService
{
    public interface IBaseContext
    {
        string ConnectionStrings { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;


        int SaveChanges();
        /// <summary>
        /// 执行异步存储操作
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /*IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters)
            where TEntity : BaseEntity, new();*/

        //IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters);

        //int ExecuteSqlCommand(string sql, int? timeout = null, params object[] parameters);
    }
}
