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
    public static class ProjectsEx
    {

        /*public static async Task<bool> Create(this BitbucketClient client)
        {
            var list = new List<PullRequest>();
            var token = await client.GetAccessAsync();

            var response = await HttpHelper.POST<PullRequestApprove>(
            requestUri: "https://bitbucket.org/account/projects/create?owner=SettleApps",
            content: new StringContent($"{{\"content\": \"{content}\"}}", Encoding.UTF8, "application/json"),
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
        }*/

    }
}
