using System.Runtime.Serialization;

namespace Restub.Tests.Github
{
    public class GithubOrg
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public string Login { get; set; }
        public string Description { get; set; }
    }
}
