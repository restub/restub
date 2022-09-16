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
    internal class Authenticator<TClient, TAuthToken, TCredentials> : Authenticator
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

        // real API client will save an authentication header
        // private string AuthHeader { get; set; }

        public override void SetAuthToken(AuthToken authToken) =>
            SetAuthToken((TAuthToken)authToken);

        public virtual void SetAuthToken(TAuthToken authToken)
        {
            // real client will use something like:
            // AuthHeader = string.IsNullOrWhiteSpace(authToken?.AccessToken) ?
            //    null : // $"{authToken.TokenType ?? "Bearer"} "
            //    "Bearer " + authToken.AccessToken;
        }

        public override void Authenticate(IRestClient client, IRestRequest request)
        {
            base.Authenticate(client, request);

            // real API client: add authorization header if any
            // if (!string.IsNullOrWhiteSpace(AuthHeader))
            // {
            //    request.AddOrUpdateParameter("Authorization", AuthHeader, ParameterType.HttpHeader);
            // }
        }
    }
}
