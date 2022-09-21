﻿using System.Runtime.Serialization;

namespace Restub.Tests.Pochta
{
    [DataContract]
    public class OtpravkaAddressRequest
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "original-address")]
        public string OriginalAddress { get; set; }
    }
}
