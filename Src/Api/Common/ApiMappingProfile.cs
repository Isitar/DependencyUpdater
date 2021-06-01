namespace Isitar.DependencyUpdater.Api.Common
{
    using System.Reflection;
    using Application.Common.Mappings;

    public class ApiMappingProfile : MappingProfile
    {
        public ApiMappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}