namespace Isitar.DependencyUpdater.Application.Common.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    public class ProcessOutput
    {
        public ProcessOutput(string output, string errorOutput, int exitCode)
        {
            Output = output;
            ErrorOutput = errorOutput;
            ExitCode = exitCode;
        }

        public string Output { get; }
        public string ErrorOutput { get; }
        public int ExitCode { get; }

        public bool Success => ExitCode == 0;
    }
    
    public interface IProcessExecutor
    {
        Task<ProcessOutput> Run(string workingDirectory, string command, string arguments, bool ensureSuccess, CancellationToken cancellationToken);
    }
}