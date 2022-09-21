using Restub.DataContracts;

namespace Restub.Tests.Pochta.Otpravka
{
    public class OtpravkaAuthToken : AuthToken
    {
        public string AccessToken { get; set; }

        public string AuthorizationKey { get; set; }
    }
}
