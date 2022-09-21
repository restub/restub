using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Restub.Tests.Pochta.Tariff
{
    [DataContract]
    public class TariffServiceItem
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "serviceon")]
        public List<int> Serviceon { get; set; }

        [DataMember(Name = "from")]
        public int From { get; set; }

        [DataMember(Name = "to")]
        public int To { get; set; }

        [DataMember(Name = "tariff")]
        public TariffAmount Amount { get; set; }
    }
}
