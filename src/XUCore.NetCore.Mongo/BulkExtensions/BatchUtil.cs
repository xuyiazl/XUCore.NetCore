using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using XUCore.Extensions;

namespace XUCore.NetCore.Mongo
{
    internal static class BatchUtil
    {
        public static TModel GetUpdateMap<TModel>(this IMongoCollection<TModel> table, TModel newModel, bool isUpsert, FilterDefinition<TModel> filter = null)
        {
            var uModel = table.Find(filter).ToList().FirstOrDefault();
            if (uModel != null)
            {
                foreach (var prop in newModel.GetType().GetProperties())
                {
                    //找到BsonId以后跳出
                    if (prop.GetCustomAttributes().Any(t => t.GetType() == typeof(BsonIdAttribute)))
                        continue;

                    var newValue = prop.GetValue(newModel);
                    var oldValue = uModel.GetType().GetProperty(prop.Name).GetValue(uModel);
                    if (newValue != null && !Convert.ToString(newValue).Equals(Convert.ToString(oldValue)))
                        uModel.GetType().GetProperty(prop.Name).SetValue(uModel, newValue);
                }

                return uModel;
            }
            else
                return isUpsert ? newModel : uModel;
        }

        public static List<UpdateDefinition<TModel>> BuildUpdateDefinition<TModel>(this object doc, string parent = null)
        {
            var updateList = new List<UpdateDefinition<TModel>>();
            foreach (var property in typeof(TModel).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                //找到BsonId以后跳出
                if (property.GetCustomAttributes().Any(t => t.GetType() == typeof(BsonIdAttribute)))
                    continue;

                var key = parent == null ? property.Name : string.Format("{0}.{1}", parent, property.Name);
                updateList.Add(Builders<TModel>.Update.Set(key, property.GetValue(doc)));
            }

            return updateList;
        }

        public static List<UpdateDefinition<TModel>> BuildUpdateDefinition<TModel>(this Expression<Func<TModel, TModel>> entity)
        {
            var fieldList = new List<UpdateDefinition<TModel>>();

            var param = entity.Body as MemberInitExpression;
            foreach (var item in param.Bindings)
            {
                if (item.Member.GetCustomAttributes().Any(t => t.GetType() == typeof(BsonIdAttribute)))
                    continue;

                object propertyValue;
                var memberAssignment = item as MemberAssignment;
                if (memberAssignment.Expression.NodeType == ExpressionType.Constant)
                {
                    propertyValue = (memberAssignment.Expression as ConstantExpression).Value;
                }
                else
                {
                    propertyValue = Expression.Lambda(memberAssignment.Expression, null).Compile().DynamicInvoke();
                }

                fieldList.Add(Builders<TModel>.Update.Set(item.Member.Name, propertyValue));
            }

            return fieldList;
        }

        public static SortDefinition<TModel> OrderByBatch<TModel>(this string orderby)
        {
            if (orderby.IsEmpty()) return null;

            SortDefinitionBuilder<TModel> builderSort = Builders<TModel>.Sort;
            SortDefinition<TModel> sort = null;

            var a = orderby.ToMap(',', ' ', false, false, true);
            foreach (var item in a)
            {
                if (item.Value.ToLower().Equals("desc"))
                    sort = sort != null ? sort.Descending(item.Key) : builderSort.Descending(item.Key);
                else
                    sort = sort != null ? sort.Ascending(item.Key) : builderSort.Ascending(item.Key);
            }

            return sort;
        }
    }
}
