using System.Linq;
using NUnit.Framework;

namespace Restub.Tests.Github
{
    [TestFixture]
    public class GithubClientTests
    {
        private GithubClient Client { get; } = new GithubClient
        {
            Tracer = TestContext.Progress.WriteLine
        };

        [Test]
        public void GithubClientReturnsUsersOrganizations()
        {
            var orgs = Client.GetUserOrgs("yallie");
            Assert.That(orgs, Is.Not.Null.Or.Empty);

            var org = orgs.First(o => o.Login == "restub");
            Assert.That(org, Is.Not.Null.Or.Empty);
            Assert.That(org.Url, Is.EqualTo("https://api.github.com/orgs/restub"));
        }
    }
}
