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
        /// <summary>
        /// Initializes a new instance of the <see cref="Authenticator{TClient, TAuthToken}"/> class.
        /// </summary>
        /// <param name="apiClient">Rest API client.</param>
        /// <param name="credentials">User's credentials.</param>
        public Authenticator(TClient apiClient, Credentials<TClient, TAuthToken> credentials)
            : base(apiClient, credentials)
        {
        }

        /// <summary>
        /// Gets the REST client.
        /// </summary>
        protected TClient Client => (TClient)BaseClient;

        /// <summary>
        /// Gets the credentials.
        /// </summary>
        protected Credentials<TClient, TAuthToken> Credentials =>
            (Credentials<TClient, TAuthToken>)BaseCredentials;

        /// <summary>
        /// Gets or sets the authentication token returned by <see cref="Credentials"/>.
        /// </summary>
        protected internal TAuthToken AuthToken
        {
            get => (TAuthToken)BaseAuthToken;
            set => BaseAuthToken = value;
        }

        /// <summary>
        /// Populates the AuthHeaders dictionary.
        /// </summary>
        /// <param name="authToken">Authentication token returned by the <see cref="Credentials"/>.</param>
        public override sealed void InitAuthHeaders(AuthToken authToken) =>
            InitAuthHeaders((TAuthToken)authToken);

        /// <summary>
        /// Populates the AuthHeaders dictionary.
        /// </summary>
        /// <param name="authToken">Authentication token returned by the <see cref="Credentials"/>.</param>
        public abstract void InitAuthHeaders(TAuthToken authToken);
            // real client would do something like this:
            // AuthHeaders["Authorization"] = "Bearer " + authToken.AccessToken;
    }
}
