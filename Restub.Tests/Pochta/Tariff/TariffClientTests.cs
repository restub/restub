using System;
using NUnit.Framework;

namespace Restub.Tests.Pochta.Tariff
{
    [TestFixture]
    public class TariffClientTests
    {
        private TariffClient Client { get; } = new TariffClient
        {
            Tracer = TestContext.Progress.WriteLine
        };

        [Test]
        public void TariffClientCalculatesTariff()
        {
            var tariff = Client.Calculate(new TariffRequest
            {
                Object = 23030,
                From = 344038,
                To = 115162,
                Weight = 1000,
                Date = DateTime.Now,
                Time = "0223",
            });

            Assert.That(tariff, Is.Not.Null);
            Assert.That(tariff.FromIndex, Is.EqualTo(344038));
            Assert.That(tariff.ToIndex, Is.EqualTo(115162));
        }
    }
}
