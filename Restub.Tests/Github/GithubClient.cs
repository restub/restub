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

        public GithubOrg[] GetUserOrgs(string user) =>
            Get<GithubOrg[]>("users/{user}/orgs", r => r.AddUrlSegment("user", user));
    }
}
