namespace Isitar.DependencyUpdater.Process
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Services;
    using Microsoft.Extensions.Logging;

    public class ProcessExecutor : IProcessExecutor
    {
        private readonly ILogger<ProcessExecutor> logger;

        public ProcessExecutor(ILogger<ProcessExecutor> logger)
        {
            this.logger = logger;
        }

        public async Task<ProcessOutput> Run(string workingDirectory, string command, string arguments, bool ensureSuccess, CancellationToken cancellationToken)
        {
            logger.LogDebug($"In path {workingDirectory}, running command: {command} {arguments}");

            System.Diagnostics.Process process;

            try
            {
                var processInfo = MakeProcessStartInfo(workingDirectory, command, arguments);
                process = Process.Start(processInfo);
            }
            catch (Exception ex)
            {
                logger.LogError($"External command failed:{command} {arguments}", ex);

                if (ensureSuccess)
                {
                    throw;
                }

                var message = $"Error starting external process for {command}: {ex.GetType().Name} {ex.Message}";
                return new ProcessOutput(string.Empty, message, 1);
            }

            if (process == null)
            {
                throw new Exception($"Could not start external process for {command}");
            }

            var outputs = await Task.WhenAll(
                process.StandardOutput.ReadToEndAsync(),
                process.StandardError.ReadToEndAsync()
            );

            var textOut = outputs[0];
            var errorOut = outputs[1];

            await process.WaitForExitAsync(cancellationToken);

            var exitCode = process.ExitCode;

            if (exitCode != 0)
            {
                var message = $"Command {command} failed with exit code: {exitCode}\n\n{textOut}\n\n{errorOut}";
                logger.LogDebug(message);

                if (ensureSuccess)
                {
                    throw new Exception(message);
                }
            }

            return new ProcessOutput(textOut, errorOut, exitCode);
        }

        private static ProcessStartInfo MakeProcessStartInfo(string workingDirectory, string command, string arguments)
        {
            return new ProcessStartInfo(command, arguments)
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                WorkingDirectory = workingDirectory
            };
        }
    }
}