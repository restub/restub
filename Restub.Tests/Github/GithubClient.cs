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
    }
}
