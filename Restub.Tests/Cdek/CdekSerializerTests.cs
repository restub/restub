using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Restub.Tests.Cdek
{
    [TestFixture]
    public class CdekSerializerTests
    {
        private CdekSerializer Ser { get; } = new CdekSerializer();

        private string Serialize<T>(T dto) => Ser.Serialize(dto);

        private T Deserialize<T>(string json, T _ = default(T)) => Ser.Deserialize<T>(json);

        [Test]
        public void SerializationRoundtrip()
        {
            var obj = new
            {
                str = "string",
                num = 123,
                dec = 456.78,
                date = new DateTimeOffset(2022, 08, 11, 13, 06, 00, TimeSpan.FromHours(3)),
            };

            var json = Serialize(obj);
            Assert.That(json, Is.Not.Empty);
            Assert.That(json, Is.EqualTo("{\"str\":\"string\",\"num\":123,\"dec\":456.78,\"date\":\"2022-08-11T13:06:00+0300\"}"));

            var des = Deserialize(json, obj);
            Assert.That(des, Is.Not.Null);
            Assert.That(des, Is.EqualTo(obj));
        }

        [Test]
        public void DateTimeSerialization()
        {
            var date = new DateTime(2022, 08, 29, 21, 25, 00);
            var json = Serialize(date);
            Assert.That(json, Is.Not.Empty);

            // note: time zone can be different, i.e. 2022-08-29T21:25:00+0300 or +0700 or whatever
            Assert.That(json, Does.StartWith("\"2022-08-29T21:25:00+"));
            Assert.That(json, Does.EndWith("00\""));
            Assert.That(json, Does.Match("\"2022\\-08\\-29T21\\:25\\:00\\+\\d\\d00\""));

            var des = Deserialize(json, date);
            Assert.That(des, Is.Not.Null);
            Assert.That(des, Is.EqualTo(date));
        }
    }
}
