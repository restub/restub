namespace Restub.Tests.Pochta.Otpravka
{
    public class OtpravkaAuthenticator : Authenticator<OtpravkaClient, OtpravkaAuthToken>
    {
        public OtpravkaAuthenticator(OtpravkaClient apiClient, OtpravkaCredentials credentials)
            : base(apiClient, credentials)
        {
        }

        public override void InitAuthHeaders(OtpravkaAuthToken authToken)
        {
            AuthHeaders["Authorization"] = $"AccessToken {authToken.AccessToken}";
            AuthHeaders["X-User-Authorization"] = $"Basic {authToken.AuthorizationKey}";
        }
    }
}
