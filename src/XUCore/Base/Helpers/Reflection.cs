﻿using XUCore.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;
using System.Runtime.Loader;

namespace XUCore.Helpers
{
    /// <summary>
    /// 反射 操作
    /// </summary>
    public static class Reflection
    {
        #region GetDescription(获取类型描述)

        /// <summary>
        /// 获取类型描述，使用<see cref="DescriptionAttribute"/>设置描述
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public static string GetDescription<T>() => GetDescription(Common.GetType<T>());

        /// <summary>
        /// 获取类型成员描述，使用<see cref="DescriptionAttribute"/>设置描述
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="memberName">成员名称</param>
        public static string GetDescription<T>(string memberName) => GetDescription(Common.GetType<T>(), memberName);

        /// <summary>
        /// 获取类型成员描述，使用<see cref="DescriptionAttribute"/>设置描述
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="memberName">成员名称</param>
        public static string GetDescription(Type type, string memberName)
        {
            if (type == null)
                return string.Empty;
            return memberName.IsEmpty()
                ? string.Empty
                : GetDescription(type.GetTypeInfo().GetMember(memberName).FirstOrDefault());
        }

        /// <summary>
        /// 获取类型成员描述，使用<see cref="DescriptionAttribute"/>设置描述
        /// </summary>
        /// <param name="member">成员</param>
        public static string GetDescription(MemberInfo member)
        {
            if (member == null)
                return string.Empty;
            return member.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute attribute
                ? attribute.Description
                : member.Name;
        }

        #endregion GetDescription(获取类型描述)

        #region GetDisplayName(获取类型显示名称)

        /// <summary>
        /// 获取类型显示名称，使用<see cref="DisplayNameAttribute"/>设置显示名称
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public static string GetDisplayName<T>() => GetDisplayName(Common.GetType<T>());

        /// <summary>
        /// 获取类型成员显示名称，，使用<see cref="DisplayNameAttribute"/>或<see cref="DisplayAttribute"/>设置显示名称
        /// </summary>
        /// <param name="member">成员</param>
        /// <returns></returns>
        private static string GetDisplayName(MemberInfo member)
        {
            if (member == null)
                return string.Empty;
            if (member.GetCustomAttribute<DisplayAttribute>() is DisplayAttribute displayAttribute)
                return displayAttribute.Description; ;
            if (member.GetCustomAttribute<DisplayNameAttribute>() is DisplayNameAttribute displayNameAttribute)
                return displayNameAttribute.DisplayName;
            return string.Empty;
        }

        #endregion GetDisplayName(获取类型显示名称)

        #region GetDisplayNameOrDescription(获取显示名称或类型描述)

        /// <summary>
        /// 获取类型显示名称或描述，使用<see cref="DescriptionAttribute"/>设置描述，使用<see cref="DisplayNameAttribute"/>设置显示名称
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public static string GetDisplayNameOrDescription<T>()
        {
            return GetDisplayNameOrDescription(Common.GetType<T>());
        }

        /// <summary>
        /// 获取类型显示名称或成员描述，使用<see cref="DescriptionAttribute"/>设置描述，使用<see cref="DisplayNameAttribute"/>或<see cref="DisplayAttribute"/>设置显示名称
        /// </summary>
        /// <param name="member">成员</param>
        /// <returns></returns>
        public static string GetDisplayNameOrDescription(MemberInfo member)
        {
            var result = GetDisplayName(member);
            return string.IsNullOrWhiteSpace(result) ? GetDescription(member) : result;
        }

        #endregion GetDisplayNameOrDescription(获取显示名称或类型描述)

        #region FindTypes(查找类型列表)

        /// <summary>
        /// 查找类型列表
        /// </summary>
        /// <typeparam name="TFind">查找类型</typeparam>
        /// <param name="assemblies">待查找的程序集列表</param>
        /// <returns></returns>
        public static List<Type> FindTypes<TFind>(params Assembly[] assemblies)
        {
            var findType = typeof(TFind);
            return FindTypes(findType, assemblies);
        }

        /// <summary>
        /// 查找类型列表
        /// </summary>
        /// <param name="findType">查找类型</param>
        /// <param name="assemblies">待查找的程序集列表</param>
        /// <returns></returns>
        public static List<Type> FindTypes(Type findType, params Assembly[] assemblies)
        {
            var result = new List<Type>();
            foreach (var assembly in assemblies)
            {
                result.AddRange(GetTypes(findType, assembly));
            }

            return result.Distinct().ToList();
        }

        /// <summary>
        /// 获取类型列表
        /// </summary>
        /// <param name="findType">查找类型</param>
        /// <param name="assembly">待查找的程序集</param>
        /// <returns></returns>
        private static List<Type> GetTypes(Type findType, Assembly assembly)
        {
            var result = new List<Type>();
            if (assembly == null)
            {
                return result;
            }

            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException)
            {
                return result;
            }

            foreach (var type in types)
            {
                AddType(result, findType, type);
            }

            return result;
        }

        /// <summary>
        /// 添加类型
        /// </summary>
        /// <param name="result">类型列表</param>
        /// <param name="findType">查找类型</param>
        /// <param name="type">类型</param>
        private static void AddType(List<Type> result, Type findType, Type type)
        {
            if (type.IsInterface || type.IsAbstract)
            {
                return;
            }

            if (findType.IsAssignableFrom(type) == false && MatchGeneric(findType, type) == false)
            {
                return;
            }

            result.Add(type);
        }

        /// <summary>
        /// 泛型匹配
        /// </summary>
        /// <param name="findType">查找类型</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private static bool MatchGeneric(Type findType, Type type)
        {
            if (findType.IsGenericTypeDefinition == false)
            {
                return false;
            }

            var definition = findType.GetGenericTypeDefinition();
            foreach (var implementedInterface in type.FindInterfaces((filiter, criteria) => true, null))
            {
                if (implementedInterface.IsGenericType == false)
                {
                    continue;
                }

                return definition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());
            }

            return false;
        }

        #endregion FindTypes(查找类型列表)

        #region GetInstancesByInterface(获取实现了接口的所有实例)

        /// <summary>
        /// 获取实现了接口的所有实例
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <param name="assembly">在该程序集中查找</param>
        public static List<TInterface> GetInstancesByInterface<TInterface>(Assembly assembly)
        {
            var typeInterface = typeof(TInterface);
            return
                assembly.GetTypes()
                    .Where(
                        t =>
                            typeInterface.GetTypeInfo().IsAssignableFrom(t) && t != typeInterface &&
                            t.GetTypeInfo().IsAbstract == false)
                    .Select(t => CreateInstance<TInterface>(t))
                    .ToList();
        }

        #endregion GetInstancesByInterface(获取实现了接口的所有实例)

        #region CreateInstance(动态创建实例)

        /// <summary>
        /// 动态创建实例
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="type">类型</param>
        /// <param name="parameters">传递给构造函数的参数</param>
        public static T CreateInstance<T>(Type type, params object[] parameters) => Conv.To<T>(Activator.CreateInstance(type, parameters));

        /// <summary>
        /// 动态创建实例
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="className">类名，包括命名空间,如果类型不处于当前执行程序集中，需要包含程序集名，范例：Test.Core.Test2,Test.Core</param>
        /// <param name="parameters">传递给构造函数的参数</param>
        public static T CreateInstance<T>(string className, params object[] parameters)
        {
            var type = Type.GetType(className) ?? Assembly.GetCallingAssembly().GetType(className);
            return CreateInstance<T>(type, parameters);
        }

        #endregion CreateInstance(动态创建实例)

        //#region GetAssembly(获取程序集)

        ///// <summary>
        ///// 获取程序集
        ///// </summary>
        ///// <param name="assemblyName">程序集名称</param>
        ///// <returns></returns>
        //public static Assembly GetAssembly(string assemblyName) => Assembly.Load(new AssemblyName(assemblyName));

        //#endregion GetAssembly(获取程序集)

        #region GetAssemblies(从目录获取所有程序集)

        /// <summary>
        /// 从目录获取所有程序集
        /// </summary>
        /// <param name="directoryPath">目录绝对路径</param>
        /// <returns></returns>
        public static List<Assembly> GetAssemblies(string directoryPath)
        {
            return
                Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                    .ToList()
                    .Where(t => t.EndsWith(".exe") || t.EndsWith(".dll"))
                    .Select(path => Assembly.Load(new AssemblyName(path)))
                    .ToList();
        }

        #endregion GetAssemblies(从目录获取所有程序集)

        #region GetCurrentProjectAssemblies(获取当前项目中所有程序集)

        /// <summary>
        /// 获取当前项目中所有程序集
        /// </summary>
        /// <param name="filter">过滤条件，StartsWith</param>
        /// <returns></returns>
        public static List<Assembly> GetCurrentProjectAssemblies(string filter)
        {
            var list = new List<Assembly>();
            var deps = DependencyContext.Default;
            var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package" && lib.Name.StartsWith(filter));
            foreach (var lib in libs)
            {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                list.Add(assembly);
            }
            return list;
        }

        #endregion GetCurrentProjectAssemblies(获取当前项目中所有程序集)

        #region GetCurrentAssemblyName(获取当前程序集名称)

        /// <summary>
        /// 获取当前程序集名称
        /// </summary>
        public static string GetCurrentAssemblyName() => Assembly.GetCallingAssembly().GetName().Name;

        #endregion GetCurrentAssemblyName(获取当前程序集名称)

        #region GetAttribute(获取特性信息)

        /// <summary>
        /// 获取特性信息
        /// </summary>
        /// <typeparam name="TAttribute">泛型特性</typeparam>
        /// <param name="memberInfo">元数据</param>
        public static TAttribute GetAttribute<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
        {
            return (TAttribute)memberInfo.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault();
        }

        #endregion GetAttribute(获取特性信息)

        #region GetAttributes(获取特性信息数据)

        /// <summary>
        /// 获取特性信息数组
        /// </summary>
        /// <typeparam name="TAttribute">泛型特性</typeparam>
        /// <param name="memberInfo">元数据</param>
        /// <returns></returns>
        public static TAttribute[] GetAttributes<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
        {
            return Array.ConvertAll(memberInfo.GetCustomAttributes(typeof(TAttribute), false), x => (TAttribute)x);
        }

        #endregion GetAttributes(获取特性信息数据)

        #region GetPropertyInfo(获取属性信息)

        /// <summary>
        /// 获取属性信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>存在时返回PropertyInfo，不存在时返回null</returns>
        public static PropertyInfo GetPropertyInfo(Type type, string propertyName) => type.GetProperties().FirstOrDefault(p => p.Name.Equals(propertyName));

        #endregion GetPropertyInfo(获取属性信息)

        #region IsBool(是否布尔类型)

        /// <summary>
        /// 是否布尔类型
        /// </summary>
        /// <param name="member">成员</param>
        /// <returns></returns>
        public static bool IsBool(MemberInfo member)
        {
            if (member == null)
            {
                return false;
            }
            switch (member.MemberType)
            {
                case MemberTypes.TypeInfo:
                    return member.ToString() == "System.Boolean";

                case MemberTypes.Property:
                    return IsBool((PropertyInfo)member);
            }
            return false;
        }

        /// <summary>
        /// 是否布尔类型
        /// </summary>
        /// <param name="property">属性</param>
        /// <returns></returns>
        public static bool IsBool(PropertyInfo property)
        {
            return property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?);
        }

        #endregion IsBool(是否布尔类型)

        #region IsEnum(是否枚举类型)

        /// <summary>
        /// 是否枚举类型
        /// </summary>
        /// <param name="member">成员</param>
        /// <returns></returns>
        public static bool IsEnum(MemberInfo member)
        {
            if (member == null)
            {
                return false;
            }
            switch (member.MemberType)
            {
                case MemberTypes.TypeInfo:
                    return ((TypeInfo)member).IsEnum;

                case MemberTypes.Property:
                    return IsEnum((PropertyInfo)member);
            }
            return false;
        }

        /// <summary>
        /// 是否枚举类型
        /// </summary>
        /// <param name="property">属性</param>
        /// <returns></returns>
        public static bool IsEnum(PropertyInfo property)
        {
            if (property.PropertyType.GetTypeInfo().IsEnum)
            {
                return true;
            }
            var value = Nullable.GetUnderlyingType(property.PropertyType);
            if (value == null)
            {
                return false;
            }
            return value.GetTypeInfo().IsEnum;
        }

        #endregion IsEnum(是否枚举类型)

        #region IsDate(是否日期类型)

        /// <summary>
        /// 是否日期类型
        /// </summary>
        /// <param name="member">成员</param>
        /// <returns></returns>
        public static bool IsDate(MemberInfo member)
        {
            if (member == null)
            {
                return false;
            }
            switch (member.MemberType)
            {
                case MemberTypes.TypeInfo:
                    return member.ToString() == "System.DateTime";

                case MemberTypes.Property:
                    return IsDate((PropertyInfo)member);
            }
            return false;
        }

        /// <summary>
        /// 是否日期类型
        /// </summary>
        /// <param name="property">属性</param>
        /// <returns></returns>
        public static bool IsDate(PropertyInfo property)
        {
            if (property.PropertyType == typeof(DateTime))
            {
                return true;
            }
            if (property.PropertyType == typeof(DateTime?))
            {
                return true;
            }
            return false;
        }

        #endregion IsDate(是否日期类型)

        #region IsInt(是否整型)

        /// <summary>
        /// 是否整型
        /// </summary>
        /// <param name="member">成员</param>
        /// <returns></returns>
        public static bool IsInt(MemberInfo member)
        {
            if (member == null)
            {
                return false;
            }
            switch (member.MemberType)
            {
                case MemberTypes.TypeInfo:
                    return member.ToString() == "System.Int32" || member.ToString() == "System.Int16" ||
                           member.ToString() == "System.Int64";

                case MemberTypes.Property:
                    return IsInt((PropertyInfo)member);
            }
            return false;
        }

        /// <summary>
        /// 是否整型
        /// </summary>
        /// <param name="property">成员</param>
        /// <returns></returns>
        public static bool IsInt(PropertyInfo property)
        {
            if (property.PropertyType == typeof(int))
            {
                return true;
            }
            if (property.PropertyType == typeof(int?))
            {
                return true;
            }
            if (property.PropertyType == typeof(short))
            {
                return true;
            }
            if (property.PropertyType == typeof(short?))
            {
                return true;
            }
            if (property.PropertyType == typeof(long))
            {
                return true;
            }
            if (property.PropertyType == typeof(long?))
            {
                return true;
            }
            return false;
        }

        #endregion IsInt(是否整型)

        #region IsNumber(是否数值类型)

        /// <summary>
        /// 是否数值类型
        /// </summary>
        /// <param name="member">成员</param>
        /// <returns></returns>
        public static bool IsNumber(MemberInfo member)
        {
            if (member == null)
            {
                return false;
            }

            if (IsInt(member))
            {
                return true;
            }
            switch (member.MemberType)
            {
                case MemberTypes.TypeInfo:
                    return member.ToString() == "System.Double" || member.ToString() == "System.Decimal" ||
                           member.ToString() == "System.Single";

                case MemberTypes.Property:
                    return IsNumber((PropertyInfo)member);
            }
            return false;
        }

        /// <summary>
        /// 是否数值类型
        /// </summary>
        /// <param name="property">属性</param>
        /// <returns></returns>
        public static bool IsNumber(PropertyInfo property)
        {
            if (property.PropertyType == typeof(double))
            {
                return true;
            }
            if (property.PropertyType == typeof(double?))
            {
                return true;
            }
            if (property.PropertyType == typeof(decimal))
            {
                return true;
            }
            if (property.PropertyType == typeof(decimal?))
            {
                return true;
            }
            if (property.PropertyType == typeof(float))
            {
                return true;
            }
            if (property.PropertyType == typeof(float?))
            {
                return true;
            }
            return false;
        }

        #endregion IsNumber(是否数值类型)

        #region IsCollection(是否集合)

        /// <summary>
        /// 是否集合
        /// </summary>
        /// <param name="type">类型</param>
        public static bool IsCollection(Type type) => type.IsArray || IsGenericCollection(type);

        #endregion IsCollection(是否集合)

        #region IsGenericCollection(是否泛型集合)

        /// <summary>
        /// 是否泛型集合
        /// </summary>
        /// <param name="type">类型</param>
        public static bool IsGenericCollection(Type type)
        {
            if (!type.IsGenericType)
                return false;
            var typeDefinition = type.GetGenericTypeDefinition();
            return typeDefinition == typeof(IEnumerable<>)
                   || typeDefinition == typeof(IReadOnlyCollection<>)
                   || typeDefinition == typeof(IReadOnlyList<>)
                   || typeDefinition == typeof(ICollection<>)
                   || typeDefinition == typeof(IList<>)
                   || typeDefinition == typeof(List<>);
        }

        #endregion IsGenericCollection(是否泛型集合)

        #region GetPublicProperties(获取公共属性列表)

        /// <summary>
        /// 获取公共属性列表
        /// </summary>
        /// <param name="instance">实例</param>
        public static List<Item> GetPublicProperties(object instance)
        {
            var properties = instance.GetType().GetProperties();
            return properties.ToList().Select(t => new Item(t.Name, t.GetValue(instance))).ToList();
        }

        #endregion GetPublicProperties(获取公共属性列表)

        #region GetTopBaseType(获取顶级基类)

        /// <summary>
        /// 获取顶级基类
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public static Type GetTopBaseType<T>()
        {
            return GetTopBaseType(typeof(T));
        }

        /// <summary>
        /// 获取顶级基类
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static Type GetTopBaseType(Type type)
        {
            if (type == null)
            {
                return null;
            }

            if (type.IsInterface)
            {
                return type;
            }

            if (type.BaseType == typeof(object))
            {
                return type;
            }

            return GetTopBaseType(type.BaseType);
        }

        #endregion GetTopBaseType(获取顶级基类)

        #region IsDeriveClassFrom(判断当前类型是否可由指定类型派生)

        /// <summary>
        /// 判断当前类型是否可由指定类型派生
        /// </summary>
        /// <typeparam name="TBaseType">基类型</typeparam>
        /// <param name="type">当前类型</param>
        /// <param name="canAbstract">能否是抽象类</param>
        public static bool IsDeriveClassFrom<TBaseType>(Type type, bool canAbstract = false) => IsDeriveClassFrom(type, typeof(TBaseType), canAbstract);

        /// <summary>
        /// 判断当前类型是否可由指定类型派生
        /// </summary>
        /// <param name="type">当前类型</param>
        /// <param name="baseType">基类型</param>
        /// <param name="canAbstract">能否是抽象类</param>
        public static bool IsDeriveClassFrom(Type type, Type baseType, bool canAbstract = false)
        {
            Check.NotNull(type, nameof(type));
            Check.NotNull(baseType, nameof(baseType));

            return type.IsClass && (!canAbstract && !type.IsAbstract) && type.IsBaseOn(baseType);
        }

        #endregion IsDeriveClassFrom(判断当前类型是否可由指定类型派生)

        #region IsBaseOn(返回当前类型是否是指定基类的派生类)

        /// <summary>
        /// 返回当前类型是否是指定基类的派生类
        /// </summary>
        /// <typeparam name="TBaseType">基类型</typeparam>
        /// <param name="type">类型</param>
        public static bool IsBaseOn<TBaseType>(Type type) => IsBaseOn(type, typeof(TBaseType));

        /// <summary>
        /// 返回当前类型是否是指定基类的派生类
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="baseType">基类类型</param>
        public static bool IsBaseOn(Type type, Type baseType) => baseType.IsGenericTypeDefinition
            ? baseType.IsGenericAssignableFrom(type)
            : baseType.IsAssignableFrom(type);

        #endregion IsBaseOn(返回当前类型是否是指定基类的派生类)

        #region IsGenericAssignableFrom(判断当前泛型类型是否可由指定类型的实例填充)

        /// <summary>
        /// 判断当前泛型类型是否可由指定类型的实例填充
        /// </summary>
        /// <param name="genericType">泛型类型</param>
        /// <param name="type">指定类型</param>
        public static bool IsGenericAssignableFrom(Type genericType, Type type)
        {
            Check.NotNull(genericType, nameof(genericType));
            Check.NotNull(type, nameof(type));

            if (!genericType.IsGenericType)
            {
                throw new ArgumentException("该功能只支持泛型类型的调用，非泛型类型可使用 IsAssignableFrom 方法。");
            }

            var allOthers = new List<Type>() { type };
            if (genericType.IsInterface)
            {
                allOthers.AddRange(type.GetInterfaces());
            }

            foreach (var other in allOthers)
            {
                var cur = other;
                while (cur != null)
                {
                    if (cur.IsGenericType)
                    {
                        cur = cur.GetGenericTypeDefinition();
                    }

                    if (cur.IsSubclassOf(genericType) || cur == genericType)
                    {
                        return true;
                    }

                    cur = cur.BaseType;
                }
            }

            return false;
        }

        #endregion IsGenericAssignableFrom(判断当前泛型类型是否可由指定类型的实例填充)

        /// <summary>
        /// 获取应用有效程序集
        /// </summary>
        /// <param name="predicate">指定查询程序集</param>
        /// <returns>IEnumerable</returns>
        public static IEnumerable<Assembly> GetAssemblies(Func<Assembly, bool> predicate)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                 .Where(assembly =>
                     !assembly.FullName.StartsWith("System") &&
                     !assembly.FullName.StartsWith("Microsoft") &&
                     !assembly.FullName.StartsWith("netstandard") &&
                     !assembly.FullName.StartsWith("Pomelo")
                 );

            if (predicate != null)
                assemblies = assemblies.Where(predicate);

            return assemblies;
        }

        /// <summary>
        /// 获取入口程序集
        /// </summary>
        /// <returns></returns>
        public static Assembly GetEntryAssembly()
        {
            return Assembly.GetEntryAssembly();
        }

        /// <summary>
        /// 根据程序集名称获取运行时程序集
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static Assembly GetAssembly(string assemblyName)
        {
            // 加载程序集
            return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
        }

        /// <summary>
        /// 根据路径加载程序集
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Assembly LoadAssembly(string path)
        {
            if (!File.Exists(path)) return default;
            return AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
        }

        /// <summary>
        /// 通过流加载程序集
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Assembly LoadAssembly(MemoryStream assembly)
        {
            return Assembly.Load(assembly.ToArray());
        }

        /// <summary>
        /// 根据程序集名称、类型完整限定名获取运行时类型
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static Type GetType(string assemblyName, string typeFullName)
        {
            return GetAssembly(assemblyName).GetType(typeFullName);
        }

        /// <summary>
        /// 根据程序集和类型完全限定名获取运行时类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static Type GetType(Assembly assembly, string typeFullName)
        {
            return assembly.GetType(typeFullName);
        }

        /// <summary>
        /// 根据程序集和类型完全限定名获取运行时类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static Type GetType(MemoryStream assembly, string typeFullName)
        {
            return LoadAssembly(assembly).GetType(typeFullName);
        }

        /// <summary>
        /// 获取程序集名称
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetAssemblyName(Assembly assembly)
        {
            return assembly.GetName().Name;
        }

        /// <summary>
        /// 获取程序集名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetAssemblyName(Type type)
        {
            return GetAssemblyName(type.GetTypeInfo());
        }

        /// <summary>
        /// 获取程序集名称
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        public static string GetAssemblyName(TypeInfo typeInfo)
        {
            return GetAssemblyName(typeInfo.Assembly);
        }
    }
}