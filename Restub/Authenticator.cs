using Restub.DataContracts;
using RestSharp;
using RestSharp.Authenticators;

namespace Restub
{
    /// <summary>
    /// Abstract RestSharp authenticator that uses credentials.
    /// </summary>
    public abstract class Authenticator : IAuthenticator
    {
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

        // real API client will save an authentication header
        // private string AuthHeader { get; set; }

        public abstract void SetAuthToken(AuthToken authToken);

        public virtual void Authenticate(IRestClient client, IRestRequest request)
        {
            // perform authentication request
            if (State == AuthState.NotAuthenticated)
            {
                State = AuthState.InProgress;
                BaseAuthToken = BaseCredentials.Authenticate(BaseClient);
                SetAuthToken(BaseAuthToken);
                State = AuthState.Authenticated;
            }

            // real API client: add authorization header if any
            // if (!string.IsNullOrWhiteSpace(AuthHeader))
            // {
            //    request.AddOrUpdateParameter("Authorization", AuthHeader, ParameterType.HttpHeader);
            // }
        }

        public virtual void Logout()
        {
            State = AuthState.NotAuthenticated;
            BaseAuthToken = null;
        }
    }
}
