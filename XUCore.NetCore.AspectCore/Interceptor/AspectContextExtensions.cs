using AspectCore.DynamicProxy;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.AspectCore.Interceptor
{
    internal static class AspectContextExtensions
    {
        private static readonly ConcurrentDictionary<Type, MethodInfo> TypeofTaskResultMethod = new ConcurrentDictionary<Type, MethodInfo>();

        private static readonly ConcurrentDictionary<MethodInfo, object[]>
                    MethodAttributes = new ConcurrentDictionary<MethodInfo, object[]>();

        public static Type GetReturnType(this AspectContext context)
        {
            return context.IsAsync()
                    ? context.ServiceMethod.ReturnType.GetGenericArguments().First()
                    : context.ServiceMethod.ReturnType;
        }

        public static T GetAttribute<T>(this AspectContext context) where T : Attribute
        {
            MethodInfo method = context.ServiceMethod;
            var attributes = MethodAttributes.GetOrAdd(method, method.GetCustomAttributes(true));
            var attribute = attributes.FirstOrDefault(x => typeof(T).IsAssignableFrom(x.GetType()));
            if (attribute is T)
            {
                return (T)attribute;
            }
            return null;
        }

        public static async Task<object> GetReturnValue(this AspectContext context)
        {
            return context.IsAsync() ? await context.UnwrapAsyncReturnValue() : context.ReturnValue;
        }

        public static object ResultFactory(this AspectContext context, object result, Type returnType, bool isAsync)
        {
            if (isAsync)
            {
                return TypeofTaskResultMethod
                    .GetOrAdd(returnType, t => typeof(Task)
                    .GetMethods()
                    .First(p => p.Name == "FromResult" && p.ContainsGenericParameters)
                    .MakeGenericMethod(returnType))
                    .Invoke(null, new object[] { result });
            }
            else
            {
                return result;
            }
        }
    }
}
