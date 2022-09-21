using System.Runtime.Serialization;

namespace Restub.Tests.Pochta.Tariff
{
    [DataContract]
    public class TariffPackageGroup
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
