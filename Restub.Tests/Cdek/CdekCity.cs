using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Restub.Tests.Cdek
{
    [DataContract]
    public class CdekCity
    {
        [DataMember(Name = "code")]
        public int Code { get; set; }

        [DataMember(Name = "city")]
        public string City { get; set; }

        [DataMember(Name = "fias_guid")]
        public string FiasGuid { get; set; }

        [DataMember(Name = "kladr_code")]
        public string KladrCode { get; set; }

        [DataMember(Name = "country_code")]
        public string CountryCode { get; set; }

        [DataMember(Name = "country")]
        public string Country { get; set; }

        [DataMember(Name = "region")]
        public string Region { get; set; }

        [DataMember(Name = "region_code")]
        public int RegionCode { get; set; }

        [DataMember(Name = "sub_region")]
        public string SubRegion { get; set; }

        [DataMember(Name = "postal_codes")]
        public List<string> PostalCodes { get; set; }

        [DataMember(Name = "longitude")]
        public double Longitude { get; set; }

        [DataMember(Name = "latitude")]
        public double Latitude { get; set; }

        [DataMember(Name = "time_zone")]
        public string TimeZone { get; set; }

        [DataMember(Name = "payment_limit")]
        public double PaymentLimit { get; set; }
    }
}
