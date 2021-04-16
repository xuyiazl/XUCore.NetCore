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
    public abstract class AbstractMongoRepository : IMongoRepositoryBase
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

        public AbstractMongoRepository() { }

        /// <summary>
        /// 获取对应的数据库
        /// </summary>
        protected MongoConnection GetDatabase(string connectionName = null)
        {
            if (Connection.Value == null)
                throw new MongoException("mongo的连接字符串错误");

            if (!connectionName.IsEmpty())
                return Connection.Value.FirstOrDefault(a => a.ConnectionName == connectionName);
            else
                return Connection.Value.FirstOrDefault();
        }
        /// <summary>
        /// 获得数据Collection内容
        /// </summary>
        public IMongoCollection<TEntity> GetTable<TEntity>()
            where TEntity : MongoEntity
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

            var db = GetDatabase(connectionName);

            if (db == null)
                throw new MongoException("数据库连接别名配置错误，数据库连接串里的ConnectionName必须和MongoAttribute.ConnectionName一致，多个mongo连接串请配置不同的名称");

            if (tableName.IsEmpty())
                throw new MongoException("数据库表名配置错误，实体对象必须加MongoAttribute标签，MongoAttribute.Table不能为空");

            var mongoClient = db.Client.GetCollection<TEntity>(tableName);
            if (mongoClient == null)
                throw new MongoException($"{typeof(TEntity)} is Null");

            return mongoClient;
        }
        /// <summary>
        /// 获得具有bsonId的属性
        /// </summary>
        /// <returns></returns>
        public string GetBsonIds<TEntity>()
            where TEntity : MongoEntity
        {
            if (BsonIds.IsEmpty())
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
        public virtual TEntity Add<TEntity>(TEntity model, bool? bypassDocumentValidation = null)
            where TEntity : MongoEntity
        {
            GetTable<TEntity>().InsertOne(model, new InsertOneOptions()
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
        public virtual async Task<TEntity> AddAsync<TEntity>(TEntity model, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            await GetTable<TEntity>().InsertOneAsync(model, new InsertOneOptions()
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
        public virtual void Add<TEntity>(IEnumerable<TEntity> models, bool isOrdered = true, bool? bypassDocumentValidation = null)
            where TEntity : MongoEntity
        {
            GetTable<TEntity>().InsertMany(models, new InsertManyOptions()
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
        public virtual async Task AddAsync<TEntity>(IEnumerable<TEntity> models, bool isOrdered = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            await GetTable<TEntity>().InsertManyAsync(models, new InsertManyOptions()
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
        public virtual bool Update<TEntity>(TEntity model, Expression<Func<TEntity, bool>> where = null, bool isUpsert = true, bool? bypassDocumentValidation = null)
            where TEntity : MongoEntity
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
        public virtual bool Update<TEntity>(TEntity model, FilterDefinition<TEntity> filter, bool isUpsert = true, bool? bypassDocumentValidation = null)
            where TEntity : MongoEntity
        {
            var uModel = model.BuildUpdateDefinition<TEntity>();
            if (uModel != null)
                return GetTable<TEntity>().UpdateOne(filter, Builders<TEntity>.Update.Combine(uModel), new UpdateOptions()
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
        public virtual async Task<UpdateResult> UpdateAsync<TEntity>(TEntity model, Expression<Func<TEntity, bool>> where = null, bool isUpsert = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.ObjectId, model.ObjectId);
            if (where != null)
                filter = Builders<TEntity>.Filter.Where(where);

            return await UpdateAsync(model, filter, isUpsert, bypassDocumentValidation, cancellationToken);
        }
        /// <summary>
        /// 异步更新
        /// </summary>
        public virtual async Task<UpdateResult> UpdateAsync<TEntity>(TEntity model, FilterDefinition<TEntity> filter, bool isUpsert = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            var uModel = model.BuildUpdateDefinition<TEntity>();
            if (uModel != null)
                return await GetTable<TEntity>().UpdateOneAsync(filter, Builders<TEntity>.Update.Combine(uModel), new UpdateOptions()
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
        public virtual bool Update<TEntity>(IEnumerable<TEntity> models, Expression<Func<TEntity, bool>> where = null, bool isUpsert = true)
            where TEntity : MongoEntity
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
                return GetTable<TEntity>().BulkWrite(wModels, new BulkWriteOptions { IsOrdered = false }).IsAcknowledged;
            else
                return false;
        }
        /// <summary>
        /// 异步批量更新
        /// </summary>
        public virtual async Task<BulkWriteResult<TEntity>> UpdateAsync<TEntity>(IEnumerable<TEntity> models, Expression<Func<TEntity, bool>> where = null, bool isUpsert = true, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
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
                return await GetTable<TEntity>().BulkWriteAsync(wModels, new BulkWriteOptions { IsOrdered = false });
            else
                return await Task.FromResult<BulkWriteResult<TEntity>>(null);
        }
        /// <summary>
        /// 同步修改（部分字段）
        /// </summary>
        public virtual bool Update<TEntity>(Expression<Func<TEntity, bool>> where, string field, string value, bool? bypassDocumentValidation = null)
            where TEntity : MongoEntity
        {
            return this.GetTable<TEntity>().UpdateOne(Builders<TEntity>.Filter.Where(where), Builders<TEntity>.Update.Set(field, value), new UpdateOptions() { IsUpsert = false, BypassDocumentValidation = bypassDocumentValidation }).IsAcknowledged;
        }
        /// <summary>
        /// 异步修改（部分字段）
        /// </summary>
        public virtual async Task<UpdateResult> UpdateAsync<TEntity>(Expression<Func<TEntity, bool>> where, string field, string value, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            return await this.GetTable<TEntity>().UpdateOneAsync(Builders<TEntity>.Filter.Where(where), Builders<TEntity>.Update.Set(field, value), new UpdateOptions() { IsUpsert = false, BypassDocumentValidation = bypassDocumentValidation }, cancellationToken);
        }
        /// <summary>
        /// 批量修改（部分字段）
        /// </summary>
        public virtual bool Update<TEntity>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> update)
            where TEntity : MongoEntity
        {
            var filter = Builders<TEntity>.Filter.Where(where);

            var uList = update.BuildUpdateDefinition();
            if (uList != null)
                return GetTable<TEntity>().UpdateMany(filter, Builders<TEntity>.Update.Combine(uList), new UpdateOptions() { IsUpsert = false }).IsAcknowledged;
            else
                return false;
        }
        /// <summary>
        /// 异步批量修改（部分字段）
        /// </summary>
        public virtual async Task<UpdateResult> UpdateAsync<TEntity>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> update, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            var filter = Builders<TEntity>.Filter.Where(where);

            var uList = update.BuildUpdateDefinition();
            if (uList != null)
                return await GetTable<TEntity>().UpdateManyAsync(filter, Builders<TEntity>.Update.Combine(uList), new UpdateOptions() { IsUpsert = false }, cancellationToken);
            else
                return await Task.FromResult<UpdateResult>(null);
        }

        #endregion

        #region 大批量操作

        /// <summary>
        /// 大批量写入
        /// </summary>
        public virtual BulkWriteResult<TEntity> BulkAdd<TEntity>(IEnumerable<TEntity> models, bool isOrdered = true, bool? bypassDocumentValidation = null)
            where TEntity : MongoEntity
        {
            if (models != null && models.Any())
            {
                var writes = models.ToMap(c => new InsertOneModel<TEntity>(c));

                return BulkWrite<TEntity>(writes, isOrdered, bypassDocumentValidation);
            }
            else
                return null;
        }
        /// <summary>
        /// 异步大批量写入
        /// </summary>
        public virtual async Task<BulkWriteResult<TEntity>> BulkAddAsync<TEntity>(IEnumerable<TEntity> models, bool isOrdered = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            if (models != null && models.Any())
            {
                var writes = models.ToMap(c => new InsertOneModel<TEntity>(c));

                return await BulkWriteAsync<TEntity>(writes, isOrdered, bypassDocumentValidation);
            }
            else
                return null;
        }
        /// <summary>
        /// 大批量操作
        /// </summary>
        public virtual BulkWriteResult<TEntity> BulkWrite<TEntity>(IEnumerable<WriteModel<TEntity>> models, bool isOrdered = true, bool? bypassDocumentValidation = null)
            where TEntity : MongoEntity
        {
            if (models != null && models.Any())
            {
                return GetTable<TEntity>().BulkWrite(models, new BulkWriteOptions
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
        public virtual async Task<BulkWriteResult<TEntity>> BulkWriteAsync<TEntity>(IEnumerable<WriteModel<TEntity>> models, bool isOrdered = true, bool? bypassDocumentValidation = null, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            if (models != null && models.Any())
            {
                return await GetTable<TEntity>().BulkWriteAsync(models, new BulkWriteOptions
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
        public virtual bool DeleteOne<TEntity>(TEntity t)
            where TEntity : MongoEntity
        {
            if (BsonIds.IsEmpty())
                BsonIds = GetBsonIds<TEntity>();

            return GetTable<TEntity>().DeleteOne(Builders<TEntity>.Filter.Eq(BsonIds, t.ObjectId)).IsAcknowledged;
        }
        /// <summary>
        /// 按条件，删除
        /// </summary>
        public virtual TEntity FindOneAndDelete<TEntity>(Expression<Func<TEntity, bool>> where)
            where TEntity : MongoEntity
        {
            return GetTable<TEntity>().FindOneAndDelete<TEntity>(Builders<TEntity>.Filter.Where(where), null);
        }
        /// <summary>
        /// 按条件，异步删除
        /// </summary>
        public virtual async Task<DeleteResult> DeleteOneAsync<TEntity>(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            return await GetTable<TEntity>().DeleteOneAsync(Builders<TEntity>.Filter.Where(where), cancellationToken);
        }
        /// <summary>
        /// 按条件，异步删除
        /// </summary>
        public virtual async Task<DeleteResult> DeleteOneAsync<TEntity>(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            return await GetTable<TEntity>().DeleteOneAsync(filter, cancellationToken);
        }
        /// <summary>
        /// 按条件，批量删除
        /// </summary>
        public virtual bool Delete<TEntity>(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            return GetTable<TEntity>().DeleteMany(Builders<TEntity>.Filter.Where(where), cancellationToken).IsAcknowledged;
        }
        /// <summary>
        /// 按条件，批量删除
        /// </summary>
        public virtual bool Delete<TEntity>(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            return GetTable<TEntity>().DeleteMany(filter, cancellationToken).IsAcknowledged;
        }
        /// <summary>
        /// 按条件，异步批量删除
        /// </summary>
        public virtual async Task<DeleteResult> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            return await GetTable<TEntity>().DeleteManyAsync(Builders<TEntity>.Filter.Where(where), cancellationToken);
        }
        /// <summary>
        /// 按条件，异步批量删除
        /// </summary>
        public virtual async Task<DeleteResult> DeleteAsync<TEntity>(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            return await GetTable<TEntity>().DeleteManyAsync(filter, cancellationToken);
        }

        #endregion

        #region [查询]

        /// <summary>
        /// 获取指定Linq条件总记录数
        /// </summary>
        /// <returns></returns>
        public virtual long GetCount<TEntity>(Expression<Func<TEntity, bool>> where)
            where TEntity : MongoEntity
        {
            return GetTable<TEntity>().CountDocuments(where, null);
        }
        /// <summary>
        /// 获取指定Filter条件总记录数
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual long GetCount<TEntity>(FilterDefinition<TEntity> filter)
            where TEntity : MongoEntity
        {
            return GetTable<TEntity>().CountDocuments(filter, null);
        }
        /// <summary>
        /// 异步获取指定Linq条件总记录数
        /// </summary>
        /// <returns></returns>
        public virtual async Task<long> GetCountAsync<TEntity>(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            return await GetTable<TEntity>().CountDocumentsAsync(where, null, cancellationToken);
        }
        /// <summary>
        /// 异步获取指定Filter条件总记录数
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<long> GetCountAsync<TEntity>(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            return await GetTable<TEntity>().CountDocumentsAsync(filter, null, cancellationToken);
        }
        /// <summary>
        /// 获取指定主键id记录
        /// </summary>
        /// <returns></returns>
        public virtual TEntity GetById<TEntity>(object id)
            where TEntity : MongoEntity
        {
            return GetTable<TEntity>().Find(Builders<TEntity>.Filter.Eq(a => a.ObjectId, id))?.FirstOrDefault();
        }
        /// <summary>
        /// 异步获取指定主键id记录
        /// </summary>
        public virtual async Task<TEntity> GetByIdAsync<TEntity>(object id, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            var list = await GetTable<TEntity>().FindAsync(Builders<TEntity>.Filter.Eq(a => a.ObjectId, id), null, cancellationToken);
            return list?.FirstOrDefault(cancellationToken: cancellationToken);
        }
        /// <summary>
        /// 获得指定Linq条件内的单条数据
        /// </summary>
        public virtual TEntity GetSingle<TEntity>(Expression<Func<TEntity, bool>> where)
            where TEntity : MongoEntity
        {
            return GetTable<TEntity>().Find(where)?.FirstOrDefault();
        }
        /// <summary>
        /// 获得指定Filter内的单条数据
        /// </summary>
        public virtual TEntity GetSingle<TEntity>(FilterDefinition<TEntity> filter)
            where TEntity : MongoEntity
        {
            return GetTable<TEntity>().Find(filter)?.FirstOrDefault();
        }
        /// <summary>
        /// 异步获得指定Linq条件内的单条数据
        /// </summary>
        public virtual async Task<TEntity> GetSingleAsync<TEntity>(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            var list = await GetTable<TEntity>().FindAsync(where, null, cancellationToken);
            return list?.FirstOrDefault(cancellationToken: cancellationToken);
        }
        /// <summary>
        /// 异步获得指定Filter内的单条数据
        /// </summary>
        public virtual async Task<TEntity> GetSingleAsync<TEntity>(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            var list = await GetTable<TEntity>().FindAsync(filter, null, cancellationToken);
            return list?.FirstOrDefault(cancellationToken: cancellationToken);
        }
        /// <summary>
        /// 获取当前所有集合
        /// </summary>
        /// <returns></returns>
        public virtual IList<TEntity> GetList<TEntity>()
            where TEntity : MongoEntity
        {
            return GetTable<TEntity>().Find(t => true)?.ToList();
        }
        /// <summary>
        /// 异步获取当前所有集合
        /// </summary>
        /// <returns>返回异步结果</returns>
        public virtual async Task<IList<TEntity>> GetListAsync<TEntity>(CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            var list = await GetTable<TEntity>().FindAsync(t => true, null, cancellationToken);
            return list?.ToList(cancellationToken: cancellationToken);
        }
        /// <summary>
        /// 获得指定Linq条件内的多条数据
        /// </summary>
        public virtual List<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> where, string orderby = "", int? limit = null)
            where TEntity : MongoEntity
        {
            return GetTable<TEntity>().Find(where).Sort(orderby.OrderByBatch<TEntity>()).Limit(limit).ToList();
        }
        /// <summary>
        /// 获得指定Filter内的多条数据
        /// </summary>
        public virtual List<TEntity> GetList<TEntity>(FilterDefinition<TEntity> filter, string orderby = "", int? limit = null)
            where TEntity : MongoEntity
        {
            return GetTable<TEntity>().Find(filter).Sort(orderby.OrderByBatch<TEntity>()).Limit(limit).ToList();
        }
        /// <summary>
        /// 异步获得指定Linq条件内的多条数据
        /// </summary>
        public virtual async Task<List<TEntity>> GetListAsync<TEntity>(Expression<Func<TEntity, bool>> where, string orderby = "", int? limit = null, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            var list = await GetTable<TEntity>().FindAsync(where, new FindOptions<TEntity, TEntity>()
            {
                Limit = limit,
                Sort = orderby.OrderByBatch<TEntity>()
            }, cancellationToken);
            return list?.ToList(cancellationToken: cancellationToken);
        }
        /// <summary>
        /// 异步获得指定Filter内的多条数据
        /// </summary>
        public virtual async Task<List<TEntity>> GetListAsync<TEntity>(FilterDefinition<TEntity> filter, string orderby = "", int? limit = null, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            var list = await GetTable<TEntity>().FindAsync(filter, new FindOptions<TEntity, TEntity>()
            {
                Limit = limit,
                Sort = orderby.OrderByBatch<TEntity>()
            }, cancellationToken);
            return list?.ToList(cancellationToken: cancellationToken);
        }
        /// <summary>
        /// 分页获取数据
        /// </summary>
        public virtual PagedList<TEntity> GetPagedList<TEntity>(Expression<Func<TEntity, bool>> selector, string orderby, int currentPage, int pageSize)
            where TEntity : MongoEntity
        {
            currentPage = currentPage < 1 ? 1 : currentPage;

            var listTable = GetTable<TEntity>();

            var total = listTable.CountDocuments(selector, null);

            var pageList = listTable.Find(selector)?.Sort(orderby.OrderByBatch<TEntity>()).Skip((currentPage - 1) * pageSize).Limit(pageSize).ToList();
            return new PagedList<TEntity>(pageList, total, currentPage, pageSize);
        }
        /// <summary>
        /// 分页获取数据
        /// </summary>
        public virtual PagedList<TEntity> GetPagedList<TEntity>(FilterDefinition<TEntity> filter, string orderby, int currentPage, int pageSize)
            where TEntity : MongoEntity
        {
            currentPage = currentPage < 1 ? 1 : currentPage;

            var listTable = GetTable<TEntity>();

            var total = listTable.CountDocuments(filter, null);
            var pageList = listTable.Find(filter)?.Sort(orderby.OrderByBatch<TEntity>()).Skip((currentPage - 1) * pageSize).Limit(pageSize).ToList();

            return new PagedList<TEntity>(pageList, total, currentPage, pageSize);
        }
        /// <summary>
        /// 异步分页获取数据
        /// </summary>
        public virtual async Task<PagedList<TEntity>> GetPagedListAsync<TEntity>(Expression<Func<TEntity, bool>> selector, string orderby, int currentPage, int pageSize, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            currentPage = currentPage < 1 ? 1 : currentPage;

            var listTable = GetTable<TEntity>();

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
        public virtual async Task<PagedList<TEntity>> GetPagedListAsync<TEntity>(FilterDefinition<TEntity> filter, string orderby, int currentPage, int pageSize, CancellationToken cancellationToken = default)
            where TEntity : MongoEntity
        {
            currentPage = currentPage < 1 ? 1 : currentPage;

            var listTable = GetTable<TEntity>();

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
