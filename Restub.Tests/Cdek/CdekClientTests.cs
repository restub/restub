﻿using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Restub.Tests.Cdek
{
    [TestFixture]
    public class CdekClientTests
    {
        [Test]
        public void CdekClientAuthenticates()
        {
            var client = new CdekClient();
            var trace = new StringBuilder();
            client.Tracer += (format, args) => trace.AppendFormat(format, args);

            var regions = client.GetRegions(3);
            Assert.That(regions, Is.Not.Null);
            Assert.That(regions.Length, Is.EqualTo(3));

            // verify that authentication method is called and logged
            var log = trace.ToString();
            Assert.That(log, Is.Not.Empty);
            Assert.That(log, Contains.Substring("oauth/token?parameters"));
            Assert.That(log, Contains.Substring("Authorization = Bearer").Or.Contains("Authorization = bearer"));
            Assert.That(log, Contains.Substring("country_code"));
        }

        private CdekClient CdekClient { get; } = new CdekClient
        {
            Tracer = TestContext.Progress.WriteLine
        };

        [Test]
        public void CdekClientReturnsRegions()
        {
            var regions = CdekClient.GetRegions(size: 10, page: 2);
            Assert.That(regions, Is.Not.Null);
            Assert.That(regions.Length, Is.EqualTo(10));

            // page is specified, but size is not specified
            Assert.That(() => CdekClient.GetRegions(page: 3), Throws.TypeOf<RestubException>());
        }

        [Test]
        public void CdekClientReturnsCities()
        {
            var cities = CdekClient.GetCities(new[] { "ru", "en" });
            Assert.That(cities, Is.Not.Null.Or.Empty);

            cities = CdekClient.GetCities(city: "москва");
            Assert.That(cities, Is.Not.Null.Or.Empty);
            Assert.That(cities.First(), Is.Not.Null.Or.Empty);

            var city = cities.First();
            Assert.That(city.City, Is.EqualTo("Москва"));
            Assert.That(city.Code, Is.EqualTo(44));
        }
    }
}