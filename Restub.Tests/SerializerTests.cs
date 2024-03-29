﻿using System;
using System.Runtime.Serialization;
using Restub.Toolbox;
using Newtonsoft.Json;
using NUnit.Framework;
using Restub.DataContracts;

namespace Restub.Tests
{
    [TestFixture]
    public class SerializerTests
    {
        private IRestubSerializer Ser { get; } = new NewtonsoftSerializer();

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
            Assert.That(json, Is.EqualTo("{\"str\":\"string\",\"num\":123,\"dec\":456.78,\"date\":\"2022-08-11T13:06:00+03:00\"}"));

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
            Assert.That(json, Does.Match("\"2022\\-08\\-29T21\\:25\\:00\\+\\d\\d\\:00\""));

            var des = Deserialize(json, date);
            Assert.That(des, Is.Not.Null);
            Assert.That(des, Is.EqualTo(date));
        }

        [DataContract]
        public class NoTime
        {
            [DataMember(Name = "d"), JsonConverter(typeof(DateOnlyConverter))]
            public DateTime Date { get; set; }

            [DataMember(Name = "t"), JsonConverter(typeof(TimeOnlyConverter))]
            public DateTime Time { get; set; }
        }

        [Test]
        public void DateNoTimeSerialization()
        {
            var obj = new NoTime
            {
                Date = new DateTime(2022, 08, 11),
                Time = new DateTime(1, 1, 1, 12, 34, 00),
            };

            var json = Serialize(obj);
            Assert.That(json, Is.EqualTo("{\"d\":\"2022-08-11\",\"t\":\"12:34\"}"));

            var date = Deserialize<NoTime>(json);
            Assert.That(date, Is.Not.Null);
            Assert.That(date.Date, Is.EqualTo(new DateTime(2022, 08, 11)));
            Assert.That(date.Time.TimeOfDay, Is.EqualTo(new TimeSpan(12, 34, 00)));
        }

        [Test]
        public void AutoEnumSerializationNoDataContract()
        {
            // known value
            var json = Serialize(DayOfWeek.Friday);
            Assert.That(json, Is.EqualTo("5"));
            var dow = Deserialize<DayOfWeek>("5");
            Assert.That(dow, Is.EqualTo(DayOfWeek.Friday));

            // unknown value
            Assert.That(Serialize((DayOfWeek)123), Is.EqualTo("123"));
            Assert.That(Deserialize<DayOfWeek>("123"), Is.EqualTo((DayOfWeek)123));
        }

        [DataContract]
        public enum TestEnum
        {
            [EnumMember(Value = "hello")]
            HelloThere
        }

        [Test]
        public void AutoEnumSerializationDataContract()
        {
            // known value
            var json = Serialize(TestEnum.HelloThere);
            Assert.That(json, Is.EqualTo("\"hello\""));
            var dow = Deserialize<TestEnum>("\"hello\"");
            Assert.That(dow, Is.EqualTo(TestEnum.HelloThere));

            // unknown value
            Assert.That(Serialize((TestEnum)123), Is.EqualTo("123"));
            Assert.That(Deserialize<TestEnum>("123"), Is.EqualTo((TestEnum)123));
        }

        [Test]
        public void AutoEnumSerializationThrowsOnUnknownEnumMember()
        {
            Assert.That(() => Deserialize<TestEnum>("\"goodbye\""),
                Throws.TypeOf<JsonSerializationException>().With.Message
                    .Contains("Error converting value \"goodbye\" to type"));
        }

        [DefaultEnumMember("1")]
        public enum BrokenEnum
        {
            Unknown
        }

        [DataContract, DefaultEnumMember(Unknown)]
        public enum DefaultTestEnum
        {
            [EnumMember(Value = "unknown")]
            Unknown,

            [EnumMember(Value = "hello")]
            HelloThere
        }

        [Test]
        public void DefaultEnumMemberAttributeReturnsDefaultEnumMemberIfItsDefined()
        {
            Assert.That(DefaultEnumMemberAttribute.GetDefaultValue(typeof(object)), Is.Null);
            Assert.That(DefaultEnumMemberAttribute.GetDefaultValue(typeof(DayOfWeek)), Is.Null);
            Assert.That(DefaultEnumMemberAttribute.GetDefaultValue(typeof(BrokenEnum)), Is.Null);
            Assert.That(DefaultEnumMemberAttribute.GetDefaultValue(typeof(BrokenEnum?)), Is.Null);
            Assert.That(DefaultEnumMemberAttribute.GetDefaultValue(typeof(DefaultTestEnum)), Is.EqualTo(DefaultTestEnum.Unknown));
            Assert.That(DefaultEnumMemberAttribute.GetDefaultValue(typeof(DefaultTestEnum?)), Is.EqualTo(DefaultTestEnum.Unknown));
        }

        [Test]
        public void AutoEnumSerializationDoesntThrowOnUnknownEnumMemberIfDefaultValueIsSpecified()
        {
            Assert.That(Deserialize<DefaultTestEnum>("\"goodbye\""), Is.EqualTo(DefaultTestEnum.Unknown));
            Assert.That(() => Deserialize<DayOfWeek>("\"goodbye\""), Throws
                .TypeOf<JsonSerializationException>().With.Message
                    .Contains("Error converting value \"goodbye\" to type"));
        }

        [DataContract]
        public class IntBool
        {
            [DataMember(Name = "email"), JsonConverter(typeof(BoolIntConverter))]
            public bool EmailNotification { get; set; }

            [DataMember(Name = "sms"), JsonConverter(typeof(BoolIntConverter))]
            public bool? SmsNotification { get; set; }
        }

        [Test]
        [TestCase(false, null, "{\"email\":0,\"sms\":null}")]
        [TestCase(true, null, "{\"email\":1,\"sms\":null}")]
        [TestCase(false, false, "{\"email\":0,\"sms\":0}")]
        [TestCase(true, false, "{\"email\":1,\"sms\":0}")]
        [TestCase(false, true, "{\"email\":0,\"sms\":1}")]
        [TestCase(true, true, "{\"email\":1,\"sms\":1}")]
        public void BoolIntConverterWorksAsExpected(bool email, bool? sms, string result)
        {
            Assert.That(Serialize(new IntBool
            {
                EmailNotification = email,
                SmsNotification = sms
            }), Is.EqualTo(result));

            var dto = Deserialize<IntBool>(result);
            Assert.That(dto.EmailNotification, Is.EqualTo(email));
            Assert.That(dto.SmsNotification, Is.EqualTo(sms));
        }
    }
}