using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XUCore.Ddd.Domain;
using XUCore.NetCore.Data.DbService;

namespace XUCore.NetCore.DataTest.Mapping
{
    public abstract class BaseMapping<T> : KeyMapping<T>
        where T : Entity, new()
    {
        public BaseMapping(string tableName, Expression<Func<T, object>> primaryKey) : base(tableName, primaryKey)
        {

        }

        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
        }
    }
}
