using System.Text;
using NUnit.Framework;

namespace Restub.Tests.Pochta.Otpravka
{
    [TestFixture]
    public class OtpravkaClientTests : TestBase
    {
        private OtpravkaClient Client { get; } = new OtpravkaClient(new OtpravkaCredentials
        {
            AccessToken = Env("OTPRAVKA_ACCESS_TOKEN"),
            UserName = Env("OTPRAVKA_USER_EMAIL"),
            Password = Env("OTPRAVKA_USER_PASSWORD"),
        })
        {
            Tracer = TestContext.Progress.WriteLine
        };

        [Test]
        public void OtpravkaClientAuthorizesUsingUserEmail()
        {
            var sb = new StringBuilder();
            var client = new OtpravkaClient(new OtpravkaCredentials
            {
                AccessToken = Env("OTPRAVKA_ACCESS_TOKEN"),
                UserName = Env("OTPRAVKA_USER_EMAIL"),
                Password = Env("OTPRAVKA_USER_PASSWORD"),
            })
            {
                Tracer = (fmt, args) => sb.AppendFormat(fmt, args)
            };

            var address = client.CleanAddress("Москва, Варшавское шоссе, 37");
            Assert.That(address, Is.Not.Null);
            Assert.That(address.Index, Is.EqualTo("117105"));

            var log = sb.ToString();
            Assert.That(log.Length, Is.GreaterThan(0));
            Assert.That(log, Does.Contain("Authorization = AccessToken"));
            Assert.That(log, Does.Contain("X-User-Authorization"));
            Assert.That(log, Does.Contain("<- OK 200 (OK) https://otpravka-api.pochta.ru/1.0/clean/address"));
        }

        [Test]
        public void OtpravkaClientAuthorizesUsingUserPhone()
        {
            var sb = new StringBuilder();
            var client = new OtpravkaClient(new OtpravkaCredentials
            {
                AccessToken = Env("OTPRAVKA_ACCESS_TOKEN"),
                UserName = Env("OTPRAVKA_USER_PHONE"),
                Password = Env("OTPRAVKA_USER_PASSWORD"),
            })
            {
                Tracer = (fmt, args) => sb.AppendFormat(fmt, args)
            };

            var address = client.CleanAddress("Москва, Варшавское шоссе, 37");
            Assert.That(address, Is.Not.Null);
            Assert.That(address.Index, Is.EqualTo("117105"));

            var log = sb.ToString();
            Assert.That(log.Length, Is.GreaterThan(0));
            Assert.That(log, Does.Contain("Authorization = AccessToken"));
            Assert.That(log, Does.Contain("X-User-Authorization"));
            Assert.That(log, Does.Contain("<- OK 200 (OK) https://otpravka-api.pochta.ru/1.0/clean/address"));
        }

        [Test]
        public void OtpravkaClientAddressCleanup()
        {
            var address = Client.CleanAddress("Москва, Варшавское шоссе, 37");
            Assert.That(address, Is.Not.Null);
            Assert.That(address.AddressType, Is.EqualTo("DEFAULT"));
            Assert.That(address.AddressGuid, Is.EqualTo("990231d5-4bd1-4323-997a-217002c4094e"));
            Assert.That(address.QualityCode, Is.EqualTo("GOOD"));
            Assert.That(address.ValidationCode, Is.EqualTo("VALIDATED"));
            Assert.That(address.Index, Is.EqualTo("117105"));
            Assert.That(address.Region, Is.EqualTo("г Москва"));
            Assert.That(address.Place, Is.EqualTo("г Москва"));
            Assert.That(address.Street, Is.EqualTo("ш Варшавское"));
            Assert.That(address.House, Is.EqualTo("37"));
            Assert.That(address.RegionGuid, Is.EqualTo("0c5b2444-70a0-4932-980c-b4dc0d3f02b5"));
            Assert.That(address.PlaceGuid, Is.EqualTo("0c5b2444-70a0-4932-980c-b4dc0d3f02b5"));
            Assert.That(address.StreetGuid, Is.EqualTo("8fc06b0b-5de3-4a72-9e6f-9e0647a37a66"));
        }

        [Test]
        public void OtpravkaClientAddressBatchCleanup()
        {
            var addresses = Client.CleanAddress("Москва, Варшавское шоссе, 37", "ул. Мясницкая, д. 26, г. Москва");
            Assert.That(addresses, Is.Not.Null);
            Assert.That(addresses.Length, Is.EqualTo(2));

            var address = addresses[0];
            Assert.That(address.AddressType, Is.EqualTo("DEFAULT"));
            Assert.That(address.AddressGuid, Is.EqualTo("990231d5-4bd1-4323-997a-217002c4094e"));
            Assert.That(address.QualityCode, Is.EqualTo("GOOD"));
            Assert.That(address.ValidationCode, Is.EqualTo("VALIDATED"));
            Assert.That(address.Index, Is.EqualTo("117105"));
            Assert.That(address.Region, Is.EqualTo("г Москва"));
            Assert.That(address.Place, Is.EqualTo("г Москва"));
            Assert.That(address.Street, Is.EqualTo("ш Варшавское"));
            Assert.That(address.House, Is.EqualTo("37"));
            Assert.That(address.RegionGuid, Is.EqualTo("0c5b2444-70a0-4932-980c-b4dc0d3f02b5"));
            Assert.That(address.PlaceGuid, Is.EqualTo("0c5b2444-70a0-4932-980c-b4dc0d3f02b5"));
            Assert.That(address.StreetGuid, Is.EqualTo("8fc06b0b-5de3-4a72-9e6f-9e0647a37a66"));
            Assert.That(address.OriginalAddress, Is.EqualTo("Москва, Варшавское шоссе, 37"));

            address = addresses[1];
            Assert.That(address.AddressType, Is.EqualTo("DEFAULT"));
            Assert.That(address.AddressGuid, Is.EqualTo("c511f9b0-5117-11ec-87e6-1bcdc4503f64"));
            Assert.That(address.QualityCode, Is.EqualTo("GOOD"));
            Assert.That(address.ValidationCode, Is.EqualTo("VALIDATED"));
            Assert.That(address.Index, Is.EqualTo("101000"));
            Assert.That(address.Region, Is.EqualTo("г Москва"));
            Assert.That(address.Place, Is.EqualTo("г Москва"));
            Assert.That(address.Street, Is.EqualTo("ул Мясницкая"));
            Assert.That(address.House, Is.EqualTo("26"));
            Assert.That(address.RegionGuid, Is.EqualTo("0c5b2444-70a0-4932-980c-b4dc0d3f02b5"));
            Assert.That(address.PlaceGuid, Is.EqualTo("0c5b2444-70a0-4932-980c-b4dc0d3f02b5"));
            Assert.That(address.StreetGuid, Is.EqualTo("757b544e-3c93-424c-b717-6f9813f123a9"));
            Assert.That(address.OriginalAddress, Is.EqualTo("ул. Мясницкая, д. 26, г. Москва"));
        }

        [Test]
        public void OtpravkaFullNameCleanup()
        {
            var person = Client.CleanFullName("Христофор Бонифатиевич Врунгель");
            Assert.That(person, Is.Not.Null);
            Assert.That(person.QualityCode, Is.EqualTo("NOT_SURE"));
            Assert.That(person.OriginalFullName, Is.EqualTo("Христофор Бонифатиевич Врунгель"));
            Assert.That(person.Name, Is.EqualTo("Христофор"));
            Assert.That(person.MiddleName, Is.EqualTo("Бонифатиевич"));
            Assert.That(person.Surname, Is.EqualTo("Врунгель"));

            person = Client.CleanFullName("Иван Рылов");
            Assert.That(person, Is.Not.Null);
            Assert.That(person.QualityCode, Is.EqualTo("NOT_SURE"));
            Assert.That(person.OriginalFullName, Is.EqualTo("Иван Рылов"));
            Assert.That(person.Name, Is.EqualTo("Иван"));
            Assert.That(person.MiddleName, Is.Null);
            Assert.That(person.Surname, Is.EqualTo("Рылов"));

            person = Client.CleanFullName("Иванка Петкова");
            Assert.That(person, Is.Not.Null);
            Assert.That(person.QualityCode, Is.EqualTo("NOT_SURE"));
            Assert.That(person.OriginalFullName, Is.EqualTo("Иванка Петкова"));
            Assert.That(person.Name, Is.EqualTo("Иванка"));
            Assert.That(person.MiddleName, Is.Null);
            Assert.That(person.Surname, Is.EqualTo("Петкова"));

            person = Client.CleanFullName("Марфа Васильевна");
            Assert.That(person, Is.Not.Null);
            Assert.That(person.QualityCode, Is.EqualTo("NOT_SURE"));
            Assert.That(person.OriginalFullName, Is.EqualTo("Марфа Васильевна"));
            Assert.That(person.Name, Is.EqualTo("Марфа"));
            Assert.That(person.MiddleName, Is.EqualTo("Васильевна"));
            Assert.That(person.Surname, Is.Null);
        }

        [Test]
        public void OtpravkaFullNameBatchCleanup()
        {
            var people = Client.CleanFullName("Христофор Бонифатьевич Врунгель", "Иван Рылов",
                "Иванка Петкова", "Марфа Васильевна", "Достоевский Константин Константинович");

            Assert.That(people, Is.Not.Null.Or.Empty);
            Assert.That(people.Length, Is.EqualTo(5));

            var person = people[0];
            Assert.That(person, Is.Not.Null);
            Assert.That(person.QualityCode, Is.EqualTo("NOT_SURE"));
            Assert.That(person.OriginalFullName, Is.EqualTo("Христофор Бонифатьевич Врунгель"));
            Assert.That(person.Name, Is.EqualTo("Христофор"));
            Assert.That(person.MiddleName, Is.EqualTo("Бонифатьевич"));
            Assert.That(person.Surname, Is.EqualTo("Врунгель"));

            person = people[1];
            Assert.That(person, Is.Not.Null);
            Assert.That(person.QualityCode, Is.EqualTo("NOT_SURE"));
            Assert.That(person.OriginalFullName, Is.EqualTo("Иван Рылов"));
            Assert.That(person.Name, Is.EqualTo("Иван"));
            Assert.That(person.MiddleName, Is.Null);
            Assert.That(person.Surname, Is.EqualTo("Рылов"));

            person = people[2];
            Assert.That(person, Is.Not.Null);
            Assert.That(person.QualityCode, Is.EqualTo("NOT_SURE"));
            Assert.That(person.OriginalFullName, Is.EqualTo("Иванка Петкова"));
            Assert.That(person.Name, Is.EqualTo("Иванка"));
            Assert.That(person.MiddleName, Is.Null);
            Assert.That(person.Surname, Is.EqualTo("Петкова"));

            person = people[3];
            Assert.That(person, Is.Not.Null);
            Assert.That(person.QualityCode, Is.EqualTo("NOT_SURE"));
            Assert.That(person.OriginalFullName, Is.EqualTo("Марфа Васильевна"));
            Assert.That(person.Name, Is.EqualTo("Марфа"));
            Assert.That(person.MiddleName, Is.EqualTo("Васильевна"));
            Assert.That(person.Surname, Is.Null);

            person = people[4];
            Assert.That(person, Is.Not.Null);
            Assert.That(person.QualityCode, Is.EqualTo("EDITED"));
            Assert.That(person.OriginalFullName, Is.EqualTo("Достоевский Константин Константинович"));
            Assert.That(person.Name, Is.EqualTo("Константин"));
            Assert.That(person.MiddleName, Is.EqualTo("Константинович"));
            Assert.That(person.Surname, Is.EqualTo("Достоевский"));
        }
    }
}
