using System;
using System.Runtime.Serialization;
using Restub.Toolbox;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Restub.Tests
{
    [TestFixture]
    public class SerializerTests
    {
        private string Serialize<T>(T dto)
        {
            var ss = new NewtonsoftSerializer();
            return ss.Serialize(dto);
        }

        private T Deserialize<T>(string json, T _ = default(T))
        {
            var ss = new NewtonsoftSerializer();
            return ss.Deserialize<T>(json);
        }

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

        [DataContract]
        public class NoTime
        {
            [DataMember(Name = "d"), JsonConverter(typeof(DateOnlyConverter))]
            public DateTime Date { get; set; }
        }

        [Test]
        public void DateNoTimeSerialization()
        {
            var obj = new NoTime { Date = new DateTime(2022, 08, 11) };
            var json = Serialize(obj);
            Assert.That(json, Is.EqualTo("{\"d\":\"2022-08-11\"}"));

            var date = Deserialize<NoTime>(json);
            Assert.That(date, Is.Not.Null);
            Assert.That(date.Date, Is.EqualTo(new DateTime(2022, 08, 11)));
        }

        [Test]
        public void EnumSerialization()
        {
            // known value
            var json = Serialize(DayOfWeek.Friday);
            Assert.That(json, Is.EqualTo("\"Friday\""));
            var dow = Deserialize<DayOfWeek>("\"Friday\"");
            Assert.That(dow, Is.EqualTo(DayOfWeek.Friday));

            // unknown value
            Assert.That(Serialize((DayOfWeek)123), Is.EqualTo("123"));
            Assert.That(Deserialize<DayOfWeek>("123"), Is.EqualTo((DayOfWeek)123));
        }
    }
}