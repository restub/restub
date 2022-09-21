using Restub.DataContracts;

namespace Restub
{
    /// <summary>
    /// Abstract credentials.
    /// </summary>
    /// <remarks>
    /// Usage: subclass it, add user+password or client+secret properties,
    /// implement Authenticate method.
    /// </remarks>
    /// <typeparam name="TClient">The type of the REST client.</typeparam>
    /// <typeparam name="TAuthToken">The type of the auth token DTO.</typeparam>
    public abstract class Credentials<TClient, TAuthToken> : Credentials
        where TClient : RestubClient
        where TAuthToken : AuthToken, new()
    {
        /// <inheritdoc/>
        public override AuthToken Authenticate(RestubClient client) =>
            Authenticate((TClient)client);

        /// <summary>
        /// Authenticates the client using account/secret pair.
        /// </summary>
        /// <param name="client">API client to make an authentication request.</param>
        public abstract TAuthToken Authenticate(TClient client);
            // real API client would call something like:
            // client.GetAuthToken(ClientAccount, ClientSecret);
            // or return new TAuthToken();
    }
}
