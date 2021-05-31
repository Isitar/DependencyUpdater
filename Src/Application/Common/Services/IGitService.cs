namespace Isitar.DependencyUpdater.Application.Common.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface IGitService
    {
        public string WorkingDirectory { get; set; }
        public Task CloneRepositoryAsync(string url, Platform platform, CancellationToken cancellationToken);
        public Task<bool> BranchExistsAsync(string name, CancellationToken cancellationToken);
        public Task CheckoutBranchAsync(string name, bool create, CancellationToken cancellationToken);
        public Task AddFilesAsync(string[] files, CancellationToken cancellationToken);
        public Task AddFilesAsync(CancellationToken cancellationToken);

        public Task CommitAddedFilesAsync(string message, CancellationToken cancellationToken);
        public Task PushAsync(string remote, string branch, CancellationToken cancellationToken);

        public Task MergeAsync(string branch, CancellationToken cancellationToken);
        public Task DeleteBranchAsync(string branch, bool remote, CancellationToken cancellationToken);
        public Task ResetHardAsync(CancellationToken cancellationToken);
    }
}