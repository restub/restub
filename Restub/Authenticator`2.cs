using Restub.DataContracts;
using RestSharp;

namespace Restub
{
    /// <summary>
    /// Base authenticator that uses credentials.
    /// </summary>
    /// <remarks>
    /// Usage: subclass, override SetAuthToken, Authenticate and, optionally, Logout methods.
    /// </remarks>
    public abstract class Authenticator<TClient, TAuthToken, TCredentials> : Authenticator
        where TClient : RestubClient
        where TCredentials : Credentials<TClient, TAuthToken>, new()
        where TAuthToken : AuthToken, new()
    {
        public Authenticator(TClient apiClient, TCredentials credentials)
            : base(apiClient, credentials)
        {
        }

        protected TClient Client => (TClient)BaseClient;

        protected TCredentials Credentials => (TCredentials)BaseCredentials;

        protected internal TAuthToken AuthToken
        {
            get => (TAuthToken)BaseAuthToken;
            set => BaseAuthToken = value;
        }

        // real API client would save an authentication header
        // private string AuthHeader { get; set; }

        public override sealed void SetAuthToken(AuthToken authToken) =>
            SetAuthToken((TAuthToken)authToken);

        public abstract void SetAuthToken(TAuthToken authToken);
            // real client would do something like this:
            // AuthHeader = "Bearer " + authToken.AccessToken;

        public override void Authenticate(IRestClient client, IRestRequest request)
        {
            base.Authenticate(client, request);

            // real API client would add authorization headers
            // if (!string.IsNullOrWhiteSpace(AuthHeader))
            // {
            //    request.AddOrUpdateParameter("Authorization", AuthHeader, ParameterType.HttpHeader);
            // }
        }
    }
}
