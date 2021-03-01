using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Develops;
using XUCore.Extensions;
using XUCore.Helpers;

namespace XUCore.NetCore.Mongo.Test
{
    public class MainService : IHostedService
    {
        private readonly ILogger logger;
        private readonly IMongoServiceProvider<UserMongoModel> mongoServiceProvider;
        public MainService(ILogger<MainService> logger, IMongoServiceProvider<UserMongoModel> mongoServiceProvider)
        {
            this.logger = logger;
            this.mongoServiceProvider = mongoServiceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            {
                /*
                 
                创建Mongo索引

                try { db.getCollection("comment").ensureIndex({ "AutoId":1},{ background: 1, unique: false })} catch (e) { print(e)}
                try { db.getCollection("comment").ensureIndex({ "Status":1},{ background: 1, unique: false })} catch (e) { print(e)}
                try { db.getCollection("comment").ensureIndex({ "CreateTime":1},{ background: 1, unique: false })} catch (e) { print(e)}
                try { db.getCollection("comment").ensureIndex({ "Source":1},{ background: 1, unique: false })} catch (e) { print(e)}
                try { db.getCollection("comment").ensureIndex({ "CommentType":1},{ background: 1, unique: false })} catch (e) { print(e)}
                try { db.getCollection("comment").ensureIndex({ "ThemeId":1},{ background: 1, unique: false })} catch (e) { print(e)}
                try { db.getCollection("comment").ensureIndex({ "FromUserId":1},{ background: 1, unique: false })} catch (e) { print(e)}
                try { db.getCollection("comment").ensureIndex({ "ToUserId":1},{ background: 1, unique: false })} catch (e) { print(e)}
                try { db.getCollection("comment").ensureIndex({ "ToCommentId":1},{ background: 1, unique: false })} catch (e) { print(e)}
                try { db.getCollection("comment").ensureIndex({ "ParentCommentId":1},{ background: 1, unique: false })} catch (e) { print(e)}
                try { db.getCollection("comment").ensureIndex({ "Score":1},{ background: 1, unique: false })} catch (e) { print(e)}

                */
            }
            //{
            //    //添加
            //    var works = new List<WorkModel> {
            //        new WorkModel{ Year = 2021, CompanyName = "腾讯" },
            //    };
            //    var model = new UserMongoModel
            //    {
            //        AutoId = 2,
            //        Name = "王五",
            //        Age = 22,
            //        Birthday = DateTime.Parse("2000-01-30"),
            //        Works = works
            //    };
            //    await mongoServiceProvider.AddAsync(model, cancellationToken: cancellationToken);
            //}
            {
                //批量写入测试

                100.Times(num =>
                {
                    var models = new List<UserMongoModel>();

                    10000.Times(c =>
                    {
                        models.Add(new UserMongoModel
                        {
                            AutoId = Id.NewLong(),
                            Name = $"王五{c + 1}",
                            Age = 22,
                            Birthday = DateTime.Parse("2000-01-30"),
                            Works = new List<WorkModel> {
                            new WorkModel{ Year = 2021, CompanyName = "腾讯" },
                        }
                        });
                    });

                    var watch = Stopwatch.StartNew();

                    var res = mongoServiceProvider.BulkAdd(models);

                    watch.Stop();

                    Console.WriteLine(watch.Elapsed);
                });
            }
            //{
            //    //查询所有记录
            //    var allList = await mongoServiceProvider.GetListAsync(cancellationToken);
            //}
            //{
            //    //根据条件查询记录
            //    var builders = Builders<UserMongoModel>.Filter;
            //    var filters = new List<FilterDefinition<UserMongoModel>>();
            //    Expression<Func<UserMongoModel, bool>> selector = c => c.Age > 25;
            //    //如果没有对子集的in查询，那么不需要拼接builders
            //    filters.Add(builders.Where(selector));
            //    //子集列表做in查询
            //    filters.Add(builders.ElemMatch(c => c.Works, Builders<WorkModel>.Filter.Where(c => c.Year == 2000)));
            //    var allFilters = builders.And(filters);
            //    var list = await mongoServiceProvider.GetListAsync(allFilters);
            //}
            //{
            //    //查询分页
            //    Expression<Func<UserMongoModel, bool>> selector = c => c.Age > 20;

            //    var pageModel = await mongoServiceProvider.GetPagedListAsync(selector, "AutoId desc", 1, 1, cancellationToken);
            //}
            //{
            //    //统计 aggregate
            //    //求记录数
            //    var stages = new List<BsonDocument> {
            //        {
            //            new BsonDocument {
            //                { "$match", new BsonDocument {
            //                    { "Age",new BsonDocument {
            //                        { "$gte" , 25 }
            //                    }}
            //                }}
            //            }
            //        },
            //        {
            //            new BsonDocument {
            //                { "$count", "total"}
            //            }
            //        }
            //    };

            //    var pipe = PipelineDefinition<UserMongoModel, BsonDocument>.Create(stages);

            //    var obj = mongoServiceProvider.Table.Aggregate(pipe, new AggregateOptions { AllowDiskUse = true }).FirstOrDefault();

            //    if (obj != null && obj.Contains("total"))
            //    {
            //        var total = obj["total"].ToInt64();
            //    }
            //}
            //{
            //    //统计  aggregate
            //    //不需要分组，直接求年龄总和
            //    var stages = new List<BsonDocument> {
            //        {
            //            new BsonDocument {
            //                { "$match", new BsonDocument {
            //                    { "Age",new BsonDocument {
            //                        { "$gte" , 20 }
            //                    }}
            //                }}
            //            }
            //        },
            //        {
            //            new BsonDocument {
            //                { "$group", new BsonDocument {
            //                    { "_id" , 1 },
            //                    { "total" , new BsonDocument {
            //                        { "$sum" , "$Age" }
            //                    }}
            //                }}
            //            }
            //        }
            //    };

            //    var pipe = PipelineDefinition<UserMongoModel, BsonDocument>.Create(stages);

            //    var obj = mongoServiceProvider.Table.Aggregate(pipe, new AggregateOptions { AllowDiskUse = true }).FirstOrDefault();

            //    if (obj != null && obj.Contains("total"))
            //    {
            //        var total = obj["total"].ToInt64();
            //    }

            //}
            //{
            //    //统计  aggregate
            //    //根据UserId去重后统计用户总数
            //    var stages = new List<BsonDocument> {
            //        {
            //            new BsonDocument {
            //                { "$match", new BsonDocument {
            //                    { "ExposalId", exposalId },
            //                    { "VirtualMode", virtualMode }
            //                }}
            //            }
            //        },
            //        {
            //            new BsonDocument {
            //                { "$group", new BsonDocument {
            //                    { "_id" , "$UserId" }
            //                }}
            //            }
            //        },
            //        {
            //            new BsonDocument {
            //                { "$count", "total"}
            //            }
            //        }
            //    };

            //    var pipe = PipelineDefinition<UserMongoModel, BsonDocument>.Create(stages);

            //    var obj = mongoServiceProvider.Table.Aggregate(pipe, new AggregateOptions { AllowDiskUse = true }).FirstOrDefault();

            //    if (obj != null && obj.Contains("total"))
            //        return obj["total"].ToInt64();
            //}
            //{
            //    //统计  aggregate
            //    / 该示例用于参考 实现group去重后，并获取第一条最新的记录的objectid列表
            //     var stages = new List<BsonDocument>();
            //    if (type > 0)
            //    {
            //        stages.Add(
            //            new BsonDocument {
            //                { "$match", new BsonDocument {
            //                    { "PeriodsId", periodsId },
            //                    { "Type", type }
            //                }}
            //            }
            //        );
            //    }
            //    else
            //    {
            //        stages.Add(
            //            new BsonDocument {
            //                { "$match", new BsonDocument {
            //                    { "PeriodsId", periodsId }
            //                }}
            //            }
            //        );
            //    }
            //    stages.AddRange(
            //        new List<BsonDocument> {
            //            new BsonDocument {
            //                { "$sort", new BsonDocument {
            //                    { "ThumbTime", -1 }
            //                }}
            //            },
            //            new BsonDocument {
            //                { "$group", new BsonDocument {
            //                    {
            //                        "_id" , new BsonDocument{
            //                            { "UserId", "$UserId" }
            //                        }
            //                    },
            //                    {
            //                        "ids" , new BsonDocument{
            //                            { "$addToSet", "$_id" }
            //                        }
            //                    }
            //                }}
            //            },
            //            new BsonDocument {
            //                { "$sort", new BsonDocument {
            //                    { "ids" , -1 }
            //                }}
            //            },
            //            new BsonDocument {
            //                { "$limit", limit }
            //            }
            //        }
            //    );

            //    var pipe = PipelineDefinition<UserMongoModel, BsonDocument>.Create(stages);

            //    var objs = mongoServiceProvider.Table.Aggregate(pipe, new AggregateOptions { AllowDiskUse = true }).ToList();

            //    var objIds = new List<string>();

            //    foreach (var obj in objs)
            //    {
            //        var array = obj["ids"].AsBsonArray;

            //        var _objIds = array.ToMap(c => c.AsObjectId.ToString()).OrderByDescending(c => c).ToList();

            //        objIds.Add(_objIds[0]);
            //    }

            //    return objIds;
            //}
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
