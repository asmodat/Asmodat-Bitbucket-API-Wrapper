using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AsmodatStandard.Extensions;
using AsmodatStandard.Extensions.Collections;
using AsmodatStandard.Extensions.Threading;
using Newtonsoft.Json;

namespace AsmodatBitbucket
{
    public class AccessToken
    {
        public string access_token;
        public string scopes;
        public int expires_in;
        public string refresh_token;
        public string token_type;
        public string error_description;
        public string error;

        [JsonIgnore]
        public long CreationTime;

        [JsonIgnore]
        public bool IsInvalid
            => !error.IsNullOrEmpty() || !error_description.IsNullOrEmpty();

        [JsonIgnore]
        public bool IsExpired
            => EspiresInSeconds <= 0;

        [JsonIgnore]
        public int EspiresInSeconds
        {
            get
            {
                var remainingTicks = TimeSpan.FromSeconds(expires_in).Ticks - (DateTime.UtcNow.Ticks - CreationTime);
                if (remainingTicks <= 0)
                    return 0;
                else
                    return (int)new TimeSpan(remainingTicks).TotalSeconds;
            }
        }
    }

    public class BitbucketClient
    {
        public readonly string Url = "https://bitbucket.org";
        public readonly string ApiUrl = "https://api.bitbucket.org";
        private readonly string _accessTokenEndpoint = "site/oauth2/access_token";
        private readonly string _key;
        private readonly string _secret;
        private AccessToken _accessToken;

        private static SemaphoreSlim ss = new SemaphoreSlim(1, 1);

        public BitbucketClient(string key, string secret)
        {
            _key = key;
            _secret = secret;
        }

        public Task<AccessToken> GetAccessAsync()
            => ss.Lock(async () =>
            {
                if (_accessToken != null && _accessToken.EspiresInSeconds > 180)
                    return _accessToken;

                if (_accessToken != null && _accessToken.EspiresInSeconds > 60)
                   return _accessToken = await RefreshAccessTokenAsync(_accessToken);

                return _accessToken = await GetAccessTokenAsync();
            });

        private async Task<AccessToken> GetAccessTokenAsync()
        {
            var time = DateTime.UtcNow.Ticks;
            var accessToken = await HttpHelper.POST<AccessToken>(
                requestUri: $"{this.Url}/{_accessTokenEndpoint}",
                content:
                "grant_type=" + "client_credentials".UriEncode() +
                "&client_id=" + _key.UriEncode() +
                "&client_secret=" + _secret.UriEncode(),
                encoding: Encoding.UTF8,
                mediaType: "application/x-www-form-urlencoded");

            if (accessToken.IsInvalid)
                throw new Exception($"Failed Authentication, Response: {accessToken.JsonSerialize()}");

            accessToken.CreationTime = time;
            return accessToken;
        }

        private async Task<AccessToken> RefreshAccessTokenAsync(AccessToken accessToken)
        {
            if (accessToken.IsInvalid)
                throw new Exception($"Can't refresh, access token is invalid: {accessToken.JsonSerialize()}");

            if (accessToken.IsExpired)
                throw new Exception($"Can't refresh, access token expired: {accessToken.JsonSerialize()}");

            var time = DateTime.UtcNow.Ticks;
            var newAccessToken = await HttpHelper.POST<AccessToken>(
                requestUri: $"{this.Url}/{_accessTokenEndpoint}",
                content:
                "grant_type=" + "refresh_token".UriEncode() +
                "&refresh_token=" + accessToken.refresh_token.UriEncode() +
                "&client_id=" + _key.UriEncode() +
                "&client_secret=" + _secret.UriEncode(),
                encoding: Encoding.UTF8,
                mediaType: "application/x-www-form-urlencoded");

            if (newAccessToken.IsInvalid)
                throw new Exception($"Failed Token Refresh, Response: {newAccessToken.JsonSerialize()}");

            newAccessToken.CreationTime = time;
            return accessToken;
        }
    }
}
