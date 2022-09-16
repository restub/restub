using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Exprest.DataContracts
{
    /// <summary>
    /// Represents error messages.
    /// </summary>
    [DataContract]
    public class ErrorResponse : IHasErrors
    {
        [DataMember(Name = "errors")]
        public List<Error> Errors { get; set; }

        public IEnumerable<Error> GetErrors() =>
            Errors ?? Enumerable.Empty<Error>();
    }
}
