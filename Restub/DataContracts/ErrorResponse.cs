using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Restub.DataContracts
{
    /// <summary>
    /// Sample error response DTO implementing <see cref="IHasErrors"/> interface.
    /// </summary>
    [DataContract]
    public class ErrorResponse : IHasErrors
    {
        [DataMember(Name = "errors")]
        public List<Error> Errors { get; set; }

        public bool HasErrors() => Errors?.Any() ?? false;

        public string GetErrorMessage() =>
            string.Join(". ", (Errors?.Select(e => e.Message) ?? Enumerable.Empty<string>())
                .Distinct()
                .Where(m => !string.IsNullOrWhiteSpace(m)));
    }
}
