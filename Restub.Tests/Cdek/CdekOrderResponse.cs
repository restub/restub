using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Restub.DataContracts;

namespace Restub.Tests.Cdek
{
    /// <summary>
    /// Represents created or deleted delivery order information.
    /// EN: https://api-docs.cdek.ru/33828802.html
    /// RU: https://api-docs.cdek.ru/29923926.html
    /// </summary>
    [DataContract]
    public class CdekOrderResponse : IHasErrors
    {
        [DataContract]
        public class CdekEntity
        {
            [DataMember(Name = "uuid")]
            public string Uuid { get; set; }
        }

        [DataMember(Name = "entity")]
        public CdekEntity Entity { get; set; }

        [DataMember(Name = "requests")]
        public List<CdekRequestStatus> Requests { get; set; }

        public IEnumerable<CdekError> GetErrors() =>
            from r in Requests ?? Enumerable.Empty<CdekRequestStatus>()
            from e in r.Errors ?? Enumerable.Empty<CdekError>()
            select e;

        public string GetErrorMessage() =>
            string.Join(". ", GetErrors().Select(e => e.Message)
                .Distinct()
                .Where(m => !string.IsNullOrWhiteSpace(m)));

        public bool HasErrors() => GetErrors().Any();
    }
}
