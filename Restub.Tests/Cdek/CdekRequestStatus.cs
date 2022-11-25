using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Restub.Tests.Cdek
{
    /// <summary>
    /// Represents delivery order request status.
    /// EN: https://api-docs.cdek.ru/33828802.html
    /// RU: https://api-docs.cdek.ru/29923926.html
    /// </summary>
    [DataContract]
    public class CdekRequestStatus
    {
        [DataMember(Name = "request_uuid")]
        public string RequestUuid { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "date_time")]
        public DateTime DateTime { get; set; }

        [DataMember(Name = "errors")]
        public List<CdekError> Errors { get; set; }

        [DataMember(Name = "warnings")]
        public List<CdekError> Warnings { get; set; }
    }
}
