using System.Runtime.Serialization;

namespace Restub.Tests.Pochta
{
    [DataContract]
    public class OtpravkaAddress
    {
        [DataMember(Name = "address-guid")]
        public string AddressGuid { get; set; }

        [DataMember(Name = "address-type")]
        public string AddressType { get; set; }

        [DataMember(Name = "area")]
        public string Area { get; set; }

        [DataMember(Name = "building")]
        public string Building { get; set; }

        [DataMember(Name = "corpus")]
        public string Corpus { get; set; }

        [DataMember(Name = "hotel")]
        public string Hotel { get; set; }

        [DataMember(Name = "house")]
        public string House { get; set; }

        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "index")]
        public string Index { get; set; }

        [DataMember(Name = "letter")]
        public string Letter { get; set; }

        [DataMember(Name = "location")]
        public string Location { get; set; }

        [DataMember(Name = "num-address-type")]
        public string NumAddressType { get; set; }

        [DataMember(Name = "original-address")]
        public string OriginalAddress { get; set; }

        [DataMember(Name = "place")]
        public string Place { get; set; }

        [DataMember(Name = "place-guid")]
        public string PlaceGuid { get; set; }

        [DataMember(Name = "quality-code")]
        public string QualityCode { get; set; }

        [DataMember(Name = "region")]
        public string Region { get; set; }

        [DataMember(Name = "region-guid")]
        public string RegionGuid { get; set; }

        [DataMember(Name = "room")]
        public string Room { get; set; }

        [DataMember(Name = "slash")]
        public string Slash { get; set; }

        [DataMember(Name = "street")]
        public string Street { get; set; }

        [DataMember(Name = "street-guid")]
        public string StreetGuid { get; set; }

        [DataMember(Name = "validation-code")]
        public string ValidationCode { get; set; }
    }
}
