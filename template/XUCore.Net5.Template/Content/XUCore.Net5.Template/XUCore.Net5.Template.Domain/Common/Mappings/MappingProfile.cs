using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XUCore.Extensions;

namespace XUCore.Net5.Template.Domain.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
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
}
