using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Restub.Toolbox;

namespace Restub.Tests.Pochta.Tariff
{
    [DataContract]
    public class TariffRequest
    {
        [DataMember(Name = "object")]
        public TariffObjectType Object { get; set; } = TariffObjectType.ParcelCourierEms;

        [DataMember(Name = "from")]
        public int From { get; set; } = 344038;

        [DataMember(Name = "to")]
        public int To { get; set; } = 115162;

        [DataMember(Name = "weight")]
        public int Weight { get; set; } = 1000;

        [DataMember(Name = "group")]
        public int Group { get; set; } = 0;

        [DataMember(Name = "closed")]
        public int Closed { get; set; } = 1;

        [DataMember(Name = "date"), JsonConverter(typeof(DateOnlyConverter))]
        public DateTime Date { get; set; } = DateTime.Today;

        [DataMember(Name = "time"), JsonConverter(typeof(DateOnlyConverter))]
        public string Time { get; set; } = "2128";
    }
}
