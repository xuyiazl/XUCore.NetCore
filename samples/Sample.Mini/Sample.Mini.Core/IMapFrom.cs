﻿using AutoMapper;
using System;
using System.Linq;
using System.Reflection;
using XUCore.Extensions;
using XUCore.Helpers;
using Sample.Mini.Core.Enums;

namespace Sample.Mini.Core
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Reflection.GetCurrentProjectAssemblies("Sample.Mini")
                .ForEach(a => ApplyMappingsFromAssembly(a));
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes(type => type.IsAbstract == false && type.GetInterfaces().Any(i => i.IsParticularGeneric(typeof(IMapFrom<>))));

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }

    public abstract class DtoBase<T> : IMapFrom<T>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public Status Status { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime Created_At { get; set; }
        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime? Updated_At { get; set; }
        /// <summary>
        /// 删除日期
        /// </summary>
        public DateTime? Deleted_At { get; set; }

        public virtual void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }

    public abstract class DtoKeyBase<T> : IMapFrom<T>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        public virtual void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}