using System.Collections.Generic;
using RestSharp;

namespace Restub.Tests.Pochta.Otpravka
{
    public class OtpravkaAuthenticator : Authenticator<OtpravkaClient, OtpravkaAuthToken, OtpravkaCredentials>
    {
        public OtpravkaAuthenticator(OtpravkaClient apiClient, OtpravkaCredentials credentials)
            : base(apiClient, credentials)
        {
        }

        private Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        public override void SetAuthToken(OtpravkaAuthToken authToken)
        {
            Headers["Authorization"] = $"AccessToken {authToken.AccessToken}";
            Headers["X-User-Authorization"] = $"Basic {authToken.AuthorizationKey}";
            //Headers["Content-Type"] = "application/json;charset=UTF-8";
        }

        public override void Authenticate(IRestClient client, IRestRequest request)
        {
            base.Authenticate(client, request);

            // add authorization headers, if any
            foreach (var header in Headers)
            {
                request.AddOrUpdateParameter(header.Key, header.Value, ParameterType.HttpHeader);
            }
        }
    }
}
