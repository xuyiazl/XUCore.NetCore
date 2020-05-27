using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace XUCore.NetCore.Data.DbRepositories
{
    public partial class DbRepository<TEntity> : IDbRepository<TEntity> where TEntity : class
    {
        public void Add(TEntity entity)
        {
            Table.Add(entity);
        }

        public async void AddAsync(TEntity entity)
        {
            await Table.AddAsync(entity);
        }

        public async void AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await Table.AddAsync(entity, cancellationToken);
        }

        public async void AddRange(IList<TEntity> Entities)
        {
            await Table.AddRangeAsync(Entities);
        }

        public async void AddRangeAsync(IList<TEntity> Entities)
        {
            await Table.AddRangeAsync(Entities);
        }

        public async void AddRangeAsync(IList<TEntity> Entities, CancellationToken cancellationToken = default)
        {
            await Table.AddRangeAsync(Entities, cancellationToken);
        }

        public void Update(TEntity entity)
        {
            Table.Update(entity);
        }

        public void Update(TEntity entity, params Expression<Func<TEntity, object>>[] updatedProperties)
        {
            Context.Attach(entity);
            var dbEntry = Entry(entity);
            if (updatedProperties.Any())
            {
                foreach (var property in updatedProperties)
                {
                    dbEntry.Property(property).IsModified = true;
                }
            }
            //else
            //{
            //    foreach (var property in dbEntry.OriginalValues.Properties)
            //    {
            //        var original = dbEntry.OriginalValues.GetValue<object>(property);
            //        var current = dbEntry.CurrentValues.GetValue<object>(property);
            //        if (original != null && !original.Equals(current))
            //        {
            //            dbEntry.Property(property.Name).IsModified = true;
            //        }
            //    }
            //}
        }

        public void UpdateRange(IList<TEntity> entities)
        {
            if (entities == null || entities.Count() == 0) return;
            Table.UpdateRange(entities);
        }

        public void Delete(TEntity Entity)
        {
            Table.Remove(Entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> selector)
        {
            var entity = this.AsNoTracking().FirstOrDefault(selector);
            if (entity != null)
            {
                Table.Remove(entity);
            }
        }

        public void DeleteRange(IList<TEntity> entities)
        {
            if (entities == null || entities.Count() == 0)
                return;

            Table.RemoveRange(entities);
        }

        public void DeleteRange(Expression<Func<TEntity, bool>> selector)
        {
            var res = Table.Where(selector);

            if (Table.Count() == 0) return;

            Table.RemoveRange(res);
        }
    }
}