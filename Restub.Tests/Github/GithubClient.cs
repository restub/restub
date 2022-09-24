using System.Threading.Tasks;

namespace Restub.Tests.Github
{
    /// <summary>
    /// Sample Github client for the unauthenticated API methods.
    /// </summary>
    public class GithubClient : RestubClient
    {
        public GithubClient() : base("https://api.github.com/")
        { 
        }

        public GithubUser GetUser(string name) =>
            Get<GithubUser>("users/{user}", r => r.AddUrlSegment("user", name));

        public GithubOrg[] GetUserOrgs(string name) =>
            Get<GithubOrg[]>("users/{user}/orgs", r => r.AddUrlSegment("user", name));

        public Task<GithubUser> GetUserAsync(string name) =>
            GetAsync<GithubUser>("users/{user}", r => r.AddUrlSegment("user", name));

        public Task<GithubOrg[]> GetUserOrgsAsync(string name) =>
            GetAsync<GithubOrg[]>("users/{user}/orgs", r => r.AddUrlSegment("user", name));
    }
}
