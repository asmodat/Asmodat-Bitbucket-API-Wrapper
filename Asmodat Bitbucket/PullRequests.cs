using AsmodatStandard.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AsmodatBitbucket
{
    public class PullRequests : Pagination<PullRequest>
    {
        public RequestError error;
    }

    public class Link
    {
        public string href;
    }

    public class SelfLink
    {
        public Link self;
    }

    public class PullRequestLinks
    {
        public Link decline;
        public Link commits;
        public Link self;
        public Link comments;
        public Link merge;
        public Link html;
        public Link activity;
        public Link diff;
        public Link approve;
        public Link statuses;
    }

    public class PullRequestCommit
    {
        public string hash;
        public SelfLink links;
    }

    public class RepositoryLinks
    {
        public Link self;
        public Link html;
        public Link avatar;
    }

    public class PullRequestRepository
    {
        public RepositoryLinks links;
        public string type;
        public string name;
        public string full_name;
        public string uuid;
    }

    public class PullRequestBranch
    {
        public string name;
    }

    public class PullRequestOrigin
    {
        public PullRequestCommit commit;
        public PullRequestRepository repository;
        public PullRequestBranch branch;
    }

    public class PullRequestAuthor
    {
        public string username;
        public string display_name;
        public string account_id;
        public UserLinks links;
        public string type;
        public string uuid; 
    }

    public class PullRequestSummary
    {
        public string raw;
        public string markup;
        public string html;
        public string type;
    }

    /// <summary>
    /// https://developer.atlassian.com/bitbucket/api/2/reference/resource/repositories/%7Busername%7D/%7Brepo_slug%7D/pullrequests
    /// </summary>
    public class PullRequest
    {
        public string description;
        public PullRequestLinks links;
        public string title;
        public bool close_source_branch;
        public string type;
        public int id;
        public PullRequestOrigin destination;
        public string created_on;
        public PullRequestSummary summary;
        public PullRequestOrigin source;
        public int comment_count;

        /// <summary>
        /// OPEN, DECLINED, SUPERSEDED, MERGED
        /// </summary>
        public string state;
         
        public int task_count;
        public string reason;
        public string updated_on;
        public PullRequestAuthor author;
        public object merge_commit;
        public object closed_by;
    }

    public class PullRequestApprove
    {
        public User user;
        public string role;
        public string participated_on;
        public string type;
        public bool approved;

        public RequestErrorInfo error;
    }
}
