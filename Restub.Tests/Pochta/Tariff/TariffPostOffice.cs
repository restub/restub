using System.Runtime.Serialization;

namespace Restub.Tests.Pochta.Tariff
{
    [DataContract]
    public class TariffPostOffice
    {
        [DataMember(Name = "index")]
        public int Index { get; set; }

        [DataMember(Name = "tp")]
        public int Tp { get; set; }

        [DataMember(Name = "type")]
        public int Type { get; set; }

        [DataMember(Name = "typei")]
        public int Typei { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "regionid")]
        public int Regionid { get; set; }

        [DataMember(Name = "regiono")]
        public long Regiono { get; set; }

        [DataMember(Name = "region-main")]
        public int RegionMain { get; set; }

        [DataMember(Name = "area-main")]
        public int AreaMain { get; set; }

        [DataMember(Name = "placeid")]
        public int Placeid { get; set; }

        [DataMember(Name = "placeo")]
        public long Placeo { get; set; }

        [DataMember(Name = "parent")]
        public int Parent { get; set; }

        [DataMember(Name = "root")]
        public int Root { get; set; }

        [DataMember(Name = "courier")]
        public int Courier { get; set; }

        [DataMember(Name = "pvz")]
        public int Pvz { get; set; }

        [DataMember(Name = "item-check-view")]
        public int ItemCheckView { get; set; }

        [DataMember(Name = "move")]
        public int Move { get; set; }

        [DataMember(Name = "weight-max")]
        public int WeightMax { get; set; }

        [DataMember(Name = "pack-max")]
        public int PackMax { get; set; }

        [DataMember(Name = "box")]
        public int Box { get; set; }
    }
}
