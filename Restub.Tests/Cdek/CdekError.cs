using System.Runtime.Serialization;

namespace Restub.Tests.Cdek
{
    /// <summary>
    /// Sample CDEK error message and code DTO.
    /// </summary>
    [DataContract]
    public class CdekError
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
