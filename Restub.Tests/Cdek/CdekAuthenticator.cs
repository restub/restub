using RestSharp;

namespace Restub.Tests.Cdek
{
    /// <summary>
    /// Sample CDEK API authenticator using credentials.
    /// </summary>
    internal class CdekAuthenticator : Authenticator<CdekClient, CdekAuthToken, CdekCredentials>
    {
        public CdekAuthenticator(CdekClient apiClient, CdekCredentials credentials)
            : base(apiClient, credentials)
        {
        }

        private string AuthHeader { get; set; }

        public override void SetAuthToken(CdekAuthToken authToken) =>
            AuthHeader = string.IsNullOrWhiteSpace(authToken?.AccessToken) ?
                null : "Bearer " + authToken.AccessToken;

        public override void Authenticate(IRestClient client, IRestRequest request)
        {
            base.Authenticate(client, request);

            // add authorization header, if any
            if (!string.IsNullOrWhiteSpace(AuthHeader))
            {
                request.AddOrUpdateParameter("Authorization", AuthHeader, ParameterType.HttpHeader);
            }
        }
    }
}
