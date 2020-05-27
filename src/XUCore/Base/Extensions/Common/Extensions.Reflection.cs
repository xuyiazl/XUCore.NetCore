using System;
using System.Collections.Generic;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace XUCore.Extensions
{
    /// <summary>
    /// 系统扩展 - 反射
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 获取实例上的属性值
        /// </summary>
        /// <param name="member">成员信息</param>
        /// <param name="instance">成员所在的类实例</param>
        /// <returns></returns>
        public static object GetPropertyValue(this MemberInfo member, object instance)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return instance.GetType().GetProperty(member.Name)?.GetValue(instance);
        }

        /// <summary>
        /// 实体深拷贝，防止部分业务需要修改实体后 自动同步到所有实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopyByReflect<T>(this T obj) where T : class
        {
            //System.String类型似乎比较特殊，复制它的所有字段，并不能复制它本身 
            //不过由于System.String的不可变性，即使指向同一对象，也无所谓 
            //而且.NET里本来就用字符串池来维持 
            if (obj == null || obj.GetType() == typeof(string))
                return obj;
            object newObj = null;
            try
            {
                //尝试调用默认构造函数 
                newObj = Activator.CreateInstance(obj.GetType());
            }
            catch
            {
                //失败的话，只好枚举构造函数了 
                foreach (ConstructorInfo ci in obj.GetType().GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                {
                    try
                    {
                        ParameterInfo[] pis = ci.GetParameters();
                        object[] objs = new object[pis.Length];
                        for (int i = 0; i < pis.Length; i++)
                        {
                            if (pis[i].ParameterType.IsValueType)
                                objs[i] = Activator.CreateInstance(pis[i].ParameterType);
                            else
                                //参数类型可能是抽象类或接口，难以实例化 
                                //我能想到的就是枚举应用程序域里的程序集，找到实现了该抽象类或接口的类 
                                //但显然过于复杂了 
                                objs[i] = null;
                        }
                        newObj = ci.Invoke(objs);
                        //无论调用哪个构造函数，只要成功就行了 
                        break;
                    }
                    catch
                    {
                    }
                }
            }
            foreach (FieldInfo fi in obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                if (fi.FieldType.IsValueType || fi.FieldType == typeof(string))
                    fi.SetValue(newObj, fi.GetValue(obj));
                else
                    fi.SetValue(newObj, DeepCopyByReflect(fi.GetValue(obj)));
            }
            //基类的私有实例字段在子类里检索不到，但它仍占据子类对象的内存空间 
            Deep(newObj, obj);
            return (T)newObj;
        }

        private static void Deep(object newObj, object obj)
        {
            for (Type father = newObj.GetType().BaseType; father != typeof(object); father = father.BaseType)
            {
                foreach (FieldInfo fi in father.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    //只需要处理私有字段，因为非私有成员已经在子类处理过了 
                    if (fi.IsPrivate)
                    {
                        if (fi.FieldType.IsValueType || fi.FieldType == typeof(string))
                        {
                            fi.SetValue(newObj, fi.GetValue(obj));
                        }
                        else
                        {
                            fi.SetValue(newObj, DeepCopyByReflect(fi.GetValue(obj)));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 反射复制相同属性值
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="Target">目标类型</typeparam>
        /// <param name="sources">源数据</param>
        /// <returns></returns>
        public static IList<Target> CopyByReflect<TSource, Target>(this IList<TSource> sources)
        {
            IList<Target> targetList = new List<Target>();

            if (sources == null || sources.Count == 0)
                return targetList;

            PropertyInfo[] _targetProperties = typeof(Target).GetProperties();
            PropertyInfo[] _sourceProperties = typeof(TSource).GetProperties();

            foreach (var source in sources)
            {
                Target model = Activator.CreateInstance<Target>();

                foreach (var _target in _targetProperties)
                {
                    foreach (var _source in _sourceProperties)
                    {
                        if (_target.Name == _source.Name && _target.PropertyType == _source.PropertyType)
                        {
                            _target.SetValue(model, _source.GetValue(source, null), null);
                            break;
                        }
                    }
                }

                targetList.Add(model);
            }

            return targetList;
        }

        /// <summary>
        /// 反射复制相同属性值
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="Target">目标类型</typeparam>
        /// <param name="source">源数据</param>
        /// <returns></returns>
        public static Target CopyByReflect<TSource, Target>(this TSource source)
        {
            Target model = default(Target);
            if (source == null) return model;

            PropertyInfo[] _targetProperties = typeof(Target).GetProperties();
            PropertyInfo[] _sourceProperties = typeof(TSource).GetProperties();

            model = Activator.CreateInstance<Target>();

            foreach (var _target in _targetProperties)
            {
                foreach (var _source in _sourceProperties)
                {
                    if (_target.Name == _source.Name && _target.PropertyType == _source.PropertyType)
                    {
                        _target.SetValue(model, _source.GetValue(source, null), null);
                        break;
                    }
                }
            }
            return model;
        }
    }
}