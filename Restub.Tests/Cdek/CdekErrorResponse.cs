using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Restub.DataContracts;

namespace Restub.Tests.Cdek
{
    /// <summary>
    /// Sample CDEK error response DTO implementing <see cref="IHasErrors"/> interface.
    /// </summary>
    [DataContract]
    public class CdekErrorResponse : IHasErrors
    {
        [DataMember(Name = "errors")]
        public List<CdekError> Errors { get; set; }

        public bool HasErrors() => Errors?.Any() ?? false;

        public string GetErrorMessage() =>
            string.Join(". ", (Errors?.Select(e => e.Message) ?? Enumerable.Empty<string>())
                .Distinct()
                .Where(m => !string.IsNullOrWhiteSpace(m)));
    }
}
