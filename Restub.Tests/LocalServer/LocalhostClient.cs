using System;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace Restub.Tests.LocalServer
{
    /// <summary>
    /// Local REST API server sample client, uses http basic authorization.
    /// </summary>
    public partial class LocalhostClient : RestubClient, IAuthenticator
    {
        public LocalhostClient(int port, string userName, string password)
            : base("http://127.0.0.1:" + port)
        {
            var authToken = GetEncoding().GetBytes($"{userName}:{password}");
            Authorization = $"Basic {Convert.ToBase64String(authToken)}";
        }

        private string Authorization { get; }

        public void Authenticate(IRestClient client, IRestRequest request) =>
            request.AddHeader(nameof(Authorization), Authorization);

        public string Hello() => Get<string>("/");

        public Task<string> HelloAsync() => GetAsync<string>("/");
    }
}
