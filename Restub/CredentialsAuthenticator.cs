using Exprest.DataContracts;
using RestSharp;
using RestSharp.Authenticators;

namespace Exprest
{
    /// <summary>
    /// RestSharp authenticator that uses credentials.
    /// </summary>
    /// <remarks>
    /// Usage: subclass, override SetAuthToken, Authenticate and, optionally, Logout methods.
    /// </remarks>
    internal class CredentialsAuthenticator : IAuthenticator
    {
        public CredentialsAuthenticator(ExprestClient apiClient, Credentials credentials)
        {
            State = AuthState.NotAuthenticated;
            Client = apiClient;
            Credentials = credentials;
        }

        private ExprestClient Client { get; set; }

        private Credentials Credentials { get; set; }

        private AuthState State { get; set; }

        private enum AuthState
        {
            NotAuthenticated, InProgress, Authenticated
        }

        protected internal AuthToken AuthToken { get; set; }

        // real API client will save an authentication header
        // private string AuthHeader { get; set; }

        public virtual void SetAuthToken(AuthToken authToken)
        {
            // real client will use something like:
            // AuthHeader = string.IsNullOrWhiteSpace(authToken?.AccessToken) ?
            //    null : // $"{authToken.TokenType ?? "Bearer"} "
            //    "Bearer " + authToken.AccessToken;
        }

        public virtual void Authenticate(IRestClient client, IRestRequest request)
        {
            // perform authentication request
            if (State == AuthState.NotAuthenticated)
            {
                State = AuthState.InProgress;
                AuthToken = Credentials.Authenticate(Client);
                SetAuthToken(AuthToken);
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
            AuthToken = null;
        }
    }
}
