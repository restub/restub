using System.Runtime.Serialization;

namespace Restub.Tests.Pochta.Tariff
{
    /// <summary>
    /// Sample Tariff API client error DTO.
    /// </summary>
    [DataContract]
    public class TariffError
    {
        [DataMember(Name = "msg")]
        public string Message { get; set; }

        [DataMember(Name = "type")]
        public int Type { get; set; }

        [DataMember(Name = "code")]
        public int Code { get; set; }
    }
}
