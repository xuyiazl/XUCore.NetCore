using AutoMapper;
using System;
using System.Linq;
using System.Reflection;
using XUCore.Extensions;
using XUCore.Helpers;

namespace XUCore.Template.Ddd.Domain.Core.Mappings
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Reflection.GetCurrentProjectAssemblies("XUCore.Template.Ddd")
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

    public abstract class DtoBase<T> : DtoKeyBase<T>
    {
        /// <summary>
        /// 数据状态
        /// </summary>
        public Status Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
        ///// <summary>
        ///// 创建人
        ///// </summary>
        //public string CreatedAtUserId { get; set; }
        ///// <summary>
        ///// 最后更新时间
        ///// </summary>
        //public DateTime? UpdatedAt { get; set; }
        ///// <summary>
        ///// 最后更新人
        ///// </summary>
        //public string UpdatedAtUserId { get; set; }
        ///// <summary>
        ///// 删除时间
        ///// </summary>
        //public DateTime? DeletedAt { get; set; }
        ///// <summary>
        ///// 删除人
        ///// </summary>
        //public string DeletedAtUserId { get; set; }
    }

    public abstract class DtoKeyBase<T> : IMapFrom<T>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public string Id { get; set; }

        public virtual void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
