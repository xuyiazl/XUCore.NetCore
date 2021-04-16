using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using XUCore.Extensions;
using XUCore.Paging;

namespace XUCore.NetCore.Mongo
{

    /// <summary>
    /// mongodb的基础仓库支持类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class AbstractMongoRepository<TEntity> : IMongoRepositoryBase<TEntity> where TEntity : MongoEntity
    {
        /// <summary>
        /// 实体配置缓存
        /// </summary>
        private ConcurrentDictionary<Type, (string, string)> options = new ConcurrentDictionary<Type, (string, string)>();
        /// <summary>
        /// 主键属性名
        /// </summary>
        protected string BsonIds { get; set; } = "";
        /// <summary>
        /// mongo的连接字符串,支持多个连接地址的mongo操作
        /// </summary>
        protected static Lazy<List<MongoConnection>> Connection { get; set; }

        public AbstractMongoRepository()
        {
            BsonIds = GetBsonIds();
        }

        /// <summary>
        /// 获取对应的数据库
        /// </summary>
        protected MongoConnection GetDatabaseConfig(string connectionName = null)
        {
            if (Connection.Value == null)
                throw new MongoException("mongo的连接字符串错误");

            if (!string.IsNullOrEmpty(connectionName))
                return Connection.Value.FirstOrDefault(a => a.ConnectionName == connectionName);
            else
                return Connection.Value.FirstOrDefault();
        }
        /// <summary>
        /// 获得数据Collection内容
        /// </summary>
        public IMongoCollection<TEntity> Table
        {
            get
            {
                var connectionName = string.Empty;
                var tableName = string.Empty;

                var type = typeof(TEntity);
                if (options.TryGetValue(type, out (string conectionName, string tableName) value))
                {
                    connectionName = value.conectionName;
                    tableName = value.tableName;
                }
                else
                {
                    object[] objs = typeof(TEntity).GetCustomAttributes(typeof(MongoAttribute), true);
                    foreach (object obj in objs)
                    {
                        if (obj is MongoAttribute attribute)
                        {
                            connectionName = attribute.ConnectionName;
                            tableName = attribute.Table;
                            break;
                        }
                    }

                    options.TryAdd(type, (connectionName, tableName));
                }

                var db = GetDatabaseConfig(connectionName);
                if (db == null)
                    throw new MongoException("数据库连接别名配置错误，数据库连接串里的ConnectionName必须和ModelProperty.ConnectionName一致，多个mongo连接串请配置不同的名称");

                if (tableName.IsEmpty())
                    throw new MongoException("数据库表名配置错误，实体对象必须加ModelProperty标签，ModelProperty.TableName不能为空");

                var mongoClient = db.Client.GetCollection<TEntity>(tableName);
                if (mongoClient == null)
                {
                    throw new MongoException($"{typeof(TEntity)} is Null");
                }

                return mongoClient;
            }
        }
        /// <summary>
        /// 获得具有bsonId的属性
        /// </summary>
        /// <returns></returns>
        public string GetBsonIds()
        {
            if (string.IsNullOrWhiteSpace(BsonIds))
            {
                Type type = typeof(TEntity);
                PropertyInfo[] props = type.GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    object[] objs = prop.GetCustomAttributes(typeof(BsonIdAttribute), true);
                    if (objs.Length > 0)
                        return prop.Name;
                }
            }
            return BsonIds;
        }

        #region [新增]

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <param name="bypassDocumentValidation">是否绕过文档验证</param>
        /// <returns></returns>
        public virtual TEntity Add(TEntity model, bool? bypassDocumentValidation = null)
        {
            Table.InsertOne(model, new InsertOneOptions()
            {
                BypassDocumentValidation = bypassDocumentValidation
            });
            return model;
        }
        /// <summary>
        /// 异步添加
        /// </summary>
        /// <param name="model"></param>
        /// <param name="bypassDocumentValidation">是否绕过文档验证</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> AddAsync(TEntity model, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
        {
            await Table.InsertOneAsync(model, new InsertOneOptions()
            {
                BypassDocumentValidation = bypassDocumentValidation
            }, cancellationToken);

            return model;
        }
        /// <summary>
        /// 同步批量添加
        /// </summary>
        /// <param name="models"></param>
        /// <param name="isOrdered">是否按顺序写入</param>
        /// <param name="bypassDocumentValidation">是否绕过文档验证</param>
        public virtual void Add(IEnumerable<TEntity> models, bool isOrdered = true, bool? bypassDocumentValidation = null)
        {
            Table.InsertMany(models, new InsertManyOptions()
            {
                IsOrdered = isOrdered,
                BypassDocumentValidation = bypassDocumentValidation
            });
        }
        /// <summary>
        /// 异步批量添加
        /// </summary>
        /// <param name="models"></param>
        /// <param name="isOrdered">是否按顺序写入</param>
        /// <param name="bypassDocumentValidation">是否绕过文档验证</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task AddAsync(IEnumerable<TEntity> models, bool isOrdered = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
        {
            await Table.InsertManyAsync(models, new InsertManyOptions()
            {
                IsOrdered = isOrdered,
                BypassDocumentValidation = bypassDocumentValidation
            }, cancellationToken);
        }

        #endregion

        #region [更新]
        /// <summary>
        /// 同步更新
        /// </summary>
        /// <param name="model"></param>
        /// <param name="where">条件</param>
        /// <param name="isUpsert">不存在时插入该文档</param>
        /// <param name="bypassDocumentValidation">是否绕过文档验证</param>
        /// <returns></returns>
        public virtual bool Update(TEntity model, Expression<Func<TEntity, bool>> where = null, bool isUpsert = true, bool? bypassDocumentValidation = null)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.ObjectId, model.ObjectId);
            if (where != null)
                filter = Builders<TEntity>.Filter.Where(where);

            return Update(model, filter, isUpsert, bypassDocumentValidation);
        }
        /// <summary>
        /// 同步更新
        /// </summary>
        /// <param name="model"></param>
        /// <param name="filter">条件</param>
        /// <param name="isUpsert">不存在时插入该文档</param>
        /// <param name="bypassDocumentValidation">是否绕过文档验证</param>
        /// <returns></returns>
        public virtual bool Update(TEntity model, FilterDefinition<TEntity> filter, bool isUpsert = true, bool? bypassDocumentValidation = null)
        {
            var uModel = model.BuildUpdateDefinition<TEntity>();
            if (uModel != null)
                return Table.UpdateOne(filter, Builders<TEntity>.Update.Combine(uModel), new UpdateOptions()
                {
                    IsUpsert = isUpsert,
                    BypassDocumentValidation = bypassDocumentValidation
                }).IsAcknowledged;
            else
                return false;
        }
        /// <summary>
        /// 异步更新
        /// </summary>
        public virtual async Task<UpdateResult> UpdateAsync(TEntity model, Expression<Func<TEntity, bool>> where = null, bool isUpsert = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.ObjectId, model.ObjectId);
            if (where != null)
                filter = Builders<TEntity>.Filter.Where(where);

            return await UpdateAsync(model, filter, isUpsert, bypassDocumentValidation, cancellationToken);
        }
        /// <summary>
        /// 异步更新
        /// </summary>
        public virtual async Task<UpdateResult> UpdateAsync(TEntity model, FilterDefinition<TEntity> filter, bool isUpsert = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
        {
            var uModel = model.BuildUpdateDefinition<TEntity>();
            if (uModel != null)
                return await Table.UpdateOneAsync(filter, Builders<TEntity>.Update.Combine(uModel), new UpdateOptions()
                {
                    IsUpsert = isUpsert,
                    BypassDocumentValidation = bypassDocumentValidation
                }, cancellationToken);
            else
                return await Task.FromResult<UpdateResult>(null);
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        public virtual bool Update(IEnumerable<TEntity> models, Expression<Func<TEntity, bool>> where = null, bool isUpsert = true)
        {
            var wModels = new List<WriteModel<TEntity>>();
            foreach (var model in models)
            {
                var filter = Builders<TEntity>.Filter.Eq(e => e.ObjectId, model.ObjectId);
                if (where != null)
                    filter = Builders<TEntity>.Filter.Where(where);

                var uModel = model.BuildUpdateDefinition<TEntity>();
                if (uModel != null)
                {
                    var replaceModel = new UpdateOneModel<TEntity>(filter, Builders<TEntity>.Update.Combine(uModel))
                    {
                        IsUpsert = isUpsert
                    };
                    wModels.Add(replaceModel);
                }
            }

            if (wModels.Count > 0)
                return Table.BulkWrite(wModels, new BulkWriteOptions { IsOrdered = false }).IsAcknowledged;
            else
                return false;
        }
        /// <summary>
        /// 异步批量更新
        /// </summary>
        public virtual async Task<BulkWriteResult<TEntity>> UpdateAsync(IEnumerable<TEntity> models, Expression<Func<TEntity, bool>> where = null, bool isUpsert = true, CancellationToken cancellationToken = default)
        {
            var wModels = new List<WriteModel<TEntity>>();
            foreach (var model in models)
            {
                var filter = Builders<TEntity>.Filter.Eq(e => e.ObjectId, model.ObjectId);
                if (where != null)
                    filter = Builders<TEntity>.Filter.Where(where);

                var uModel = model.BuildUpdateDefinition<TEntity>();
                if (uModel != null)
                {
                    var replaceModel = new UpdateOneModel<TEntity>(filter, Builders<TEntity>.Update.Combine(uModel))
                    {
                        IsUpsert = isUpsert
                    };
                    wModels.Add(replaceModel);
                }
            }

            if (wModels.Count > 0)
                return await Table.BulkWriteAsync(wModels, new BulkWriteOptions { IsOrdered = false });
            else
                return await Task.FromResult<BulkWriteResult<TEntity>>(null);
        }
        /// <summary>
        /// 同步修改（部分字段）
        /// </summary>
        public virtual bool Update(Expression<Func<TEntity, bool>> where, string field, string value, bool? bypassDocumentValidation = null)
        {
            return this.Table.UpdateOne(Builders<TEntity>.Filter.Where(where), Builders<TEntity>.Update.Set(field, value), new UpdateOptions() { IsUpsert = false, BypassDocumentValidation = bypassDocumentValidation }).IsAcknowledged;
        }
        /// <summary>
        /// 异步修改（部分字段）
        /// </summary>
        public virtual async Task<UpdateResult> UpdateAsync(Expression<Func<TEntity, bool>> where, string field, string value, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
        {
            return await this.Table.UpdateOneAsync(Builders<TEntity>.Filter.Where(where), Builders<TEntity>.Update.Set(field, value), new UpdateOptions() { IsUpsert = false, BypassDocumentValidation = bypassDocumentValidation }, cancellationToken);
        }
        /// <summary>
        /// 批量修改（部分字段）
        /// </summary>
        public virtual bool Update(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> update)
        {
            var table = Table;
            var filter = Builders<TEntity>.Filter.Where(where);

            var uList = update.BuildUpdateDefinition();
            if (uList != null)
                return table.UpdateMany(filter, Builders<TEntity>.Update.Combine(uList), new UpdateOptions() { IsUpsert = false }).IsAcknowledged;
            else
                return false;
        }
        /// <summary>
        /// 异步批量修改（部分字段）
        /// </summary>
        public virtual async Task<UpdateResult> UpdateAsync(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> update, CancellationToken cancellationToken = default)
        {
            var table = Table;
            var filter = Builders<TEntity>.Filter.Where(where);

            var uList = update.BuildUpdateDefinition();
            if (uList != null)
                return await table.UpdateManyAsync(filter, Builders<TEntity>.Update.Combine(uList), new UpdateOptions() { IsUpsert = false }, cancellationToken);
            else
                return await Task.FromResult<UpdateResult>(null);
        }

        #endregion

        #region 大批量操作

        /// <summary>
        /// 大批量写入
        /// </summary>
        public virtual BulkWriteResult<TEntity> BulkAdd(IEnumerable<TEntity> models, bool isOrdered = true, bool? bypassDocumentValidation = null)
        {
            if (models != null && models.Any())
            {
                var writes = models.ToMap(c => new InsertOneModel<TEntity>(c));

                return BulkWrite(writes, isOrdered, bypassDocumentValidation);
            }
            else
                return null;
        }
        /// <summary>
        /// 异步大批量写入
        /// </summary>
        public virtual async Task<BulkWriteResult<TEntity>> BulkAddAsync(IEnumerable<TEntity> models, bool isOrdered = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
        {
            if (models != null && models.Any())
            {
                var writes = models.ToMap(c => new InsertOneModel<TEntity>(c));

                return await BulkWriteAsync(writes, isOrdered, bypassDocumentValidation);
            }
            else
                return null;
        }
        /// <summary>
        /// 大批量操作
        /// </summary>
        public virtual BulkWriteResult<TEntity> BulkWrite(IEnumerable<WriteModel<TEntity>> models, bool isOrdered = true, bool? bypassDocumentValidation = null)
        {
            if (models != null && models.Any())
            {
                return Table.BulkWrite(models, new BulkWriteOptions
                {
                    IsOrdered = isOrdered,
                    BypassDocumentValidation = bypassDocumentValidation
                });
            }
            else
                return null;
        }
        /// <summary>
        /// 异步大批量操作
        /// </summary>
        public virtual async Task<BulkWriteResult<TEntity>> BulkWriteAsync(IEnumerable<WriteModel<TEntity>> models, bool isOrdered = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
        {
            if (models != null && models.Any())
            {
                return await Table.BulkWriteAsync(models, new BulkWriteOptions
                {
                    IsOrdered = isOrdered,
                    BypassDocumentValidation = bypassDocumentValidation
                }, cancellationToken);
            }
            else
                return null;
        }

        #endregion

        #region [删除]

        /// <summary>
        /// 删除指定单一记录
        /// </summary>
        public virtual bool DeleteOne(TEntity t)
        {
            return Table.DeleteOne(Builders<TEntity>.Filter.Eq(BsonIds, t.ObjectId)).IsAcknowledged;
        }
        /// <summary>
        /// 按条件，删除
        /// </summary>
        public virtual TEntity FindOneAndDelete(Expression<Func<TEntity, bool>> where)
        {
            return Table.FindOneAndDelete<TEntity>(Builders<TEntity>.Filter.Where(where), null);
        }
        /// <summary>
        /// 按条件，异步删除
        /// </summary>
        public virtual async Task<DeleteResult> DeleteOneAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
        {
            return await Table.DeleteOneAsync(Builders<TEntity>.Filter.Where(where), cancellationToken);
        }
        /// <summary>
        /// 按条件，异步删除
        /// </summary>
        public virtual async Task<DeleteResult> DeleteOneAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default)
        {
            return await Table.DeleteOneAsync(filter, cancellationToken);
        }
        /// <summary>
        /// 按条件，批量删除
        /// </summary>
        public virtual bool Delete(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
        {
            return Table.DeleteMany(Builders<TEntity>.Filter.Where(where), cancellationToken).IsAcknowledged;
        }
        /// <summary>
        /// 按条件，批量删除
        /// </summary>
        public virtual bool Delete(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default)
        {
            return Table.DeleteMany(filter, cancellationToken).IsAcknowledged;
        }
        /// <summary>
        /// 按条件，异步批量删除
        /// </summary>
        public virtual async Task<DeleteResult> DeleteAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
        {
            return await Table.DeleteManyAsync(Builders<TEntity>.Filter.Where(where), cancellationToken);
        }
        /// <summary>
        /// 按条件，异步批量删除
        /// </summary>
        public virtual async Task<DeleteResult> DeleteAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default)
        {
            return await Table.DeleteManyAsync(filter, cancellationToken);
        }

        #endregion

        #region [查询]

        /// <summary>
        /// 获取指定Linq条件总记录数
        /// </summary>
        /// <returns></returns>
        public virtual long GetCount(Expression<Func<TEntity, bool>> where)
        {
            return Table.CountDocuments(where, null);
        }
        /// <summary>
        /// 获取指定Filter条件总记录数
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual long GetCount(FilterDefinition<TEntity> filter)
        {
            return Table.CountDocuments(filter, null);
        }
        /// <summary>
        /// 异步获取指定Linq条件总记录数
        /// </summary>
        /// <returns></returns>
        public virtual async Task<long> GetCountAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
        {
            return await Table.CountDocumentsAsync(where, null, cancellationToken);
        }
        /// <summary>
        /// 异步获取指定Filter条件总记录数
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<long> GetCountAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default)
        {
            return await Table.CountDocumentsAsync(filter, null, cancellationToken);
        }
        /// <summary>
        /// 获取指定主键id记录
        /// </summary>
        /// <returns></returns>
        public virtual TEntity GetById(object id)
        {
            return Table.Find(Builders<TEntity>.Filter.Eq(a => a.ObjectId, id))?.FirstOrDefault();
        }
        /// <summary>
        /// 异步获取指定主键id记录
        /// </summary>
        public virtual async Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            var list = await Table.FindAsync(Builders<TEntity>.Filter.Eq(a => a.ObjectId, id), null, cancellationToken);
            return list?.FirstOrDefault(cancellationToken: cancellationToken);
        }
        /// <summary>
        /// 获得指定Linq条件内的单条数据
        /// </summary>
        public virtual TEntity GetSingle(Expression<Func<TEntity, bool>> where)
        {
            return Table.Find(where)?.FirstOrDefault();
        }
        /// <summary>
        /// 获得指定Filter内的单条数据
        /// </summary>
        public virtual TEntity GetSingle(FilterDefinition<TEntity> filter)
        {
            return Table.Find(filter)?.FirstOrDefault();
        }
        /// <summary>
        /// 异步获得指定Linq条件内的单条数据
        /// </summary>
        public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
        {
            var list = await Table.FindAsync(where, null, cancellationToken);
            return list?.FirstOrDefault(cancellationToken: cancellationToken);
        }
        /// <summary>
        /// 异步获得指定Filter内的单条数据
        /// </summary>
        public virtual async Task<TEntity> GetSingleAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default)
        {
            var list = await Table.FindAsync(filter, null, cancellationToken);
            return list?.FirstOrDefault(cancellationToken: cancellationToken);
        }
        /// <summary>
        /// 获取当前所有集合
        /// </summary>
        /// <returns></returns>
        public virtual IList<TEntity> GetList()
        {
            return Table.Find(t => true)?.ToList();
        }
        /// <summary>
        /// 异步获取当前所有集合
        /// </summary>
        /// <returns>返回异步结果</returns>
        public virtual async Task<IList<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
        {
            var list = await Table.FindAsync(t => true, null, cancellationToken);
            return list?.ToList(cancellationToken: cancellationToken);
        }
        /// <summary>
        /// 获得指定Linq条件内的多条数据
        /// </summary>
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> where, string orderby = "", int? limit = null)
        {
            return Table.Find(where).Sort(orderby.OrderByBatch<TEntity>()).Limit(limit).ToList();
        }
        /// <summary>
        /// 获得指定Filter内的多条数据
        /// </summary>
        public virtual List<TEntity> GetList(FilterDefinition<TEntity> filter, string orderby = "", int? limit = null)
        {
            return Table.Find(filter).Sort(orderby.OrderByBatch<TEntity>()).Limit(limit).ToList();
        }
        /// <summary>
        /// 异步获得指定Linq条件内的多条数据
        /// </summary>
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> where, string orderby = "", int? limit = null, CancellationToken cancellationToken = default)
        {
            var list = await Table.FindAsync(where, new FindOptions<TEntity, TEntity>()
            {
                Limit = limit,
                Sort = orderby.OrderByBatch<TEntity>()
            }, cancellationToken);
            return list?.ToList(cancellationToken: cancellationToken);
        }
        /// <summary>
        /// 异步获得指定Filter内的多条数据
        /// </summary>
        public virtual async Task<List<TEntity>> GetListAsync(FilterDefinition<TEntity> filter, string orderby = "", int? limit = null, CancellationToken cancellationToken = default)
        {
            var list = await Table.FindAsync(filter, new FindOptions<TEntity, TEntity>()
            {
                Limit = limit,
                Sort = orderby.OrderByBatch<TEntity>()
            }, cancellationToken);
            return list?.ToList(cancellationToken: cancellationToken);
        }
        /// <summary>
        /// 分页获取数据
        /// </summary>
        public virtual PagedList<TEntity> GetPagedList(Expression<Func<TEntity, bool>> selector, string orderby, int currentPage, int pageSize)
        {
            currentPage = currentPage < 1 ? 1 : currentPage;

            var listTable = Table;

            var total = listTable.CountDocuments(selector, null);

            var pageList = listTable.Find(selector)?.Sort(orderby.OrderByBatch<TEntity>()).Skip((currentPage - 1) * pageSize).Limit(pageSize).ToList();
            return new PagedList<TEntity>(pageList, total, currentPage, pageSize);
        }
        /// <summary>
        /// 分页获取数据
        /// </summary>
        public virtual PagedList<TEntity> GetPagedList(FilterDefinition<TEntity> filter, string orderby, int currentPage, int pageSize)
        {
            currentPage = currentPage < 1 ? 1 : currentPage;

            var listTable = Table;

            var total = listTable.CountDocuments(filter, null);
            var pageList = listTable.Find(filter)?.Sort(orderby.OrderByBatch<TEntity>()).Skip((currentPage - 1) * pageSize).Limit(pageSize).ToList();

            return new PagedList<TEntity>(pageList, total, currentPage, pageSize);
        }
        /// <summary>
        /// 异步分页获取数据
        /// </summary>
        public virtual async Task<PagedList<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> selector, string orderby, int currentPage, int pageSize, CancellationToken cancellationToken = default)
        {
            currentPage = currentPage < 1 ? 1 : currentPage;

            var listTable = Table;

            var total = await listTable.CountDocumentsAsync(selector, null, cancellationToken);
            var list = await listTable.FindAsync(selector, new FindOptions<TEntity, TEntity>()
            {
                Skip = (currentPage - 1) * pageSize,
                Limit = pageSize,
                Sort = orderby.OrderByBatch<TEntity>()
            }, cancellationToken);

            return new PagedList<TEntity>(list?.ToList(cancellationToken: cancellationToken), total, currentPage, pageSize);
        }
        /// <summary>
        /// 异步分页获取数据
        /// </summary>
        public virtual async Task<PagedList<TEntity>> GetPagedListAsync(FilterDefinition<TEntity> filter, string orderby, int currentPage, int pageSize, CancellationToken cancellationToken = default)
        {
            currentPage = currentPage < 1 ? 1 : currentPage;

            var listTable = Table;

            var total = await listTable.CountDocumentsAsync(filter, null, cancellationToken);
            var list = await listTable.FindAsync(filter, new FindOptions<TEntity, TEntity>()
            {
                Skip = (currentPage - 1) * pageSize,
                Limit = pageSize,
                Sort = orderby.OrderByBatch<TEntity>()
            }, cancellationToken);

            return new PagedList<TEntity>(list?.ToList(cancellationToken: cancellationToken), total, currentPage, pageSize);
        }
        #endregion
    }
}
