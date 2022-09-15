using System.Text;
using Exprest.DataContracts;
using NUnit.Framework;

namespace Exprest.Tests
{
    [TestFixture]
    public class AuthTests
    {
        [Test]
        public void Authenticate()
        {
            //var client = new TestClient();
            //var trace = new StringBuilder();
            //client.Tracer += (format, args) => trace.AppendFormat(format, args);

            //var regions = client.GetRegions(new RegionRequest { Size = 3 });
            //Assert.That(regions, Is.Not.Null);
            //Assert.That(regions.Length, Is.EqualTo(3));

            //var log = trace.ToString();
            //Assert.That(log, Is.Not.Empty);
            //Assert.That(log, Contains.Substring("oauth/token?parameters"));
            //Assert.That(log, Contains.Substring("Authorization = Bearer").Or.Contains("Authorization = bearer"));
            //Assert.That(log, Contains.Substring("country_code"));
        }
    }
}