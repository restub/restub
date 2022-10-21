using System.Linq;
using System.Threading.Tasks;
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
        public void GithubClientReturnsUserByName()
        {
            var user = Client.GetUser("yallie");
            Assert.That(user, Is.Not.Null);

            Assert.That(user.ID, Is.EqualTo(672878));
            Assert.That(user.Login, Is.EqualTo("yallie"));
            Assert.That(user.Name, Is.EqualTo("Alexey Yakovlev"));
            Assert.That(user.Url, Is.EqualTo("https://api.github.com/users/yallie"));
        }

        [Test]
        public void GithubClientReturnsUsersOrganizations()
        {
            var orgs = Client.GetUserOrgs("yallie");
            Assert.That(orgs, Is.Not.Null.And.Not.Empty);

            var org = orgs.First(o => o.Login == "restub");
            Assert.That(org, Is.Not.Null);
            Assert.That(org.Url, Is.EqualTo("https://api.github.com/orgs/restub"));
        }

        [Test]
        public async Task GithubClientReturnsUserByNameAsync()
        {
            var user = await Client.GetUserAsync("yallie");
            Assert.That(user, Is.Not.Null);

            Assert.That(user.ID, Is.EqualTo(672878));
            Assert.That(user.Login, Is.EqualTo("yallie"));
            Assert.That(user.Name, Is.EqualTo("Alexey Yakovlev"));
            Assert.That(user.Url, Is.EqualTo("https://api.github.com/users/yallie"));
        }

        [Test]
        public async Task GithubClientReturnsUsersOrganizationsAsync()
        {
            var orgs = await Client.GetUserOrgsAsync("yallie");
            Assert.That(orgs, Is.Not.Null.And.Not.Empty);

            var org = orgs.First(o => o.Login == "restub");
            Assert.That(org, Is.Not.Null);
            Assert.That(org.Url, Is.EqualTo("https://api.github.com/orgs/restub"));
        }
    }
}
