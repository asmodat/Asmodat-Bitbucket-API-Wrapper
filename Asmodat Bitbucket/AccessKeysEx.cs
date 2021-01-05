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
    public static class AccessKeysEx
    {
        /*public static async Task PullRequestComment(this BitbucketClient client, PullRequest pullRequest, string content)
        {
            var list = new List<PullRequest>();
            var token = await client.GetAccessAsync();
            var url = $"{client.ApiUrl}/2.0/repositories/{username}/{repo_slug}/pullrequests";


            var requestUri = pullRequest.links.comments.href.Replace("api.bitbucket.org/2.0/", "bitbucket.org/!api/1.0/");
            var response = await HttpHelper.POST(
            requestUri: requestUri,
            content: new StringContent($"{{\"content\": \"{content}\"}}", Encoding.UTF8, "application/json"),
            ensureStatusCode: System.Net.HttpStatusCode.OK,
            defaultHeaders: new(string, string)[] { ("Authorization", $"Bearer {token.access_token}") });
        }*/
    }
}
