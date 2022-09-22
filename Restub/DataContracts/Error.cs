using System.Runtime.Serialization;

namespace Restub.DataContracts
{
    /// <summary>
    /// Sample error message and code DTO.
    /// </summary>
    [DataContract]
    public class Error
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
