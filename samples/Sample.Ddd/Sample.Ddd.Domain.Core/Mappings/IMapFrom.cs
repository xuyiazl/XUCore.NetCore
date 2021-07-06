using AutoMapper;

namespace Sample.Ddd.Domain.Core.Mappings
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
