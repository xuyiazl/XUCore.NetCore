using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace XUCore.Extensions
{
    /// <summary>
    /// 程序集(<see cref="Assembly"/>) 扩展
    /// </summary>
    public static class AssemblyExtensions
    {
        #region GetFileVersion(获取程序集的文件版本)

        /// <summary>
        /// 获取程序集的文件版本
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        public static Version GetFileVersion(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new Version(info.FileVersion);
        }

        #endregion GetFileVersion(获取程序集的文件版本)

        #region GetProductVersion(获取程序集的产品版本)

        /// <summary>
        /// 获取程序集的产品版本
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        public static Version GetProductVersion(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new Version(info.ProductVersion);
        }

        #endregion GetProductVersion(获取程序集的产品版本)

        #region [GetTypes(获取程序集中的类型)]
        /// <summary>
        /// 扫描程序集中的指定条件类型
        /// </summary>
        /// <param name="assemblys">程序集列表</param>
        /// <param name="selector">检索条件</param>
        /// <returns></returns>
        public static IList<Type> GetTypes(this Assembly[] assemblys, Func<Type, bool> selector) =>
            (
                from assembly in assemblys
                from type in assembly.GetTypes()
                select type
            )
            .Where(selector)
            .ToList();
        /// <summary>
        /// 扫描程序集中的指定条件类型
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IList<Type> GetTypes(this Assembly assembly, Func<Type, bool> selector) => assembly.GetTypes().Where(selector).ToList();
        /// <summary>
        /// 是否包含指定类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        public static bool AnyBaseType(this Type type, Type baseType) => type.GetBaseTypes().Any(c => c.IsParticularGeneric(baseType));
        /// <summary>
        /// 是否是指定的类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="generic"></param>
        /// <returns></returns>
        public static bool IsParticularGeneric(this Type type, Type generic) => type.IsGenericType && type.GetGenericTypeDefinition() == generic;
        /// <summary>
        /// 获取所有不等于空的baseType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            Type t = type;
            while (true)
            {
                t = t.BaseType;
                if (t == null) break;
                yield return t;
            }
        }
        #endregion[GetTypes(获取程序集中的类型)]
    }
}