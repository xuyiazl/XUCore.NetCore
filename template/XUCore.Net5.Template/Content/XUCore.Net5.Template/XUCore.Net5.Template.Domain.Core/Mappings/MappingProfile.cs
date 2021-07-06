using AutoMapper;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using XUCore.Extensions;

namespace XUCore.Net5.Template.Domain.Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CurrentProjectAssemblies
                .ForEach(a => ApplyMappingsFromAssembly(a));
        }
        /// <summary>
        /// 当前项目程序集
        /// </summary>
        public List<Assembly> CurrentProjectAssemblies
        {
            get
            {
                var list = new List<Assembly>();
                var deps = DependencyContext.Default;
                var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package" && lib.Name.StartsWith("XUCore.Net5.Template"));
                foreach (var lib in libs)
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    list.Add(assembly);
                }
                return list;
            }
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
