using Restub.DataContracts;
using RestSharp;
using RestSharp.Authenticators;
using Headers = System.Collections.Generic.Dictionary<string, string>;

namespace Restub
{
    /// <summary>
    /// Abstract RestSharp authenticator that uses credentials.
    /// </summary>
    public abstract class Authenticator : IAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Authenticator"/> class.
        /// </summary>
        /// <param name="apiClient">Rest API client.</param>
        /// <param name="credentials">User's credentials.</param>
        public Authenticator(RestubClient apiClient, Credentials credentials)
        {
            State = AuthState.NotAuthenticated;
            BaseClient = apiClient;
            BaseCredentials = credentials;
        }

        protected RestubClient BaseClient { get; set; }

        protected Credentials BaseCredentials { get; set; }

        private AuthState State { get; set; }

        private enum AuthState
        {
            NotAuthenticated, InProgress, Authenticated
        }

        protected internal AuthToken BaseAuthToken { get; set; }

        /// <summary>
        /// Authentication headers populated by the <see cref="InitAuthHeaders(AuthToken)"/> method.
        /// </summary>
        protected Headers AuthHeaders { get; } = new Headers();

        /// <summary>
        /// Populates the <see cref="AuthHeaders"/> dictionary.
        /// </summary>
        /// <param name="authToken">Authentication token returned by the <see cref="Credentials"/>.</param>
        public abstract void InitAuthHeaders(AuthToken authToken);

        /// <summary>
        /// Initializes the authentication and populates the authentication headers.
        /// </summary>
        /// <param name="client">Rest API client.</param>
        /// <param name="request">Rest request to populate.</param>
        public virtual void Authenticate(IRestClient client, IRestRequest request)
        {
            // perform authentication request
            if (State == AuthState.NotAuthenticated)
            {
                State = AuthState.InProgress;
                BaseAuthToken = BaseCredentials.Authenticate(BaseClient);
                InitAuthHeaders(BaseAuthToken);
                State = AuthState.Authenticated;
            }

            // add authorization headers, if any
            foreach (var header in AuthHeaders)
            {
                request.AddOrUpdateParameter(header.Key, header.Value, ParameterType.HttpHeader);
            }
        }

        public virtual void Logout()
        {
            State = AuthState.NotAuthenticated;
            BaseAuthToken = null;
        }
    }
}
