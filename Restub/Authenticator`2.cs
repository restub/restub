using Restub.DataContracts;

namespace Restub
{
    /// <summary>
    /// Base authenticator that uses credentials.
    /// </summary>
    /// <remarks>
    /// Usage: subclass, override SetAuthToken, Authenticate and, optionally, Logout methods.
    /// </remarks>
    public abstract class Authenticator<TClient, TAuthToken> : Authenticator
        where TClient : RestubClient
        where TAuthToken : AuthToken
    {
        public Authenticator(TClient apiClient, Credentials<TClient, TAuthToken> credentials)
            : base(apiClient, credentials)
        {
        }

        protected TClient Client => (TClient)BaseClient;

        protected Credentials<TClient, TAuthToken> Credentials =>
            (Credentials<TClient, TAuthToken>)BaseCredentials;

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
    }
}
