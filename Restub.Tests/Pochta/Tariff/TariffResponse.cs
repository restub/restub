using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Restub.Tests.Pochta.Tariff
{
    [DataContract]
    public class TariffResponse
    {
        [DataMember(Name = "version_api")]
        public int VersionApi { get; set; }

        [DataMember(Name = "version")]
        public string Version { get; set; }

        [DataMember(Name = "caption")]
        public string Caption { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "mailtype")]
        public int Mailtype { get; set; }

        [DataMember(Name = "mailctg")]
        public int MailCategory { get; set; }

        [DataMember(Name = "directctg")]
        public int DirectCategory { get; set; }

        [DataMember(Name = "from")]
        public int FromIndex { get; set; }

        [DataMember(Name = "to")]
        public int ToIndex { get; set; }

        [DataMember(Name = "weight")]
        public int Weight { get; set; }

        [DataMember(Name = "group")]
        public TariffPackageGroup Group { get; set; }

        [DataMember(Name = "date")] // date only
        public string Date { get; set; }

        [DataMember(Name = "time")] // time only
        public string Time { get; set; }

        [DataMember(Name = "date-first")]
        public int DateFirst { get; set; }

        [DataMember(Name = "postoffice")]
        public List<TariffPostOffice> PostOffice { get; set; }

        [DataMember(Name = "transtype")]
        public int Transtype { get; set; }

        [DataMember(Name = "transname")]
        public string Transname { get; set; }

        [DataMember(Name = "items")]
        public List<TariffServiceItem> Items { get; set; }

        [DataMember(Name = "isgroup")]
        public int Isgroup { get; set; }

        [DataMember(Name = "isdogovor")]
        public int Isdogovor { get; set; }

        [DataMember(Name = "ground")]
        public TariffAmount Amount { get; set; }

        [DataMember(Name = "paymoney")]
        public int Paymoney { get; set; }

        [DataMember(Name = "paymoneynds")]
        public int Paymoneynds { get; set; }

        [DataMember(Name = "pay")]
        public int Pay { get; set; }

        [DataMember(Name = "paynds")]
        public int Paynds { get; set; }

        [DataMember(Name = "ndsrate")]
        public int Ndsrate { get; set; }

        [DataMember(Name = "nds")]
        public int Nds { get; set; }

        [DataMember(Name = "place")]
        public string Place { get; set; }
    }
}
