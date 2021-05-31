namespace Isitar.DependencyUpdater.GitLab
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using Application.Common.Services;
    using Domain.Entities;

    public class GitLabApiImplementation : IPlatformApiImplementation
    {
        private class MergeRequestResponse
        {
            public long Id { get; set; }
            public long Iid { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }

        private readonly HttpClient httpClient;
        public PlatformType SupportedPlatformType => PlatformType.Gitlab;

        public GitLabApiImplementation(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        private string BaseUrl(Project project)
        {
            var apiBaseUrl = project.Platform.ApiBaseUrl.Trim('/');
            return $"{apiBaseUrl}/projects/{project.PlatformProjectId}";
        }

        private void AddAuthHeader(ref HttpRequestMessage requestMessage, Project project)
        {
            requestMessage.Headers.Add("PRIVATE-TOKEN", project.Platform.Token);
        }

        private async Task UpdateMergeRequestAsync(MergeRequestResponse existingMergeRequest, Project project, string message, CancellationToken cancellationToken)
        {
            var baseUrl = BaseUrl(project);
            var updateReq = new HttpRequestMessage(HttpMethod.Put, $"{baseUrl}/merge_requests/{existingMergeRequest.Iid}");
            AddAuthHeader(ref updateReq, project);
            var updateReqBody = new
            {
                id = project.PlatformProjectId,
                merge_request_iid = existingMergeRequest.Iid,
                description = existingMergeRequest.Description + Environment.NewLine + Environment.NewLine + message.Replace(Environment.NewLine, Environment.NewLine + Environment.NewLine)
            };
            var content = JsonContent.Create(updateReqBody);
            updateReq.Content = content;
            var resp = await httpClient.SendAsync(updateReq, cancellationToken);
            if (!resp.IsSuccessStatusCode)
            {
                throw new Exception(await resp.Content.ReadAsStringAsync(cancellationToken));
            }
        }

        private async Task CreateMergeRequestAsync(Project project, string title, string message, string sourceBranch, string targetBranch, CancellationToken cancellationToken)
        {
            var baseUrl = BaseUrl(project);
            var createRequest = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/merge_requests");
            AddAuthHeader(ref createRequest, project);
            var updateReqBody = new
            {
                id = project.PlatformProjectId,
                source_branch = sourceBranch,
                target_branch = targetBranch,
                title = title,
                description = message.Replace(Environment.NewLine, Environment.NewLine + Environment.NewLine),
                remove_source_branch = true,
                squash = true,
            };
            var content = JsonContent.Create(updateReqBody);
            createRequest.Content = content;
            var resp = await httpClient.SendAsync(createRequest, cancellationToken);
            if (!resp.IsSuccessStatusCode)
            {
                throw new Exception(await resp.Content.ReadAsStringAsync(cancellationToken));
            }
        }

        public async Task CreateOrUpdateMergeRequestAsync(Project project, string sourceBranch, string targetBranch, string title, string message, CancellationToken cancellationToken)
        {
            var baseUrl = BaseUrl(project);
            var existingMrRequest = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/merge_requests?state=opened&source_branch={HttpUtility.UrlEncode(sourceBranch)}&target_branch={HttpUtility.UrlEncode(targetBranch)}");
            AddAuthHeader(ref existingMrRequest, project);
            var existingMr = await httpClient.SendAsync(existingMrRequest, cancellationToken);
            if (!existingMr.IsSuccessStatusCode)
            {
                throw new Exception(await existingMr.Content.ReadAsStringAsync(cancellationToken));
            }

            var parsedMr = await existingMr.Content.ReadFromJsonAsync<MergeRequestResponse[]>(cancellationToken: cancellationToken);
            if (null != parsedMr && parsedMr.Length > 0)
            {
                await UpdateMergeRequestAsync(parsedMr[0], project, message, cancellationToken);
            }
            else
            {
                await CreateMergeRequestAsync(project, title, message, sourceBranch, targetBranch, cancellationToken);
            }
        }
    }
}