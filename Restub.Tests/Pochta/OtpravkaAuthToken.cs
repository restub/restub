using Restub.DataContracts;

namespace Restub.Tests.Pochta
{
    public class OtpravkaAuthToken : AuthToken
    {
        public string AccessToken { get; set; }

        public string AuthorizationKey { get; set; }
    }
}
