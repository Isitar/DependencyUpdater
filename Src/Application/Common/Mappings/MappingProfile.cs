namespace Isitar.DependencyUpdater.Application.Common.Mappings
{
    using System;
    using System.Linq;
    using System.Reflection;
    using AutoMapper;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected void ApplyMappingsFromAssembly(params Assembly[] assemblies)
        {
            Func<Type, bool> iMapToPredicate = (i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapTo<>));
            Func<Type, bool> iMapFromPredicate = (i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>));
            Func<Type, bool> iIgnoreMappingPredicate = (i => i == typeof(IIgnoreMapping));

            var types = assemblies.SelectMany(assembly => assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i => iMapToPredicate(i) || iMapFromPredicate(i))
                            && !t.GetInterfaces().Any(iIgnoreMappingPredicate))
                .ToList()
            );

            const string methodName = nameof(IMapFrom<object>.Mapping);
            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod(methodName);
                var param = new object[] {this};

                // if default impl should be used if not overwritten
                if (null == methodInfo)
                {
                    var iMapTo = type.GetInterfaces().FirstOrDefault(iMapToPredicate);
                    if (null != iMapTo)
                    {
                        iMapTo.GetMethod(methodName)?.Invoke(instance, param);
                    }

                    var iMapFrom = type.GetInterfaces().FirstOrDefault(iMapFromPredicate);
                    if (null != iMapFrom)
                    {
                        iMapFrom.GetMethod(methodName)?.Invoke(instance, param);
                    }
                }
                else
                {
                    methodInfo.Invoke(instance, param);
                }
            }
        }
    }
}