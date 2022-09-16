using System.Runtime.Serialization;
using Restub.DataContracts;

namespace Restub.Tests.Cdek
{
    [DataContract]
    public class CdekAuthToken : AuthToken
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; } // "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzY29wZSI6WyJvcmRlcjphbGw..."

        [DataMember(Name = "token_type")]
        public string TokenType { get; set; } // "bearer"

        [DataMember(Name = "expires_in")]
        public int ExpiresInSeconds { get; set; } // 3599

        [DataMember(Name = "scope")]
        public string Scope { get; set; } // "order:all payment:all"

        [DataMember(Name = "jti")]
        public string Jti { get; set; } // "9adca50a-..."
    }
}
