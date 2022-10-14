namespace Restub.Tests.Cdek
{
    /// <summary>
    /// Sample CDEK API authenticator using credentials.
    /// </summary>
    public class CdekAuthenticator : Authenticator<CdekClient, CdekAuthToken>
    {
        public CdekAuthenticator(CdekClient apiClient, CdekCredentials credentials)
            : base(apiClient, credentials)
        {
        }

        public override void InitAuthHeaders(CdekAuthToken authToken)
        {
            AuthHeaders["Authorization"] = $"Bearer {authToken.AccessToken}";
        }
    }
}
