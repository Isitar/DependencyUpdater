namespace NugetUpdater
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Isitar.DependencyUpdater.Application.Common.Services;
    using Isitar.DependencyUpdater.Domain.Entities;
    using Microsoft.Extensions.Logging;

    public class NugetProjectUpdater : IProjectUpdater
    {
        private class PackageUpdate
        {
            public string PackageName { get; set; }
            public string FromVersion { get; set; }
            public string ToVersion { get; set; }
        }

        private readonly IProcessExecutor processExecutor;
        private readonly ILogger<NugetProjectUpdater> logger;
        public ProjectType SupportedProjectType => ProjectType.Dotnet;
        public string WorkingDirectory { get; set; }

        public NugetProjectUpdater(IProcessExecutor processExecutor, ILogger<NugetProjectUpdater> logger)
        {
            this.processExecutor = processExecutor;
            this.logger = logger;
        }

        public async Task<IEnumerable<string>> UpdateProjectDependenciesAsync(Project project, string workingDirectory, CancellationToken cancellationToken)
        {
            await processExecutor.Run(WorkingDirectory, "dotnet", "restore", true, cancellationToken);

            var projectOutdatedDependencies = new Dictionary<(string file, string name), IList<PackageUpdate>>();

            foreach (var projectFile in Directory.GetFiles(WorkingDirectory, "*.csproj", SearchOption.AllDirectories))
            {
                var processOutput = await processExecutor.Run(WorkingDirectory, "dotnet", $"list {projectFile} package --outdated", true, cancellationToken);
                var textLines = processOutput.Output.Split(Environment.NewLine);
                var line = 0;
                string currentProject = null;

                while (line < textLines.Length)
                {
                    var currentLine = textLines[line];
                    if (currentLine.StartsWith("The given project `"))
                    {
                        currentProject = Regex.Match(currentLine, "The given project `(.*)` has no").Groups[0].Value;
                        if (!projectOutdatedDependencies.ContainsKey((projectFile, currentProject)))
                        {
                            projectOutdatedDependencies[(projectFile, currentProject)] = new List<PackageUpdate>();
                        }
                    }

                    if (currentLine.StartsWith("Project `"))
                    {
                        currentProject = Regex.Match(currentLine, "Project `(.*)` has the following").Groups[1].Value;
                        if (!projectOutdatedDependencies.ContainsKey((projectFile, currentProject)))
                        {
                            projectOutdatedDependencies[(projectFile, currentProject)] = new List<PackageUpdate>();
                        }
                    }

                    if (currentLine.Trim().StartsWith(">"))
                    {
                        if (null == currentProject)
                        {
                            logger.LogError("No project found but update is here");
                            throw new Exception("No project found but update is here " + currentLine);
                        }

                        var matches = Regex.Match(currentLine, "> (\\S*)\\s*(\\S*)\\s*(\\S*)\\s*(\\S*)");
                        projectOutdatedDependencies[(projectFile, currentProject)].Add(new PackageUpdate
                        {
                            PackageName = matches.Groups[1].Value,
                            FromVersion = matches.Groups[2].Value,
                            ToVersion = matches.Groups[4].Value,
                        });
                    }

                    line++;
                }
            }

            var retVal = new List<string>();


            foreach (var kvp in projectOutdatedDependencies)
            {
                foreach (var packageToUpdate in kvp.Value)
                {
                    await processExecutor.Run(workingDirectory, "dotnet", $"add {kvp.Key.file} package -v {packageToUpdate.ToVersion} {packageToUpdate.PackageName}", true, cancellationToken);
                    retVal.Add($"Updated {packageToUpdate.PackageName} from {packageToUpdate.FromVersion} to {packageToUpdate.ToVersion} in {kvp.Key.name}");
                }
            }

            return retVal;
        }
    }
}