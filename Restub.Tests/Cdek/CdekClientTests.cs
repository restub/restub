using System.Linq;
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

        private CdekClient Client { get; } = new CdekClient
        {
            Tracer = TestContext.Progress.WriteLine
        };

        [Test]
        public void CdekClientReturnsRegions()
        {
            var regions = Client.GetRegions(size: 10, page: 2);
            Assert.That(regions, Is.Not.Null);
            Assert.That(regions.Length, Is.EqualTo(10));

            // page is specified, but size is not specified
            Assert.That(() => Client.GetRegions(page: 3),
                Throws.TypeOf<CdekException>().With
                    .Message.Contains("[size] is empty").And
                    .Not.Message.Contains("Cannot deserialize").And
                    .Property(nameof(RestubException.ErrorResponseText))
                        .Contains("v2_field_is_empty"));
        }

        [Test]
        public void CdekClientReturnsCities()
        {
            var cities = Client.GetCities(new[] { "ru", "en" }, size: 3);
            Assert.That(cities, Is.Not.Null.And.Not.Empty);

            cities = Client.GetCities(city: "Гороховец");
            Assert.That(cities, Is.Not.Null.And.Not.Empty);
            Assert.That(cities.First(), Is.Not.Null);

            var city = cities.First();
            Assert.That(city.City, Is.EqualTo("Гороховец"));
            Assert.That(city.Code, Is.EqualTo(1143104).Or.EqualTo(2486));
        }

        [Test]
        public void CdekCreateDeliveryOrderNullThrowsInternalServerError()
        {
            // internal server error
            Assert.That(() => Client.CreateDeliveryOrder(null),
               Throws.TypeOf<CdekException>().With.Message.Contains("Internal"));
        }

        [Test]
        public void CdekCreateDeliveryOrderFailsWhenLocationAddressIsNotSpecified()
        {
            // to_location.address is empty
            Assert.That(() => Client.CreateDeliveryOrder(new CdekOrderRequest
            {
                DeliveryType = 1,
                Comment = "Test order",
                FromLocation = new CdekOrderRequest.Location
                {
                    City = "Москва",
                    Latitude = 55.789046m,
                    Longitude = 37.679157m,
                },
                ToLocation = new CdekOrderRequest.Location
                {
                    City = "Москва",
                    Latitude = 55.789011m,
                    Longitude = 37.682035m,
                },
                TariffCode = 480,
                Packages = new[]
                {
                    new CdekOrderRequest.Package
                    {
                        Number = "1",
                        Comments = "Test",
                        Weight = 1000,
                        Width = 10,
                        Height = 10,
                        Length = 10,
                    },
                },
                Sender = new CdekOrderRequest.Person
                {
                    Company = "Burattino",
                    Name = "Basilio",
                    Email = "basilio@example.com",
                    Phones = new[]
                    {
                        new CdekOrderRequest.Person.Phone { Number = "+71234567890" },
                    },
                },
                Recipient = new CdekOrderRequest.Person
                {
                    Company = "Burattino",
                    Name = "Alice",
                    Email = "alice@example.com",
                    Phones = new[]
                    {
                        new CdekOrderRequest.Person.Phone { Number = "+79876543210" },
                    },
                },
            }),
            Throws.TypeOf<CdekException>().With.Message.Contain("location.address").And.Message.Contain("empty"));
        }

        [Test]
        public void CdekCreateDeliveryOrderSucceeds()
        {
            var response = Client.CreateDeliveryOrder(new CdekOrderRequest
            {
                DeliveryType = 2,
                Comment = "Test order",
                FromLocation = new CdekOrderRequest.Location
                {
                    City = "Москва",
                    Address = "Русаковская улица, 31",
                    Latitude = 55.788576m,
                    Longitude = 37.678685m,
                },
                ToLocation = new CdekOrderRequest.Location
                {
                    City = "Москва",
                    Address = "Русаковская улица, 26к1",
                    Latitude = 55.789011m,
                    Longitude = 37.682035m,
                },
                TariffCode = 480,
                Packages = new[]
                {
                    new CdekOrderRequest.Package
                    {
                        Number = "1",
                        Comments = "Test",
                        Weight = 1000,
                        Width = 10,
                        Height = 10,
                        Length = 10,
                    },
                },
                Sender = new CdekOrderRequest.Person
                {
                    Company = "Burattino",
                    Name = "Basilio",
                    Email = "basilio@example.com",
                    Phones = new[]
                    {
                        new CdekOrderRequest.Person.Phone { Number = "+71234567890" },
                    },
                },
                Recipient = new CdekOrderRequest.Person
                {
                    Company = "Burattino",
                    Name = "Alice",
                    Email = "alice@example.com",
                    Phones = new[]
                    {
                        new CdekOrderRequest.Person.Phone { Number = "+79876543210" },
                    },
                },
            });

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Entity, Is.Not.Null);
            Assert.That(response.Entity.Uuid, Is.Not.Null.And.Not.Empty);
            Assert.That(response.Requests, Is.Not.Null.And.Not.Empty);
            Assert.That(response.Requests.First().RequestUuid, Is.Not.Null.And.Not.Empty);
        }
    }
}
