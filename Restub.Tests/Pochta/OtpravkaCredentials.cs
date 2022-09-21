using System;
using System.Text;

namespace Restub.Tests.Pochta
{
    public class OtpravkaCredentials : Credentials<OtpravkaClient, OtpravkaAuthToken>
    {
        public string AccessToken { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public override OtpravkaAuthToken Authenticate(OtpravkaClient client) => new OtpravkaAuthToken
        {
            AccessToken = AccessToken,
            AuthorizationKey = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{UserName}:{Password}")),
        };
    }
}
