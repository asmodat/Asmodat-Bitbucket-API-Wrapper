using AsmodatStandard.Extensions;
using AsmodatStandard.Extensions.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AsmodatBitbucket
{
    public static class PullRequestsEx
    {
        public static async Task<PullRequest[]> GetPullRequests(this BitbucketClient client, string username, string repo_slug)
        {
            var pullRequests = new List<PullRequest>();
            var url = $"{client.ApiUrl}/2.0/repositories/{username}/{repo_slug}/pullrequests";

            var list = new List<PullRequest>();
            PullRequests response = null;
            do
            {
                var nexUrl = response == null ? url : response.next;
                var token = await client.GetAccessAsync();

                response = await HttpHelper.GET<PullRequests>(
                requestUri: nexUrl,
                ensureStatusCode: null,
                defaultHeaders: new(string, string)[] { ("Authorization", $"Bearer {token.access_token}") });

                if (response?.error != null)
                    throw new Exception($"Failed Get request: '{nexUrl}', respose: {response.JsonSerialize()}");

                list.AddRange(response.values);

            } while (!response.IsLast);

            return list.ToArray();
        }

        public static async Task<PullRequest> GetPullRequest(
            this BitbucketClient client, 
            string username,
            string repo_slug,
            string sourceBranchName, 
            string destinationBranchName,
            string state)
        {
            var result = await client.GetPullRequests(username, repo_slug);
            if (result.IsNullOrEmpty())
                return null;

            return result.FirstOrDefault(x => x.source.branch.name == sourceBranchName &&
            x.destination.branch.name == destinationBranchName &&
            x.state == state);
        }

        public static async Task<bool> PullRequestApprove(this BitbucketClient client, PullRequest pullRequest)
        {
            var list = new List<PullRequest>();
            var token = await client.GetAccessAsync();

            var response = await HttpHelper.POST<PullRequestApprove>(
            requestUri: pullRequest.links.approve.href,
            content: null,
            ensureStatusCode: null,
            defaultHeaders: new(string, string)[] { ("Authorization", $"Bearer {token.access_token}") });

            if (response?.error?.message.Contains("already approved") == true)
                if (response?.error != null)
                {
                    if (response?.error?.message != null && response.error.message.Contains("already approved"))
                        return true;

                    throw new Exception($"Failed Get request: '{pullRequest.links.approve.href}', respose: {response.JsonSerialize()}");
                }

            return response.approved;
        }

        public static async Task<bool> PullRequestUnApprove(this BitbucketClient client, PullRequest pullRequest)
        {
            var list = new List<PullRequest>();
            var token = await client.GetAccessAsync();

            var response = await HttpHelper.CURL(
                HttpMethod.Delete,
                pullRequest.links.approve.href,
                content: null,
                ensureStatusCode: null,
                defaultHeaders: new(string, string)[] { ("Authorization", $"Bearer {token.access_token}") });

            if (response.Response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var content = await response.Response.Content.ReadAsStringAsync();
                var errorResponse = content.JsonDeserialize<RequestError>();

                if (errorResponse?.error.message != null &&
                    (errorResponse.error.message.Contains("haven't approved") ||
                     errorResponse.error.message.Contains("have not approved")))
                    return true;

                if (errorResponse?.error != null)
                    throw new Exception($"Failed Delete request: '{pullRequest.links.approve.href}', respose: {response.JsonSerialize()}");
            }

            return response.Response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        public static async Task PullRequestComment(this BitbucketClient client, PullRequest pullRequest, string content)
        {
            var list = new List<PullRequest>();
            var token = await client.GetAccessAsync();

            var requestUri = pullRequest.links.comments.href.Replace("api.bitbucket.org/2.0/", "bitbucket.org/!api/1.0/");
            var response = await HttpHelper.POST(
            requestUri: requestUri,
            content: new StringContent($"{{\"content\": \"{content}\"}}", Encoding.UTF8, "application/json"),
            ensureStatusCode: System.Net.HttpStatusCode.OK,
            defaultHeaders: new(string, string)[] { ("Authorization", $"Bearer {token.access_token}") });
        }
    }
}
