namespace Isitar.DependencyUpdater.Api.Contracts.V1
{
    public static class Routes
    {
        private const string Root = "api";
        private const string Version = "v1";
        private const string Base = Root + "/" + Version;

        public static class Platform
        {
            private const string PlatformBase = Base + "/platforms";
            public const string AllPlatforms = PlatformBase;
            public const string CreatePlatform = PlatformBase;
            public const string PlatformDetail = PlatformBase + "/{id}";
            public const string UpdatePlatform = PlatformBase + "/{id}";
            public const string DeletePlatform = PlatformBase + "/{id}";
            
            public const string CreateProject = PlatformBase + "/{id}/projects";
        }

        public static class Project
        {
            private const string ProjectBase = Base + "/Projects";
            public const string AllProjects = ProjectBase;
            public const string ProjectDetail = ProjectBase + "/{id}";
            public const string UpdateProject = ProjectBase + "/{id}";
            public const string DeleteProject = ProjectBase + "/{id}";
        }
    }
}