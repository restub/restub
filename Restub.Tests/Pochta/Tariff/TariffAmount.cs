using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Restub.Tests.Pochta.Tariff
{
    [DataContract]
    public class TariffAmount
    {
        [DataMember(Name = "val")]
        public int Amount { get; set; }

        [DataMember(Name = "valnds")]
        public int AmountVat { get; set; }
    }
}
